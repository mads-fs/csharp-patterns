namespace State
{
    /// <summary>
    /// The State Pattern is a useful behavioural pattern for when you need code to act differently
    /// depending on the state of another object (often referred to as the Context object) but without
    /// knowing the specifics of the state object or how to work it. This pattern provides a common way
    /// to interface with the Context object such that the overall logic of the caller stays the same
    /// while each individual State implementation knows how to handle the calls made on it's implementation.
    /// This is especially useful for UI, AI and Animation but the State pattern is not limited to those.
    /// </summary>
    public class Program
    {
        public static World? World { get { return world; } }
        private static World? world;

        private readonly static double tickTime = 1.0;
        private static CancellationToken cancelToken = CancellationToken.None;

        public static async Task<int> Main()
        {
            Thread buttonThread = new(() =>
            {
                if (Console.ReadLine() != null) cancelToken = new(true);
            });
            buttonThread.Start();
            await Task.Run(Simulate);
            return 0;
        }

        private static async Task Simulate()
        {
            world ??= new();
            using (PeriodicTimer timer = new(TimeSpan.FromSeconds(tickTime)))
            {
                while (await timer.WaitForNextTickAsync())
                {
                    world?.Update();
                    Console.WriteLine("The Simulation will end once a button is pressed.");
                    if (cancelToken.IsCancellationRequested) break;
                }
            }
            await Task.CompletedTask;
        }
    }
}