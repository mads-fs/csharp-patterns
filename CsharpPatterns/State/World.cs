#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE1006 // Naming Styles

using System.Text;

namespace State
{
    /// <summary>
    /// This class represents a simple World that the agents can navigate and interact in.
    /// </summary>
    public class World
    {
        public int XMax { get { return xMax; } }
        private readonly int xMax = 10;
        public int YMax { get { return yMax; } }
        private readonly int yMax = 10;
        public static string[,] Map { get { return map; } }
        private static string[,] map = new string[0, 0];

        private static readonly int maxFoodPieces = 4;
        private int currentFoodSpaces = 0;
        private readonly List<Vector2Int> foodSpaces;

        private readonly Random rng = new();

        private List<Vector2Int> freeSpaces
        {
            get
            {
                List<Vector2Int> result = new();
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x, y] == "  ")
                        {
                            result.Add(new Vector2Int(x, y));
                        }
                    }
                }
                return result;
            }
        }

        public static List<Vector2Int> FoodSpots
        {
            get
            {
                List<Vector2Int> result = new(maxFoodPieces);
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x, y] == "FD") result.Add(new Vector2Int(x, y));
                    }
                }
                return result;
            }
        }

        private readonly Agent[] agents;
        private string[] agent1Strings = Array.Empty<string>();
        private string[] agent2Strings = Array.Empty<string>();

        public World()
        {
            this.foodSpaces = new(maxFoodPieces);

            map = new string[this.xMax, this.yMax];
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = "  ";
                }
            }

            this.agents = new Agent[]
            {
                new(this.rng.Next(0, map.GetLength(0)), this.rng.Next(0, map.GetLength(1)), "Agent1"),
                new(this.rng.Next(0, map.GetLength(0)), this.rng.Next(0, map.GetLength(1)), "Agent2"),
            };

            if (this.agents[0].Position == this.agents[1].Position)
            {
                this.agents[1].SetPosition(this.rng.Next(0, map.GetLength(0)), this.rng.Next(map.GetLength(1)));
            }
            SpawnFood();
        }

        public void Update()
        {
            ClearMap();
            foreach (Agent agent in this.agents)
            {
                if (foodSpaces.Contains(agent.Position))
                {
                    int hunger = this.rng.Next(30, 50);
                    int energy = this.rng.Next(10, 30);
                    agent.Eat(hunger, energy);
                    this.currentFoodSpaces = Math.Max(0, this.currentFoodSpaces - 1);
                    foodSpaces.Remove(agent.Position);
                }
                map[agent.Position.X, agent.Position.Y] = $"{agent.ShortName}";
            }
            foreach (Vector2Int foodSpot in this.foodSpaces) map[foodSpot.X, foodSpot.Y] = "FD";

            PrintWorldState();
            foreach (Agent agent in this.agents)
            {
                agent.Update();
            }
            PrintWorldState();
            SpawnFood();
        }

        private void ClearMap()
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = "  ";
                }
            }
        }

        /// <summary>This method is a stand-in in place of algorithms like A*.</summary>
        public static Vector2Int MoveTowards(int xStart, int yStart, int xDest, int yDest)
        {
            int xTarget = xStart;
            int yTarget = yStart;
            if (xDest < xStart) xTarget = Math.Max(0, xStart - 1);
            if (xDest > xStart) xTarget = Math.Min(xStart + 1, map.GetLength(0) - 1);
            if (yDest < yStart) yTarget = Math.Max(0, yStart - 1);
            if (yDest > yStart) yTarget = Math.Min(yStart + 1, map.GetLength(1) - 1);

            if (map[xTarget, yTarget].Contains('A'))
            {
                Random random = new();
                // If another Agent occupies this space, we offset randomly
                xTarget = Math.Clamp(xTarget + random.Next(-1, 1), 0, map.GetLength(0) - 1);
                yTarget = Math.Clamp(yTarget + random.Next(-1, 1), 0, map.GetLength(1) - 1);
            }

            return new(xTarget, yTarget);
        }

        private void SpawnFood()
        {
            if (this.currentFoodSpaces < maxFoodPieces)
            {
                int difference;
                if (this.currentFoodSpaces == 0) difference = maxFoodPieces;
                else difference = maxFoodPieces - this.currentFoodSpaces;

                List<Vector2Int> freeSpaces = this.freeSpaces;
                int counter = 0;
                do
                {
                    int index = this.rng.Next(0, freeSpaces.Count);
                    Vector2Int position = freeSpaces[index];
                    if (map[position.X, position.Y] == "  ")
                    {
                        this.foodSpaces.Add(position);
                        counter++;
                    }
                } while (counter < difference);
                this.currentFoodSpaces = maxFoodPieces;
            }
        }

        private void PrintWorldState()
        {
            Console.Clear();
            StringBuilder sb = new();
            this.agent1Strings = this.agents[0].ToString().Split('¤');
            this.agent2Strings = this.agents[1].ToString().Split('¤');
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    sb.Append($"[{map[x, y]}]");
                    if (x >= 0 && x <= 4 && y == map.GetLength(1) - 1)
                    {
                        sb.Append($" {this.agent1Strings[x]}");
                    }
                    else if (x >= 4 && y == map.GetLength(1) - 1)
                    {
                        sb.Append($" {this.agent2Strings[x - (int)(map.GetLength(1) * 0.5f)]}");
                    }
                }
                sb.Append('\n');
            }
            Console.WriteLine(sb.ToString());
        }
    }
}