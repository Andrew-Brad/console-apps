using Spectre.Console;

namespace WidgetFactory
{
    /// <summary>
    /// Widget Factory is an exercise in discovering a problem which is already solved.
    /// How many bugs can you find, and what antipattern are you seeing?
    /// What is the tool or mechanism that already exists to solve this problem?
    /// </summary>
    public class Program
    {
        public static async Task Main()
        {
            // Initialize command actions
            var commands = new Dictionary<string, Action>
            {
                { "help", ShowHelp },
                { "start", () => AnsiConsole.MarkupLine("[green]Starting the game...[/]") },
                {
                    "quit", () =>
                    {
                        AnsiConsole.MarkupLine("[red]Exiting game. Goodbye![/]");
                        Environment.Exit(0);
                    }
                }
            };

            // Render the command guide
            DisplayCommandGuide(commands);

            // Ask user for a command
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>("[yellow]Enter a command[/]:")
                    .Validate(cmd =>
                        commands.ContainsKey(cmd)
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Invalid command[/]")));

            // Execute the corresponding action
            commands[input]();

            var table = new Table().Centered().Title("[yellow]Widget Production Table[/]");

            await AnsiConsole.Live(table)
                .AutoClear(false) // Do not remove when done
                .Overflow(VerticalOverflow.Ellipsis) // Show ellipsis when overflowing
                .Cropping(VerticalOverflowCropping.Top) // Crop overflow at top
                .StartAsync(async ctx =>
                {
                    int foosProduced = 0;
                    int barsProduced = 0;
                    table.AddColumn("Widgets");
                    table.AddColumn("Rate");
                    table.AddColumn("Number Produced");
                    table.AddRow("Foo", "1 per second", foosProduced.ToString());
                    table.AddRow("Bar", "1 per 5 seconds", barsProduced.ToString());
                    //table.AddRow("Baz", "3/min");
                    ctx.Refresh();

                    int lastFooFrames = 0;
                    int lastBarFrames = 0;
                    while (true)
                    {
                        int numFrames = await RenderLoop(); // coupling to internal implementation :(

                        int newFoosProduced = numFrames - lastFooFrames;
                        if (((numFrames - lastFooFrames) / 1) >= 1)
                        {
                            foosProduced += newFoosProduced;
                            lastFooFrames = numFrames;
                            table.Rows.Update(0, 2, new Text(foosProduced.ToString()));
                            ctx.Refresh();
                        }

                        int newBarsProduced = numFrames - lastBarFrames;
                        if (((numFrames - lastBarFrames) / 5) >= 1)
                        {
                            barsProduced++;
                            lastBarFrames = numFrames;
                            table.Rows.Update(1, 2, new Text(barsProduced.ToString()));
                            ctx.Refresh();
                        }
                    }
                });
        }

        private static void DisplayCommandGuide(Dictionary<string, Action> commands)
        {
            // Clear and render the guide for available commands
            Console.Clear();
            AnsiConsole.MarkupLine("[blue bold]Command Guide:[/]");
            var table = new Table();
            table.AddColumn("[green]Command[/]");
            table.AddColumn("[green]Description[/]");

            // Define descriptions for each command
            var descriptions = new Dictionary<string, string>
            {
                { "help", "Show Helper Text" }, { "start", "Start the game" }, { "quit", "Exit the game" }
            };

            foreach (var command in commands.Keys)
            {
                table.AddRow($"[yellow]{command}[/]", descriptions[command]);
            }

            AnsiConsole.Write(table);
        }

        private static void ShowHelp()
        {
            AnsiConsole.MarkupLine("[green]Widget factory only exists as an exercise in bug squashing and pattern recognition.[/]");
            AnsiConsole.MarkupLine("[green]Your widgets will continue to produce over time...we think.[/]");
        }

        private static int _totalSeconds = 0;
        public async static Task<int> RenderLoop()
        {
            await Task.Delay(1000);
            _totalSeconds++;
            return _totalSeconds;
        }
    }
}
