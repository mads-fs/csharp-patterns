#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE0074 // Use compound assignment
namespace Singleton
{
    /// <summary>
    /// This implementation would not be thread-safe and could cause issues in
    /// multithreaded environments. However quite a few games can get away with
    /// not having to care about thread-safety for their singletons.
    /// To see a threadsafe version check out the <see cref="GameManagerThreadSafe"/> 
    /// class instead. Notice that the class is sealed, meaning no other class can
    /// extend or derive from this class because Singletons are supposed to only
    /// have one of itself at runtime at any given time. So having extendable
    /// Singletons would not make much sense. There are certain cases where you'd
    /// like to extend a Singleton, usually an abstract parent class, but that's beyond
    /// the scope of this example.
    /// </summary>
    public sealed class GameManager
    {
        public Guid Id { get { return id; } }
        private readonly Guid id;
        public string State { get { return state; } set { state = value; } }
        private string state = "Ready";
        public int SessionId { get { return sessionId; } }
        private int sessionId;
        // This property simulates the tick time of many Game Engine Update Loops
        public float Tick { get { return (float)rng.NextDouble() * 0.5f; } }

        private readonly Random rng = new();
        // We keep the constructor private to ensure that no
        // outside class can create it using the 'new' keyword.
        private GameManager()
        {
            id = Guid.NewGuid();
            sessionId = rng.Next();
        }

        // This is the actual instance of the GameManager we will
        // be operating on.
        private static GameManager instance;

        // This is how code will access the instance.
        // There are multiple ways to do this. This is
        // just one of many ways.
        public static GameManager GetInstance()
        {
            if (instance == null) instance = new GameManager();
            return instance;
        }

        // Lastly we have code that can be accessed on the instance like any other class.
        public void UpdateSessionId() => sessionId = new Random().Next();
    }
}