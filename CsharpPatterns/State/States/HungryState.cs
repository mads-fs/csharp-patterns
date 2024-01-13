namespace State
{
    public class HungryState : AbstractState
    {
        protected override int HungerThreshold { get { return 20; } }
        protected override int EnergyThreshold { get { return 10; } }

        private Vector2Int foodTarget = Vector2Int.MinusOne;

        public HungryState(Agent agent) : base(agent) { }

        public override void Evaluate()
        {
            if (this.agent.Energy <= EnergyThreshold)
            {
                this.agent.ChangeState(new SleepingState(this.agent));
            }
            else if (this.agent.Hunger <= HungerThreshold)
            {
                if (this.agent.Energy <= EnergyThreshold)
                {
                    this.agent.ChangeState(new SleepingState(this.agent));
                }
                else
                {
                    this.agent.ChangeState(new IdleState(this.agent));
                }
            }
            else
            {
                // Here we either find a Food Spot to aim for if we have none
                // or we update the current one if it was eaten between two frames
                if (this.foodTarget == Vector2Int.MinusOne || World.Map[this.foodTarget.X, this.foodTarget.Y] != "FD")
                {
                    Random rng = new();
                    List<Vector2Int> spots = World.FoodSpots;
                    this.foodTarget = spots[rng.Next(0, spots.Count)];
                }

                Vector2Int move =
                    World.MoveTowards(this.agent.Position.X, this.agent.Position.Y, this.foodTarget.X, this.foodTarget.Y);
                this.agent.Actions.Enqueue(new()
                {
                    Action = () => this.agent.Move(move.X, move.Y),
                    ActionName = $"MoveTowards({this.foodTarget.X},{this.foodTarget.Y})"
                });
            }
        }
    }
}
