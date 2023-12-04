namespace Singleton
{
    /// <summary>
    /// The Singleton Pattern is a great pattern for when you have systems or code
    /// that only needs to be accessible from one point of your codebase. This
    /// pattern is unfortunately also often misused and disliked. When you have a
    /// class you know there will only ever be one of, then this pattern makes sense.
    /// In games the GameManager is often the most common usage of the Singleton pattern
    /// however you could just as well use it for an AudioManager, FXManager, Level Persistence
    /// and more. In this example we'll see a GameManager implementation but it should
    /// be mentioned that there are examples of the Singleton pattern being used throughout
    /// the Projects in this solution.
    /// </summary>
    public class Program
    {
        private static float time;
        static void Main()
        {
            Log($"Creating two instances of the {nameof(GameManager)} class");
            GameManager instance = GameManager.GetInstance();
            GameManager instance2 = GameManager.GetInstance();
            Log($"Are the two GameManager instances the same: {(instance == instance2 ? "Yes" : "No")}");
            Log($"----------");
            Log($"InstanceId: {GameManager.GetInstance().Id}");
            Log($"Current State: {GameManager.GetInstance().State}");
            Log($"-- Modifying State --");
            GameManager.GetInstance().State = "Changed";
            Log($"Current State: {GameManager.GetInstance().State}");
            Log($"SessionId: {GameManager.GetInstance().SessionId}");
            Log($"-- Modifying SessionId --");
            GameManager.GetInstance().UpdateSessionId();
            Log($"SessionId: {GameManager.GetInstance().SessionId}");
            Console.WriteLine("----- THREAD SAFE TEST -----");
            // For the purposes of showcasing the GameManagerThreadSafe version
            // we will only show that two threads trying to access the same instance
            // will not collide or cause issues because of the locking object.
            
            // What you will notice in the output is that one thread will reach
            // the instantiation clause first and print the Session Id. When it
            // does the other thread will have skipped the instantiation part and
            // instead read the session Id before it was changed and thus you can
            // see that both of the instances that the threads are operating on are
            // one and the same instance.
            Thread accessor1 = new(() =>
            {
                GameManagerThreadSafe.GetInstance();
                Console.WriteLine($"{nameof(accessor1)}: {GameManagerThreadSafe.GetInstance().SessionId}");
                Console.WriteLine($"{nameof(accessor1)}: -- Modifying SessionId --");
                GameManagerThreadSafe.GetInstance().UpdateSessionId();
                Console.WriteLine($"{nameof(accessor1)}: {GameManagerThreadSafe.GetInstance().SessionId}");

            });
            Thread accessor2 = new(() =>
            {
                GameManagerThreadSafe.GetInstance();
                Console.WriteLine($"{nameof(accessor2)}: {GameManagerThreadSafe.GetInstance().SessionId}");
                Console.WriteLine($"{nameof(accessor2)}: -- Modifying SessionId --");
                GameManagerThreadSafe.GetInstance().UpdateSessionId();
                Console.WriteLine($"{nameof(accessor2)}: {GameManagerThreadSafe.GetInstance().SessionId}");

            });
            accessor1.Start();
            accessor2.Start();

            accessor1.Join();
            accessor2.Join();
        }

        private static void Log(string message)
        {
            time += GameManager.GetInstance().Tick;
            Console.WriteLine($"[{time}]:{message}");
        }
    }
}
