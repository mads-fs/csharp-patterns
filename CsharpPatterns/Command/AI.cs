namespace Command
{
    /// <summary>
    /// This class represents a very naïve implementation of an AI opponent. It will be unlikely to make
    /// the best move at all times, but it will be generally thorough enough for the purposes of this example.
    /// </summary>
    public static class AI
    {
        private static readonly Random rng = new(0);
        public static (int, int) GetMove()
        {
            int[,] boardState = Program.BoardInstance.State;
            (int, int) winningMove = CheckWinningMove(boardState);
            if (winningMove.Item1 != -1) return winningMove;

            (int, int) blockingMove = CheckBlockingMove(boardState);
            if (blockingMove.Item1 != -1) return blockingMove;

            // We have no winning or blocking move, so we pick a random square.
            // In a better implementation the AI will look for the best move
            // using something like a Min-Max algorithm.

            // Check for the most advantageoous square first
            if (boardState[1, 1] == 0) return (1, 1);
            // Pick a random square
            List<(int, int)> emptySquares = GetEmptySquares(boardState);
            return emptySquares[rng.Next(0, emptySquares.Count)];
        }

        private static (int, int) CheckWinningMove(int[,] boardState)
        {
            List<(int, int)> emptySquares = GetEmptySquares(boardState);
            foreach ((int, int) value in emptySquares)
            {
                if (CheckSquareForWinningMove(value, boardState)) return value;
            }
            return (-1, -1);
        }

        private static (int, int) CheckBlockingMove(int[,] boardState)
        {
            List<(int, int)> emptySquares = GetEmptySquares(boardState);
            foreach ((int, int) value in emptySquares)
            {
                if (CheckSquareForBlockingMove(value, boardState)) return value;
            }
            return (-1, -1);
        }

        private static List<(int, int)> GetEmptySquares(int[,] boardState)
        {
            List<(int, int)> emptySquares = new();
            for (int x = 0; x < boardState.GetLength(0); x++)
            {
                for (int y = 0; y < boardState.GetLength(1); y++)
                {
                    if (boardState[x, y] == 0) emptySquares.Add((x, y));
                }
            }
            return emptySquares;
        }

        private static bool CheckSquareForWinningMove((int, int) square, int[,] boardState)
        {
            if (CheckDiagonalForSymbol(square, boardState, 2)) return true;
            if (CheckRowForSymbol(square, boardState, 2)) return true;
            if (CheckColumnnForSymbol(square, boardState, 2)) return true;
            return false;
        }

        private static bool CheckSquareForBlockingMove((int, int) square, int[,] boardState)
        {
            if (CheckDiagonalForSymbol(square, boardState, 1)) return true;
            if (CheckRowForSymbol(square, boardState, 1)) return true;
            if (CheckColumnnForSymbol(square, boardState, 1)) return true;
            return false;
        }

        private static bool CheckDiagonalForSymbol((int, int) move, int[,] boardState, int symbol)
        {
            int opposing = (symbol == 1 ? 2 : 1);
            // If it's not a corner, or the center, we return false
            if (move != (0, 0) && move != (0, 2) &&
                move != (2, 0) && move != (2, 2) &&
                move != (1, 1)) return false;
            // If the center is the opposing symbol then the corner squares will never
            // net a diagonal win.
            if (boardState[1, 1] == opposing) return false;
            // Top Left to Bottom Right
            if (move == (0, 0)) return (boardState[1, 1] == symbol && boardState[2, 2] == symbol);
            // Top Right to Bottom Left
            if (move == (0, 2)) return (boardState[1, 1] == symbol && boardState[2, 0] == symbol);
            // Bottom Left to Top Right
            if (move == (2, 0)) return (boardState[1, 1] == symbol && boardState[0, 2] == symbol);
            // Bottom Right to Top Left
            if (move == (2, 2)) return (boardState[1, 1] == symbol && boardState[0, 0] == symbol);
            // If we for some reason have no moves at all, we end up here and return false
            return false;
        }

        private static bool CheckRowForSymbol((int, int) move, int[,] boardState, int symbol)
        {
            int opposing = (symbol == 1 ? 2 : 1);
            // Top Row
            if (move == (0, 0) || move == (0, 1) || move == (0, 2))
            {
                // If any of the spots are the opposing symbol, then this will never be a winning move.
                if (boardState[0, 0] == 1 || boardState[0, 1] == 1 || boardState[0, 2] == opposing) return false;
                // Top Left
                if (move == (0, 0) && boardState[0, 1] == symbol && boardState[0, 2] == symbol) return true;
                // Top Center
                if (move == (0, 1) && boardState[0, 0] == symbol && boardState[0, 2] == symbol) return true;
                // Top Right
                if (move == (0, 2) && boardState[0, 0] == symbol && boardState[0, 1] == symbol) return true;
            }
            // Middle Row
            if (move == (1, 0) || move == (1, 1) || move == (1, 2))
            {
                // If any of the spots are the opposing symbol, then this will never be a winning move.
                if (boardState[1, 0] == 1 || boardState[1, 1] == 1 || boardState[2, 1] == opposing) return false;
                // Middle Left
                if (move == (1, 0) && boardState[1, 1] == symbol && boardState[1, 2] == symbol) return true;
                // Middle Center
                if (move == (1, 1) && boardState[1, 0] == symbol && boardState[1, 2] == symbol) return true;
                // Middle Right
                if (move == (1, 2) && boardState[1, 0] == symbol && boardState[1, 1] == symbol) return true;
            }
            // Bottom Row
            if (move == (2, 0) || move == (2, 1) || move == (2, 2))
            {
                // If any of the spots are the opposing symbol, then this will never be a winning move.
                if (boardState[2, 0] == 1 || boardState[2, 1] == 1 || boardState[2, 2] == opposing) return false;
                // Bottom Left
                if (move == (2, 0) && boardState[2, 1] == symbol && boardState[2, 2] == symbol) return true;
                // Bottom Center
                if (move == (2, 1) && boardState[2, 0] == symbol && boardState[2, 2] == symbol) return true;
                // Bottom Right
                if (move == (2, 2) && boardState[2, 0] == symbol && boardState[2, 1] == symbol) return true;
            }
            // If we for some reason have no moves at all, we end up here and return false
            return false;
        }

        private static bool CheckColumnnForSymbol((int, int) move, int[,] boardState, int symbol)
        {
            int opposing = (symbol == 1 ? 2 : 1);
            // Left Column
            if (move == (0, 0) || move == (1, 0) || move == (2, 0))
            {
                // If any of the spots are the opposing symbol, then this will never be a winning move.
                if (boardState[0, 0] == 1 || boardState[1, 0] == 1 || boardState[2, 0] == opposing) return false;
                // Top
                if (move == (0, 0) && boardState[1, 0] == symbol && boardState[2, 0] == symbol) return true;
                // Center
                if (move == (1, 0) && boardState[0, 0] == symbol && boardState[2, 0] == symbol) return true;
                // Bottom
                if (move == (2, 0) && boardState[0, 0] == symbol && boardState[1, 0] == symbol) return true;
            }
            // Middle Column
            if (move == (0, 1) || move == (1, 1) || move == (1, 2))
            {
                // If any of the spots are the opposing symbol, then this will never be a winning move.
                if (boardState[0, 1] == 1 || boardState[1, 1] == 1 || boardState[2, 1] == opposing) return false;
                // Top
                if (move == (0, 1) && boardState[1, 1] == symbol && boardState[2, 1] == symbol) return true;
                // Center
                if (move == (1, 1) && boardState[0, 1] == symbol && boardState[2, 1] == symbol) return true;
                // Bottom
                if (move == (2, 1) && boardState[0, 1] == symbol && boardState[1, 1] == symbol) return true;
            }
            // Right Column
            if (move == (0, 2) || move == (1, 2) || move == (2, 2))
            {
                // If any of the spots are the opposing symbol, then this will never be a winning move.
                if (boardState[0, 2] == 1 || boardState[1, 2] == 1 || boardState[2, 2] == opposing) return false;
                // Bottom Left
                if (move == (0, 2) && boardState[1, 2] == symbol && boardState[2, 2] == symbol) return true;
                // Bottom Center
                if (move == (1, 2) && boardState[0, 2] == symbol && boardState[2, 2] == symbol) return true;
                // Bottom Right
                if (move == (2, 2) && boardState[0, 2] == symbol && boardState[1, 2] == symbol) return true;
            }
            // If we for some reason have no moves at all, we end up here and return false
            return false;
        }
    }
}
