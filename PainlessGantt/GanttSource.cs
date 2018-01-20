using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using PainlessGantt.Construction;
using PainlessGantt.Internal;
using YamlDotNet.Serialization;

namespace PainlessGantt
{
    /// <summary>
    /// <see cref="IGanttSource"/> インターフェイスを拡張します。
    /// </summary>
    public static class GanttSource
    {
        /// <summary>
        /// 指定した YAML ファイルを読み込み、<see cref="IGanttSource"/> オブジェクトを構築します。
        /// </summary>
        /// <param name="path">YAML ファイルを表すパス。</param>
        /// <returns><paramref name="path"/> の内容で構築された <see cref="IGanttSource"/>。</returns>
        [NotNull]
        public static IGanttSource Load([NotNull] [PathReference] string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            {
                var deserializer = new Deserializer();
                return deserializer.Deserialize<GanttSourceBuilder>(reader);
            }
        }

        /// <summary>
        /// 指定したテンプレートをもとにガント チャートを構築します。
        /// </summary>
        /// <param name="source">ガント チャートの構築に使用するデータ。</param>
        /// <param name="template">ガント チャートのテンプレート。</param>
        /// <param name="destination">構築したガント チャートの出力先。</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> または <paramref name="template"/> または <paramref name="destination"/> が <c>null</c> です。</exception>
        public static void BuildGanttChart([NotNull] this IGanttSource source, [NotNull] string template, [NotNull] string destination)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            XSSFWorkbook workbook;
            using (var stream = File.OpenRead(template))
            {
                workbook = new XSSFWorkbook(stream);
            }

            var worksheet = workbook.GetSheet("ガントチャート");

            var dateRange = new DateRangeBuilder
            {
                Start = source.Projects.SelectMany(x => x.Tickets).SelectMany(x => new[] { x.EstimatedPeriod, x.ActualPeriod }).Minimum(),
                End = source.Projects.SelectMany(x => x.Tickets).SelectMany(x => new[] { x.EstimatedPeriod, x.ActualPeriod }).Maximum(),
            };

            // metadata
            worksheet.GetRow(0).GetCell(0).SetCellValue(DateTime.Now);

            // calendar
            BuildCalendar(source, worksheet, dateRange);

            // projects
            BuildProjects(source, worksheet, dateRange);

            using (var stream = File.OpenWrite(destination))
            {
                workbook.Write(stream);
            }
        }

