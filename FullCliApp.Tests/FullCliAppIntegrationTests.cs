using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace FullCliApp.Tests;

public class FullCliAppIntegrationTests : IDisposable
{
    private readonly string _testFilePath;
    private const int SuccessExitCode = 0;
    private const string TestFileContent = "test content";

    public FullCliAppIntegrationTests()
    {
        // Set up test file
        _testFilePath = Path.GetTempFileName();
        File.WriteAllText(_testFilePath, TestFileContent);

        // Set up test appsettings
        var testSettings = @"{
            ""TestSetting"": ""TestValue""
        }";
        File.WriteAllText("appsettings.json", testSettings);
    }

    [Fact]
    public async Task Execute_WithValidFilePath_ReturnsSuccessCodeWithConsoleOutput()
    {
        // Arrange
        var args = new[] { _testFilePath };
        Program.ServiceProvider = TestServiceCollectionExtensions.ConfigureTestServices();

        // Act
        int exitCode = await Task.Run(() => Program.Main(args));

        // Assert
        Assert.Equal(SuccessExitCode, exitCode);
        AssertConsoleOutput(TestFileContent);
    }

    /// <summary>
    /// Helper method to check that the console just printed some expected output.
    /// </summary>
    /// <param name="expectedOutput"></param>
    /// <exception cref="Exception"></exception>
    private void AssertConsoleOutput(string expectedOutput)
    {
        var console = Program.ServiceProvider!.GetRequiredService<IConsole>();
        string? consoleOutput = console.Out.ToString();
        if (string.IsNullOrEmpty(consoleOutput) && !string.IsNullOrEmpty(expectedOutput))
        {
            throw new Exception($"Expected non-empty output, but console returned nothing.");
        }
        Assert.Contains(expectedOutput + Environment.NewLine, consoleOutput);
    }

    public void Dispose()
    {
        // Clean up test files
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }

        if (File.Exists("appsettings.json"))
        {
            File.Delete("appsettings.json");
        }
    }
}
