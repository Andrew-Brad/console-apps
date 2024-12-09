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
        // dotnet McMasterCliSample.dll --color Yellow Andrew Bob Sally
        var subjectsOption = app.Argument("Subjects", "Subjects to address", multipleValues: true).IsRequired();
        var colorOption = app.Option("-c|--color <COLOR>", "Console color to print", CommandOptionType.SingleValue);
        colorOption.DefaultValue = Console.ForegroundColor.ToString();

        app.OnExecute(() =>
        {
            if (Enum.TryParse(colorOption.Value(), out ConsoleColor parsedColor))
            {
                Console.ForegroundColor = parsedColor;
            }

            foreach (var subject in subjectsOption.Values)
            {
                Console.WriteLine($"Hello, {subject}!");
            }
            return 0;
        });

        return app.Execute(args);
    }
}
