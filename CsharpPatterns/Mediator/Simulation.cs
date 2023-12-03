namespace Mediator
{
    /// <summary>
    /// This class represents a very simplistic and barebones Game Engine Simulation
    /// </summary>
    public class Simulation
    {
        private readonly Player player;
        private readonly Enemy enemy;
        private readonly GameManager manager;
        private readonly Random rng;

        /// <summary>
        /// Each run is seeded so that different outcomes can be tried determinastically.
        /// </summary>
        public Simulation(int rngSeed)
        {
            rng = new Random(rngSeed);
            this.player = new Player(name: "Player", health: 100, rngSeed);
            this.enemy = new Enemy(name: "Goblin", health: 100, rngSeed);
            this.player.Target = enemy.Id;
            this.enemy.Target = player.Id;

            Dictionary<Guid, Unit> unitMap = new()
            {
                {this.player.Id, player },
                {this.enemy.Id, enemy },
            };
            this.manager = new GameManager(unitMap, rngSeed);
        }

        /// <summary>
        /// Simulates the Update or Tick method of most game engines.
        /// </summary>
        public void Tick()
        {
            // random chance on who goes first
            double chance = rng.NextDouble();
            if (chance < 0.5) { player.Tick(); enemy.Tick(); }
            else { enemy.Tick(); player.Tick(); }
        }
    }
}