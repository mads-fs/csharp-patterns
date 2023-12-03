namespace Mediator
{
    /// <summary>
    /// An abstract class representing a Unit such as a Player, Enemy, or other similar entites.
    /// </summary>
    public abstract class Unit
    {
        public Unit(int rngSeed) => this.rng = new Random(new Random(rngSeed).Next());

        public abstract Guid Id { get; }
        public abstract string Name { get; }
        public abstract uint Health { get; }
        public abstract Guid Target { set; }

        protected Random rng;

        public abstract void Tick();
        public abstract void DamageTaken(DamageInfo info);
        public abstract void AddItemToInventory(ItemInfo itemInfo);
    }

    /// <summary>
    /// A simple Player representation with an Inventory.
    /// </summary>
    public class Player : Unit
    {
        public override Guid Id { get { return id; } }
        private readonly Guid id;
        public override string Name { get { return name; } }
        private readonly string name;
        public override uint Health { get { return health; } }
        private uint health;
        public override Guid Target { set { target = value; } }
        private Guid target;

        private readonly List<ItemInfo> inventory = new();

        public Player(string name, uint health, int rngSeed) : base(rngSeed)
        {
            this.id = Guid.NewGuid();
            this.name = name;
            this.health = health;
            GameEvents.DamageTaken += DamageTaken;
            GameEvents.HealingReceived += HealingReceived;
        }

        public override void AddItemToInventory(ItemInfo item) => inventory.Add(item);

        private void Attack(uint damage) => GameEvents.OnDamage(new DamageInfo(this.id, this.target, damage));

        public override void Tick()
        {
            if (health == 0) return;
            if (inventory.Count > 0 && rng.NextDouble() > 0.7)
            {
                ItemInfo item = inventory[rng.Next(0, inventory.Count)];
                item.Use();
                inventory.RemoveAt(inventory.IndexOf(item));
            }
            if (rng.NextDouble() > 0.49)
            {
                uint damage = (uint)rng.Next(4, 12);
                Attack(damage);
            }
        }

        public override void DamageTaken(DamageInfo info)
        {
            if (info.Target == id)
            {
                if (info.Damage >= health)
                {
                    health = 0;
                    Die(killer: info.Instigator);
                }
                else health -= info.Damage;
            }
        }

        private void HealingReceived(HealingInfo info)
        {
            if (info.Target == id)
            {
                health = Math.Min(health + info.Amount, 100);
            }
        }

        private void Die(Guid killer) => GameEvents.OnDeath(killer, this.id);
    }

    /// <summary>
    /// A simple Enemy representation without an Inventory.
    /// </summary>
    public class Enemy : Unit
    {
        public override Guid Id { get { return id; } }
        private readonly Guid id;
        public override string Name { get { return name; } }
        private readonly string name;
        public override uint Health { get { return health; } }
        private uint health;
        public override Guid Target { set { target = value; } }
        private Guid target;

        public Enemy(string name, uint health, int rngSeed) : base(rngSeed)
        {
            this.id = Guid.NewGuid();
            this.name = name;
            this.health = health;
            GameEvents.DamageTaken += DamageTaken;
        }

        public override void AddItemToInventory(ItemInfo itemInfo) { }

        private void Attack(uint damage) => GameEvents.OnDamage(new DamageInfo(this.id, this.target, damage));

        public override void Tick()
        {
            if (health == 0) return;
            if (rng.NextDouble() > 0.49)
            {
                uint damage = (uint)rng.Next(4, 12);
                Attack(damage);
            }
        }

        public override void DamageTaken(DamageInfo info)
        {
            if (info.Target == id)
            {
                if (info.Damage >= health)
                {
                    health = 0;
                    Die(info.Instigator);
                }
                else health -= info.Damage;
            }
        }

        private void Die(Guid killer) => GameEvents.OnDeath(killer, this.id);
    }
}
