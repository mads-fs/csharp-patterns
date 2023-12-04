namespace Factory
{
    /// <summary>
    /// The Factory pattern is most useful when you have a challenge that can be solved by 
    /// multiple similar implementations depending on specific circumstances. Great examples
    /// in games is when you need to support cross-platform saving or input systems but all of
    /// the platforms are using the same code-base to cater to all of them. In this example
    /// you will see a simple Save/Load system implementation where the implementation for
    /// doing either is switched out based on platform, however the code that relies on the
    /// Save/Load system does not need to know what implementation is used to carry out the
    /// operation. In a real world situation, you'd only need to create one concrete type of 
    /// <see cref="IStorage"/> seeing as hardware platforms don't change during gameplay.
    /// However there are many other places in code where being able to construct different
    /// concrete classes of the same Interface can be very useful.
    /// </summary>
    internal class Program
    {
        // The following instances exists purely for the purpose of convenience in this example.
        public static GameConsole? ConsoleInstance { get { return gameConsole; } }
        private static GameConsole? gameConsole;
        public static MobileDevice? MobileInstance { get { return mobileDevice; } }
        private static MobileDevice? mobileDevice;

        static void Main()
        {
            // Setup for the demo
            gameConsole = new GameConsole();
            mobileDevice = new MobileDevice();

            // Same codebase for each platform with different outcomes based
            // on the platform being passed to the Factory.
            Platform[] platforms = (Platform[])Enum.GetValues(typeof(Platform));
            foreach (Platform platform in platforms)
            {
                IStorage? storage = StorageFactory.Create(platform);
                if (storage != null)
                {
                    storage.Save();
                    storage.Load();
                }
                Console.WriteLine("====================");
            }
        }
    }
}