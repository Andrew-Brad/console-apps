using McMaster.Extensions.CommandLineUtils;

namespace FullCli;

public class Program
{
    public static int Main(string[] args)
    {
        // https://natemcmaster.github.io/CommandLineUtils/docs/intro.html
        var app = new CommandLineApplication();

        app.HelpOption();
        var subject = app.Option("-s|--subject <SUBJECT>", "The subject", CommandOptionType.SingleValue);
        subject.DefaultValue = "world";

        app.OnExecute(() =>
        {
            Console.WriteLine($"Hello {subject.Value()}!");
            return 0;
        });

        return app.Execute(args);
    }
}
