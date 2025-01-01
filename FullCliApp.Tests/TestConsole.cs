using McMaster.Extensions.CommandLineUtils;

namespace FullCliApp.Tests;

public sealed class TestConsole : IConsole
{
    public TextWriter Out { get; } = new StringWriter();
    public TextWriter Error { get; } = new StringWriter();
    public TextReader In { get; } = new StringReader(string.Empty);
    public bool IsInputRedirected => false;
    public bool IsOutputRedirected => false;
    public bool IsErrorRedirected => false;
    public ConsoleColor ForegroundColor { get; set; }
    public ConsoleColor BackgroundColor { get; set; }
    public event ConsoleCancelEventHandler? CancelKeyPress;
    public void ResetColor() { }
    public void WriteLine(string value) => Out.WriteLine(value);
    public void Write(string value) => Out.Write(value);

    private void OnCancelKeyPress(ConsoleCancelEventArgs e)
    {
        CancelKeyPress?.Invoke(this, e);
    }
}
