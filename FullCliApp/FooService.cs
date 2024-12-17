using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;

namespace FullCliApp;

public interface IFooService { string DoTheThing(string filePath); }

public class FooService(IConsole console, IConfiguration configuration) : IFooService
{
    public string DoTheThing(string filePath)
    {
        string? priorityMessage = configuration["PriorityMessage"];
        console.WriteLine(string.IsNullOrWhiteSpace(priorityMessage)
            ? "Hello from your injected service!"
            : priorityMessage);


        bool fileExists = File.Exists(filePath);
        Console.WriteLine(fileExists
            ? $"The file path you provided was resolved and exists: {filePath}"
            : $"The file path you provided does not exist: {filePath}");

        if (!fileExists) return string.Empty;

        // parse the file:
        string input = File.ReadAllText(filePath);
        return input;
    }
}
