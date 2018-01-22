using Microsoft.Extensions.CommandLineUtils;
using PainlessGantt;

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
                var source = GanttSource.Load(project.Value());
                source = InputCompletion.Complete(source);
                source.BuildGanttChart(template.Value(), output.Value());
                return 0;
            });
            application.Execute(args);
        }
    }
}
