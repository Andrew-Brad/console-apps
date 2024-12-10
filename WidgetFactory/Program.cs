using System;
using System.Collections.Generic;
using Spectre.Console;

namespace WidgetFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize command actions
            var commands = new Dictionary<string, Action>
            {
                { "help", ShowHelp },
                { "start", () => AnsiConsole.MarkupLine("[green]Starting the game...[/]") },
                { "quit", () => { AnsiConsole.MarkupLine("[red]Exiting game. Goodbye![/]"); Environment.Exit(0); } }
            };

            while (true)
            {
                // Render the command guide
                DisplayCommandGuide(commands);

                // Ask user for a command
                var input = AnsiConsole.Prompt(
                    new TextPrompt<string>("[yellow]Enter a command[/]:")
                        .Validate(cmd => commands.ContainsKey(cmd) ? ValidationResult.Success() : ValidationResult.Error("[red]Invalid command[/]")));

                // Execute the corresponding action
                commands[input]();
            }
        }

        static void DisplayCommandGuide(Dictionary<string, Action> commands)
        {
            // Clear and render the guide for available commands
            Console.Clear();
            //AnsiConsole.MarkupLine("[blue bold]Command Guide:[/]");
            var table = new Table();
            table.AddColumn("[green]Command[/]");
            table.AddColumn("[green]Description[/]");

            // Define descriptions for each command
            var descriptions = new Dictionary<string, string>
            {
                { "help", "Show this help menu" },
                { "start", "Start the game" },
                { "quit", "Exit the game" }
            };

            foreach (var command in commands.Keys)
            {
                table.AddRow($"[yellow]{command}[/]", descriptions[command]);
            }

            AnsiConsole.Render(table);
        }

        static void ShowHelp()
        {
            AnsiConsole.MarkupLine("[blue]This is a simple REPL-style game interface.[/]");
            AnsiConsole.MarkupLine("[green]Type any of the available commands to perform an action.[/]");
        }
    }
}

