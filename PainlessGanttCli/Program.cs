using Microsoft.Extensions.CommandLineUtils;
using PainlessGantt;
using PainlessGantt.Construction;

namespace PainlessGanttCli
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var application = new CommandLineApplication();
            var project = application.Option("-p | --project", "プロジェクト定義 (YAML)", CommandOptionType.SingleValue);
            var template = application.Option("-t | --template", "テンプレート (Excel)", CommandOptionType.SingleValue);
            var output = application.Option("-o | --output", "出力先 (Excel)", CommandOptionType.SingleValue);
            application.OnExecute(() =>
            {
                var source = (GanttSourceBuilder)GanttSource.Load(project.Value());
                InputCompletion.CompleteBubble(source);
                InputCompletion.CompleteTunnel(source);
                source.BuildGanttChart(template.Value(), output.Value());
                return 0;
            });
            application.Execute(args);
        }
    }
}
