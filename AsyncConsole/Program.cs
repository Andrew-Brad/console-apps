namespace AsyncConsole
{
    public class Program
    {
        public static Task Main() => MainAsync();

        private static async Task MainAsync()
        {
            try
            {
                Console.WriteLine("MainAsync()");

                Console.WriteLine("Delaying...");
                await Task.Delay(500);//.ConfigureAwait(false);
                Console.WriteLine("Delay Completed");

                Environment.Exit(0); // https://en.wikipedia.org/wiki/Exit_status
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}
