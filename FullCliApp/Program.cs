using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FullCliApp;

public class Program(IConsole console, CommandLineContext context, IEnumerable<CommandArgument> commandArguments, IFooService fooService)
{
    public static IServiceProvider? ServiceProvider { get; set; }
    private readonly CommandLineContext _context = context;
    private readonly IConsole _console = console;
    private const string FilePathArgument = "filePath";

    public static int Main(string[] args)
    {
        var config = BuildConfiguration();
        var services = ServiceProvider ?? ServiceCollectionExtensions
            .ConfigureServices(config)
            .BuildServiceProvider();

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
        var serviceOutput = fooService.DoTheThing(filePath);
        _console.WriteLine(serviceOutput);
        return 0;
    }

    public static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables(); // Env variables will be prioritized

        return builder.Build();
    }
}
