using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.Configuration;

namespace FullCliApp;

public class Program(CommandLineContext context, IEnumerable<CommandArgument> commandArguments, IFooService fooService)
{
    private readonly CommandLineContext _context = context;
    private const string FilePathArgument = "filePath";

    public static int Main(string[] args)
    {
        var config = BuildConfiguration();
        var services = ServiceCollectionExtensions.ConfigureServices(config);

        var app = new CommandLineApplication<Program>();
        var filePathArg = app.Argument(FilePathArgument, "The path to the file to use");
        filePathArg.DefaultValue = Path.GetFullPath("../../../inputFile.txt");
        app.Conventions
            .UseDefaultConventions()
            .UseDefaultHelpOption()
            .UseConstructorInjection(services);

        return app.Execute(args);
    }

    public int OnExecute()
    {
        var filePath = commandArguments.Single(x => x.Name == FilePathArgument).Value!;
        Console.WriteLine(fooService.DoTheThing(filePath));
        return 0;
    }

    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables(); // Env variables will be prioritized

        return builder.Build();
    }
}
