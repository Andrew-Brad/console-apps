using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FullCliApp;

[Command(Name = "di", Description = "Dependency Injection sample project")]
[HelpOption]
public class Program
{
    private readonly IConsole _console;
    private readonly CommandLineContext _context;
    private readonly IFooService _fooService;

    private static int Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddSingleton<IFooService, FooService>()
            .AddSingleton<IConsole>(PhysicalConsole.Singleton)
            .BuildServiceProvider();

        var app = new CommandLineApplication<Program>();
        app.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(services);
        return app.Execute(args);
    }

    public Program(IConsole console, CommandLineContext context, IFooService fooService)
    {
        _console = console;
        _context = context;
        _fooService = fooService;
    }

    private int OnExecute()
    {
        var dir = _context.WorkingDirectory;
        _console.WriteLine($"Hello from your first application in {dir}");
        _fooService.Invoke();
        return 0;
    }
}
