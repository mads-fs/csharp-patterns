namespace Singleton
{
    /// <summary>
    /// This implementation is thread-safe and works in multithreaded environments.
    /// However quite a few games can get away with not having to care about
    /// thread-safety for their singletons. To see a simpler version check out
    /// the <see cref="GameManager"/> class instead. Notice that the class is sealed,
    /// meaning no other class can extend or derive from this class because Singletons
    /// are supposed to only have one of itself at runtime at any given time.
    /// So having extendable Singletons would not make much sense. There are certain
    /// cases where you'd like to extend a Singleton, usually an abstract parent class,
    /// but that's beyond the scope of this example.
    /// </summary>
    public sealed class GameManagerThreadSafe
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
        private static readonly object _lock = new();
        // We keep the constructor private to ensure that no
        // outside class can create it using the 'new' keyword.
        private GameManagerThreadSafe()
        {
            id = Guid.NewGuid();
            sessionId = rng.Next();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // This is the actual instance of the GameManager we will
        // be operating on.
        private static GameManagerThreadSafe instance;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // This is how code will access the instance.
        // There are multiple ways to do this. This is
        // just one of many ways.
        public static GameManagerThreadSafe GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null) instance = new GameManagerThreadSafe();
                }
            }
            return instance;
        }

        // Lastly we have code that can be accessed on the instance like any other class.
        public void UpdateSessionId() => sessionId = new Random().Next();
    }
}