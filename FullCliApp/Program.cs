using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FullCliApp;

public class Program
{
    private readonly IConsole _console;
    private readonly CommandLineContext _context;
    private readonly IEnumerable<CommandArgument> _commandArguments;
    private readonly IFooService _fooService;
    private const string FilePathArgument = "filePath";

    public Program(IConsole console, CommandLineContext context, IEnumerable<CommandArgument> commandArguments,
        IFooService fooService)
    {
        _console = console;
        _context = context;
        _commandArguments = commandArguments;
        _fooService = fooService;
    }

    public static int Main(string[] args)
    {
        var config = BuildConfiguration();
        var services = ConfigureServices(config);

        var app = new CommandLineApplication<Program>();
        var filePathArg = app.Argument(FilePathArgument, "The path to the file to use");
        filePathArg.DefaultValue = Path.GetFullPath("../../../inputFile.txt");
        app.Conventions
            .UseDefaultConventions()
            .UseDefaultHelpOption()
            .UseConstructorInjection(services);

        return app.Execute(args);
    }

    private int OnExecute()
    {
        var filePath = _commandArguments.Single(x => x.Name == FilePathArgument).Value!;
        Console.WriteLine(_fooService.DoTheThing(filePath));
        return 0;
    }

    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //.AddEnvironmentVariables() // Env variables will be prioritized

        return builder.Build();
    }

    private static IServiceProvider ConfigureServices(IConfiguration config)
    {
        // Register IConfiguration
        return new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton<IFooService, FooService>()
            .AddSingleton<IConsole>(PhysicalConsole.Singleton)
            .BuildServiceProvider();
        // Register CommandLine Application and dependencies
        // services.AddSingleton<AppCommand>();
        // services.AddTransient<CommandLineApplication<AppCommand>>();
    }
}