        private static void BuildCalendar([NotNull] IGanttSource source, [NotNull] ISheet worksheet, [NotNull] IDateRange dateRange)
        {
            var row0 = worksheet.GetRow(0);
            var row1 = worksheet.GetRow(1);
            var row2 = worksheet.GetRow(2);
            var row3 = worksheet.GetRow(3);
            var row1WeekdayStyle = row1.GetCell(6).CellStyle;
            var row1HolidayStyle = row1.GetCell(7).CellStyle;
            var row1PublicHolidayStyle = row1.GetCell(8).CellStyle;
            var row2WeekdayStyle = row2.GetCell(6).CellStyle;
            var row2HolidayStyle = row2.GetCell(7).CellStyle;
            var row2PublicHolidayStyle = row2.GetCell(8).CellStyle;
            var row3WeekdayStyle = row3.GetCell(6).CellStyle;
            var row3HolidayStyle = row3.GetCell(7).CellStyle;
            var row3PublicHolidayStyle = row3.GetCell(8).CellStyle;
            var cell0T = row0.GetCell(6);
            var cell1T = row1.GetCell(6);
            var cell2T = row2.GetCell(6);
            var cell3T = row3.GetCell(6);
            foreach (var (date, n) in dateRange.AsEnumerable().Select((x, i) => (x, i)))
            {
                var cell0 = cell0T;
                var cell1 = cell1T;
                var cell2 = cell2T;
                var cell3 = cell3T;
                if (n != 0)
                {
                    cell0 = cell0.CopyCellTo(6 + n);
                    cell1 = cell1.CopyCellTo(6 + n);
                    cell2 = cell2.CopyCellTo(6 + n);
                    cell3 = cell3.CopyCellTo(6 + n);
                }

                if (date.Day == 1)
                {
                    cell0.SetCellValue(date);
                }
                else
                {
                    cell0.SetCellValue((string)null);
                }
                if (source.Configuration.Holidays.Contains(date))
                {
                    cell1.SetCellValue("祝");
                    cell1.CellStyle = row1PublicHolidayStyle;
                    cell2.CellStyle = row2PublicHolidayStyle;
                    cell3.CellStyle = row3PublicHolidayStyle;
                }
                else if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    cell1.SetCellValue("休");
                    cell1.CellStyle = row1HolidayStyle;
                    cell2.CellStyle = row2HolidayStyle;
                    cell3.CellStyle = row3HolidayStyle;
                }
                else
                {
                    cell1.SetCellValue((string)null);
                    cell1.CellStyle = row1WeekdayStyle;
                    cell2.CellStyle = row2WeekdayStyle;
                    cell3.CellStyle = row3WeekdayStyle;
                }
                cell2.SetCellValue(date);
                cell3.SetCellValue(date);
                worksheet.SetColumnWidth(6 + n, worksheet.GetColumnWidth(6));
            }
            foreach (var (date, n) in dateRange.AsEnumerable().Select((x, i) => (x, i)))
            {
                if (date.Day == 1)
                {
                    worksheet.AddMergedRegion(new CellRangeAddress(0, 0, 6 + n, 6 + n + 2));
                }
            }
        }

        private static void BuildProjects([NotNull] IGanttSource source, [NotNull] ISheet worksheet, [NotNull] IDateRange dateRange)
        {
            var drawing = (XSSFDrawing)worksheet.CreateDrawingPatriarch();
            var row4 = worksheet.GetRow(4);
            var weekdayStyle = row4.GetCell(6).CellStyle;
            var holidayStyle = row4.GetCell(7).CellStyle;
            var publicHolidayStyle = row4.GetCell(8).CellStyle;
            weekdayStyle.BorderTop = BorderStyle.None;
            holidayStyle.BorderTop = BorderStyle.None;
            publicHolidayStyle.BorderTop = BorderStyle.None;
            var ticketsWithNestAndRowIndex = source
                .Projects
                .Select(x => new TicketBuilder { Name = x.Name, Children = (List<TicketBuilder>)x.Tickets })
                .SelectMany(x => x.AsEnumerable())
                .Select((x, i) => (x.ticket, x.nest, 4 + i));
            foreach (var (ticket, indent, rowIndex) in ticketsWithNestAndRowIndex)
            {
                var row = row4;
                if (rowIndex != 4)
                {
                    row = row.CopyRowTo(rowIndex);
                }
                row.GetCell(0).SetCellValue($"{new string('　', indent * 2)}{ticket.Name}");
                row.GetCell(1).SetCellValueOrDefault(ticket.EstimatedPeriod.Start);
                row.GetCell(2).SetCellValueOrDefault(ticket.EstimatedPeriod.End);
                row.GetCell(3).SetCellValueOrDefault(ticket.ActualPeriod.Start);
                row.GetCell(4).SetCellValueOrDefault(ticket.ActualPeriod.End);
                if (ticket.Status == TicketStatus.Unknown)
                {
                    row.GetCell(5).SetCellValue((string)null);
                }
                else
                {
                    row.GetCell(5).SetCellValue(EnumDisplayNameAttribute.GetDisplayName(ticket.Status));
                }
                foreach (var (date, n) in dateRange.AsEnumerable().Select((x, i) => (x, i)))
                {
                    var cell = row.GetCell(6);
                    if (n != 0)
                    {
                        cell = cell.CopyCellTo(6 + n);
                    }
                    if (source.Configuration.Holidays.Contains(date))
                    {
                        cell.CellStyle = publicHolidayStyle;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        cell.CellStyle = holidayStyle;
                    }
                    else
                    {
                        cell.CellStyle = weekdayStyle;
                    }
                }
                if (ticket.EstimatedPeriod.Start != default && ticket.EstimatedPeriod.End != default)
                {
                    var anchor = new XSSFClientAnchor(
                        dx1: XSSFShape.EMU_PER_POINT * 2,
                        dy1: XSSFShape.EMU_PER_POINT * 7,
                        dx2: XSSFShape.EMU_PER_POINT * -2,
                        dy2: XSSFShape.EMU_PER_POINT * 7,
                        col1: 6 + (ticket.EstimatedPeriod.Start - dateRange.Start).Days,
                        row1: rowIndex,
                        col2: 6 + (ticket.EstimatedPeriod.End - dateRange.Start).Days + 1,
                        row2: rowIndex);
                    var connector = drawing.CreateConnector(anchor);
                    connector.LineWidth = 6;
                    connector.SetLineStyleColor(source.Configuration.EstimatedLineColor);
                }
                if (ticket.ActualPeriod.Start != default && ticket.ActualPeriod.End != default)
                {
                    var anchor = new XSSFClientAnchor(
                        dx1: XSSFShape.EMU_PER_POINT * 2,
                        dy1: XSSFShape.EMU_PER_POINT * 13,
                        dx2: XSSFShape.EMU_PER_POINT * -2,
                        dy2: XSSFShape.EMU_PER_POINT * 13,
                        col1: 6 + (ticket.ActualPeriod.Start - dateRange.Start).Days,
                        row1: rowIndex,
                        col2: 6 + (ticket.ActualPeriod.End - dateRange.Start).Days + 1,
                        row2: rowIndex);
                    var connector = drawing.CreateConnector(anchor);
                    connector.LineWidth = 6;
                    connector.SetLineStyleColor(source.Configuration.ActualLineColor);
                }
                else if (ticket.ActualPeriod.Start != default && ticket.ActualPeriod.End == default && ticket.ActualPeriod.Start <= DateTime.Today)
                {
                    var anchor = new XSSFClientAnchor(
                        dx1: XSSFShape.EMU_PER_POINT * 2,
                        dy1: XSSFShape.EMU_PER_POINT * 13,
                        dx2: XSSFShape.EMU_PER_POINT * -2,
                        dy2: XSSFShape.EMU_PER_POINT * 13,
                        col1: 6 + (ticket.ActualPeriod.Start - dateRange.Start).Days,
                        row1: rowIndex,
                        col2: 6 + (DateTime.Today - dateRange.Start).Days + 1,
                        row2: rowIndex);
                    var connector = drawing.CreateConnector(anchor);
                    connector.LineWidth = 6;
                    connector.SetLineStyleColor(source.Configuration.ActualLineColor);
                }
            }
        }
    }
}
