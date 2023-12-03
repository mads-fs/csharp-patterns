namespace Command
{
    /// <summary>
    /// The Command Pattern is a useful behavioural pattern for when you need to create chains
    /// of requests that you can use later. Often these types of objects are used for things
    /// such as Replay Systems (as in this example), Undo/Redo, Turn-based games,
    /// seeded terrain modification, saving State and more. The neat part about Command objects
    /// is that you can make any number of them as long as they can implement the <see cref="ICommand"/>
    /// interface, and execute them in whatever order suits your use-cases. It isn't uncommon
    /// that in something like a turn-based game you have multiple commands being executed one
    /// after the other per-turn to make sure all of state is preserved throughout for later use.
    /// In this example, you will be playing a game of Tic-Tac-Toe/Crosses and Noughts against a
    /// very naïve <see cref="AI"/> implementation. Once the game is over, you will be shown a
    /// replay from start to finish of that game to showcase just one use-case of the Command 
    /// pattern in videogames.
    /// </summary>
    internal class Program
    {
        public static Board BoardInstance { get { return boardInstance; } }
        private readonly static Board boardInstance = new();
        private static bool gameRunning = true;
        private static int currentPlayer = 0;

        static void Main()
        {
            while (gameRunning)
            {
                if (gameRunning == false) break;
                Console.Clear();
                boardInstance.PrintState();
                Console.Write("Write where to place the Cross by writing two numbers separated by a comma (X,Y): ");
                if (currentPlayer == 0)
                {
                    string[]? input = Console.ReadLine()?.Split(",");
                    if (input != null && input.Length == 2)
                    {
                        try
                        {
                            int x = int.Parse(input[0]);
                            int y = int.Parse(input[1]);
                            if (boardInstance.DoMove(x, y, 1))
                            {
                                currentPlayer = 1;
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    // AI Player
                    (int, int) move = AI.GetMove();
                    if (move.Item1 != -1)
                    {
                        boardInstance.DoMove(move.Item1, move.Item2, 2);
                    }
                    currentPlayer = 0;
                }
                boardInstance.EvaluateBoard();
            }
            Console.Clear();
            boardInstance.PrintState();
            Console.WriteLine("Now showing a Replay of the last Game.");
            Task replayTask = Task.Run(() => Replay());
            while (!replayTask.IsCompleted) ;
            Console.ReadLine();
        }

        private static async void Replay()
        {
            List<ICommand>.Enumerator replay = boardInstance.Replay.GetEnumerator();
            boardInstance.Reset();
            Console.Clear();
            Console.WriteLine("Now showing a Replay of the last Game.");
            boardInstance.PrintState();
            await Task.Delay(1000);
            using (PeriodicTimer timer = new(TimeSpan.FromSeconds(2)))
            {
                while (await timer.WaitForNextTickAsync())
                {
                    if (replay.MoveNext())
                    {
                        replay.Current.Execute();
                        Console.Clear();
                        Console.WriteLine("Now showing a Replay of the last Game.");
                        Console.WriteLine("");
                        boardInstance.PrintState();
                    }
                    else break;
                }
            }
            Console.WriteLine("Replay Complete");
            await Task.CompletedTask;
        }

        public static void EndGame() => gameRunning = false;
    }
}
