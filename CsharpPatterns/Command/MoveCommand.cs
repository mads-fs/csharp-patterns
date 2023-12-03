namespace Command
{
    /// <summary>
    /// This command represents a simple Move command. It stores what move a player
    /// made and when it Executes it contacts the publicly available Board Instance
    /// and then does the move. The board does not care where the call comes from
    /// because it is not responsible for filtering incoming calls to change board state.
    /// This way, the rest of the game behaves as if a player is playing, evne if all
    /// that's happening is that a bunch of Command objects are Executed in sequence.
    /// </summary>
    public class MoveCommand : ICommand
    {
        private readonly int x;
        private readonly int y;
        private readonly int value;

        public MoveCommand(int x, int y, int value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }

        public void Execute() => Program.BoardInstance.DoMove(x, y, value, true);
    }
}
