using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FullCliApp.Tests;

public class FullCliAppTests
{
    [Fact]
    public void DoTheThing_ShouldOutputPriorityMessage_WhenPriorityMessageIsSet()
    {
        // Arrange
        var testConsole = GetTestConsole();
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["PriorityMessage"]).Returns("Custom Priority Message");
        var service = new FooService(testConsole, mockConfig.Object);
        string tempFile = Path.GetTempFileName();

        // Act
        service.DoTheThing(tempFile);

        // Assert
        string output = testConsole.Out.ToString()!;
        Assert.Contains("Custom Priority Message", output);
        File.Delete(tempFile);
    }

    [Fact]
    public void DoTheThing_ShouldFallbackToDefaultMessage_WhenPriorityMessageIsNotSet()
    {
        // Arrange
        var testConsole = GetTestConsole();
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["PriorityMessage"]).Returns((string?)null);
        var service = new FooService(testConsole, mockConfig.Object);
        string tempFile = Path.GetTempFileName();

        // Act
        service.DoTheThing(tempFile);

        // Assert
        string output = testConsole.Out.ToString()!;
        Assert.Contains("Hello from your injected service!", output);
        File.Delete(tempFile);
    }

    [Fact]
    public void DoTheThing_ShouldIndicateFileExists_WhenFileExists()
    {
        // Arrange
        var testConsole = GetTestConsole();
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["PriorityMessage"]).Returns((string?)null);
        var service = new FooService(testConsole, mockConfig.Object);
        string tempFile = Path.GetTempFileName();

        // Act
        service.DoTheThing(tempFile);

        // Assert
        string output = testConsole.Out.ToString()!;
        Assert.Contains($"The file path you provided was resolved and exists: {tempFile}", output);

        File.Delete(tempFile);
    }

    [Fact]
    public void DoTheThing_ShouldIndicateFileDoesNotExist_WhenFileDoesNotExist()
    {
        // Arrange
        var testConsole = GetTestConsole();
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["PriorityMessage"]).Returns((string?)null);

        var service = new FooService(testConsole, mockConfig.Object);
        string nonExistentFile = Path.Combine(Path.GetTempPath(), "nonexistentfile.txt");

        // Act
        var result = service.DoTheThing(nonExistentFile);

        // Assert
        string output = testConsole.Out.ToString()!;
        Assert.Contains($"The file path you provided does not exist: {nonExistentFile}", output);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void DoTheThing_ShouldReturnFileContent_WhenFileExists()
    {
        // Arrange
        var testConsole = GetTestConsole();
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["PriorityMessage"]).Returns((string?)null);

        var service = new FooService(testConsole, mockConfig.Object);
        string tempFile = Path.GetTempFileName();
        string fileContent = "Sample File Content";
        File.WriteAllText(tempFile, fileContent);

        // Act
        var result = service.DoTheThing(tempFile);

        // Assert
        string output = testConsole.Out.ToString()!;
        Assert.Contains($"The file path you provided was resolved and exists: {tempFile}", output);
        Assert.Equal(fileContent, result);

        File.Delete(tempFile);
    }

    private IConsole GetTestConsole() => new MockConsole();
}
