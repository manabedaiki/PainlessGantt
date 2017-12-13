using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using PainlessGantt.Construction;
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
                }
                else if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    cell1.SetCellValue("休");
                }
                else
                {
                    cell1.SetCellValue((string)null);
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
            var row4 = worksheet.GetRow(4);
            var noneStyle = row4.GetCell(6).CellStyle;
            var estimatedStyle = row4.GetCell(7).CellStyle;
            var overlappedStyle = row4.GetCell(8).CellStyle;
            var actualStyle = row4.GetCell(9).CellStyle;
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
                if (ticket.EstimatedPeriod.Start != default)
                {
                    row.GetCell(1).SetCellValue(ticket.EstimatedPeriod.Start);
                }
                else
                {
                    row.GetCell(1).SetCellValue((string)null);
                }
                if (ticket.EstimatedPeriod.End != default)
                {
                    row.GetCell(2).SetCellValue(ticket.EstimatedPeriod.End);
                }
                else
                {
                    row.GetCell(2).SetCellValue((string)null);
                }
                if (ticket.ActualPeriod.Start != default)
                {
                    row.GetCell(3).SetCellValue(ticket.ActualPeriod.Start);
                }
                else
                {
                    row.GetCell(3).SetCellValue((string)null);
                }
                if (ticket.ActualPeriod.End != default)
                {
                    row.GetCell(4).SetCellValue(ticket.ActualPeriod.End);
                }
                else
                {
                    row.GetCell(4).SetCellValue((string)null);
                }
                var status = ticket.InferredStatus();
                if (status == TicketStatus.Unknown)
                {
                    row.GetCell(5).SetCellValue((string)null);
                }
                else
                {
                    row.GetCell(5).SetCellValue(EnumDisplayNameAttribute.GetDisplayName(status));
                }
                foreach (var (date, n) in dateRange.AsEnumerable().Select((x, i) => (x, i)))
                {
                    var cell = row.GetCell(6);
                    if (n != 0)
                    {
                        cell = cell.CopyCellTo(6 + n);
                    }
                    if (ticket.EstimatedPeriod.In(date) && ticket.ActualPeriod.In(date))
                    {
                        cell.CellStyle = overlappedStyle;
                    }
                    else if (ticket.EstimatedPeriod.In(date))
                    {
                        cell.CellStyle = estimatedStyle;
                    }
                    else if (ticket.ActualPeriod.In(date))
                    {
                        cell.CellStyle = actualStyle;
                    }
                    else
                    {
                        cell.CellStyle = noneStyle;
                    }
                }
            }
        }
    }
}
