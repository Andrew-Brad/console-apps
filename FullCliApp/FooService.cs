using McMaster.Extensions.CommandLineUtils;

namespace FullCliApp;

public interface IFooService { void Invoke(); }

public class FooService(IConsole console) : IFooService
{
    public void Invoke() => console.WriteLine("Hello from you dependency injection service!");
}
