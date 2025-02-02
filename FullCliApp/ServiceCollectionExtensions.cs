using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FullCliApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(IConfiguration config)
    {
        return new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton<IFooService, FooService>()
            .AddSingleton<IConsole>(PhysicalConsole.Singleton);
        // Register CommandLine Application and dependencies
        // services.AddSingleton<AppCommand>();
        // services.AddTransient<CommandLineApplication<AppCommand>>();
    }
}
