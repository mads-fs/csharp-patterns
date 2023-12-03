namespace Command
{
    /// <summary>
    /// This class reprents the Tic-Tac-Toe board and the state of it.
    /// </summary>
    public class Board
    {
        public int[,] State { get { return state; } }
        private int[,] state;
        public List<ICommand> Replay { get { return replay; } }
        private readonly List<ICommand> replay;

        public Board()
        {
            state = new int[3, 3]
            {
                {0, 0, 0},
                {0, 0, 0},
                {0, 0, 0}
            };
            replay = new List<ICommand>();
        }

        public void Reset() => state = new int[3, 3]
            {
                {0, 0, 0},
                {0, 0, 0},
                {0, 0, 0}
            };

        public bool DoMove(int x, int y, int value, bool command = false)
        {
            if (state[x, y] != 0) return false;
            if (x < 0 || x > state.GetLength(0)) return false;
            if (y < 0 || y > state.GetLength(1)) return false;
            state[x, y] = value;
            if (!command) replay.Add(new MoveCommand(x, y, value));
            return true;
        }

        public void PrintState()
        {
            Console.WriteLine("   | 0 | 1 | 2 | Y");
            Console.WriteLine($"-------------------");
            for (int x = 0; x < 3; x++)
            {
                char symbol1 = GetSymbol(x, 0);
                char symbol2 = GetSymbol(x, 1);
                char symbol3 = GetSymbol(x, 2);
                Console.WriteLine($" {x} | {symbol1} | {symbol2} | {symbol3} |");
                Console.WriteLine($"----------------");
            }
            Console.WriteLine(" X |");
            Console.WriteLine("");
        }

        private char GetSymbol(int x, int y)
            => state[x, y] switch
            {
                1 => 'X',
                2 => 'O',
                _ => ' ',
            };

        public void EvaluateBoard()
        {
            if (CheckPlayerWin(1) || CheckPlayerWin(2) || !FlattenState().Any(x => x == 0))
            {
                Program.EndGame();
            }
        }

        private int[] FlattenState()
        {
            int[] newArr = new int[state.GetLength(0) * state.GetLength(1)];
            int counter = 0;
            foreach (int i in state)
            {
                newArr[counter] = i;
                counter++;
            }
            return newArr;
        }

        private bool CheckPlayerWin(int value)
        {
            if (CheckRow(0, value)) return true;
            if (CheckRow(1, value)) return true;
            if (CheckRow(2, value)) return true;
            if (CheckColumn(0, value)) return true;
            if (CheckColumn(1, value)) return true;
            if (CheckColumn(2, value)) return true;
            if (CheckDiagonals(value)) return true;
            return false;
        }

        private bool CheckRow(int x, int value)
            => state[x, 0] == value &&
                state[x, 1] == value &&
                state[x, 2] == value;
        private bool CheckColumn(int y, int value)
            => state[0, y] == value &&
                state[1, y] == value &&
                state[2, y] == value;
        private bool CheckDiagonals(int value)
        {
            if (state[0, 0] == value &&
                state[1, 1] == value &&
                state[2, 2] == value) return true;
            if (state[2, 0] == value &&
                state[1, 1] == value &&
                state[0, 2] == value) return true;
            return false;
        }
    }
}
