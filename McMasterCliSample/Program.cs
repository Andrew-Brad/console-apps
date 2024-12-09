using McMaster.Extensions.CommandLineUtils;

namespace McMasterCliSample;

public class Program
{
    public static int Main(string[] args)
    {
        // https://natemcmaster.github.io/CommandLineUtils/docs/intro.html
        var app = new CommandLineApplication();

        // When a command executes, the raw string[] args value can be separated into two different categories: options and arguments.
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
