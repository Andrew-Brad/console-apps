using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FullCliApp.Tests;

public static class TestServiceCollectionExtensions
{
    // also consider using IHost like https://github.com/natemcmaster/CommandLineUtils/blob/main/test/Hosting.CommandLine.Tests/CustomValueParserTests.cs
    public static IServiceProvider ConfigureTestServices()
    {
        var services = ServiceCollectionExtensions.ConfigureServices(Program.BuildConfiguration());
        services.RemoveAll<IConsole>();
        services.AddSingleton<IConsole>(GetTestConsole());
        return services.BuildServiceProvider();
    }

    public static IConsole GetTestConsole() => new TestConsole();
}
