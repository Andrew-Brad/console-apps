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
        var subjectOption = app.Option("-s|--subject <SUBJECT>", "The subject", CommandOptionType.SingleValue);
        subjectOption.DefaultValue = "world";
        var colorOption = app.Option("-c|--color <COLOR>", "Console color to print", CommandOptionType.SingleValue);
        colorOption.DefaultValue = Console.ForegroundColor.ToString();

        app.OnExecute(() =>
        {
            if (Enum.TryParse(colorOption.Value(), out ConsoleColor parsedColor))
            {
                Console.ForegroundColor = parsedColor;
            }
            Console.WriteLine($"Hello {subjectOption.Value()}!");
            return 0;
        });

        return app.Execute(args);
    }
}
