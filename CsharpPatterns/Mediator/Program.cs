using System.Diagnostics;

namespace Mediator
{
    /// <summary>
    /// The Mediator Pattern is useful when there are a bunch of components or objects
    /// that you wish to communicate with each other but without those components and objects
    /// needing to know how to do that. Using a Mediator class, like the <see cref="GameEvents"/>
    /// class in this example (check the Events.cs file), showcases one implementation where 
    /// a lot of systems of your game might want to know about various state changes 
    /// regardless of where those state changes come from. Instead of every one of those 
    /// systems having to know each other, all they need to know is the Mediator class 
    /// and subscribing to relevant events.
    /// </summary>
    internal class Program
    {
        private static bool stopGame = false;
        public static void Main()
        {
            SystemEvents.GameEnd += () => stopGame = true;
            Task sim = Task.Run(() => Start());
            while (!sim.IsCompleted) ;
            // Pause Console before exiting
            Console.ReadLine();
        }

        private static async Task Start()
        {
            Console.WriteLine("Setting up Simulation");
            Simulation simulation = new(0);
            // Simulating an Update Loop from a typical Games Update Loop
            float time = 0.5f;
            Console.WriteLine($"Running Simulation at '{time}' second(s) per frame");
            using (PeriodicTimer timer = new(TimeSpan.FromSeconds(time)))
            {
                while (await timer.WaitForNextTickAsync())
                {
                    if (stopGame) break;
                    Console.WriteLine("----- TICK -----");
                    simulation.Tick();
                }
            }
            await Task.CompletedTask;
        }
    }
}