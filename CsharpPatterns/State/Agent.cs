namespace State
{
    public class Agent
    {
        public Vector2Int Position { get { return this.position; } }
        private Vector2Int position;
        public string ShortName { get { return this.shortName; } }
        private readonly string shortName;
        public int Hunger { get { return hunger; } }
        private int hunger;
        public int Energy { get { return energy; } }
        private int energy;

        private readonly int minHunger = 0;
        private readonly int maxHunger = 100;

        private readonly int minEnergy = 0;
        private readonly int maxEnergy = 100;

        private string lastKnownAction = string.Empty;

        public Queue<AgentAction> Actions { get { return this.actions; } }
        private readonly Queue<AgentAction> actions = new(); // This is only done to have something to print to the console with ToString()

        private readonly string name;
        private AbstractState state;

        public struct AgentAction
        {
            public Action @Action;
            public string ActionName;
        }

        public Agent(int x, int y, string name, int hunger = 0, int energy = 0)
        {
            this.position = new(x, y);
            this.name = name;
            this.shortName = $"{name[0]}{name[^1]}";
            this.state = new IdleState(this);
            Random rng = new();
            this.hunger = hunger == 0 ? rng.Next(this.minHunger, 30) : hunger;
            this.energy = energy == 0 ? rng.Next(10, this.maxEnergy) : energy;
        }

        public void ChangeState(AbstractState state) => this.state = state;
        public void SetPosition(int x, int y) => this.position = new(x, y);

        public void Idle() => this.hunger += Math.Min(this.maxHunger, this.hunger + 3);

        public void Move(int x, int y)
        {
            hunger = Math.Min(hunger + 3, this.maxHunger);
            energy = Math.Max(this.minEnergy, energy - 5);
            this.SetPosition(x, y);
        }

        public void Eat(int hunger, int energy)
        {
            this.hunger = Math.Max(this.minHunger, this.hunger - hunger);
            this.energy = Math.Min(this.maxEnergy, this.energy + energy);
        }

        public void Sleep() => this.energy = Math.Min(this.maxEnergy, this.energy + 20);

        public void Update()
        {
            state.Evaluate();
            if (this.actions.Count > 0)
            {
                AgentAction action = this.actions.Dequeue();
                this.lastKnownAction = action.ActionName;
                action.Action?.Invoke();
            }
        }

        public override string ToString()
        {
            return $"{this.name} ({this.ShortName})¤" +
                $"{this.position}, ({state.GetType().Name})¤" +
                $"Energy: {this.energy}¤" +
                $"Hunger: {this.hunger}¤" +
                $"Action: {this.lastKnownAction}";
        }
    }
}