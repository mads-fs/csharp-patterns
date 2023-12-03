namespace Mediator
{
    /// <summary>
    /// A class that represents relevant data for one <see cref="Unit"/> receiving Damage from another <see cref="Unit"/>.
    /// </summary>
    public class DamageInfo
    {
        public Guid Instigator { get { return instigator; } }
        private readonly Guid instigator;
        public Guid Target { get { return target; } }
        private readonly Guid target;
        public uint Damage { get { return damage; } }
        private readonly uint damage;

        public DamageInfo(Guid instigator, Guid target, uint damage)
        {
            this.instigator = instigator;
            this.target = target;
            this.damage = damage;
        }
    }

    /// <summary>
    /// A class that represents relevant data for one <see cref="Unit"/> receiving healing from an <see cref="ItemInfo"/>.
    /// </summary>
    public class HealingInfo
    {
        public Guid Target { get { return target; } }
        private readonly Guid target;
        public Guid Source { get { return source; } }
        private readonly Guid source;
        public uint Amount { get { return amount; } }
        private readonly uint amount;

        public HealingInfo(Guid target, Guid source, uint amount)
        {
            this.target = target;
            this.source = source;
            this.amount = amount;
        }
    }

    /// <summary>A class that represents relevant Data about an Item and provides a common method to Use it.</summary>
    public abstract class ItemInfo
    {
        public Guid Id { get { return id; } }
        private readonly Guid id;
        public string Name { get { return name; } }
        private readonly string name;
        public string PluralName { get { return pluralName; } }
        private readonly string pluralName;
        public string Description { get { return description; } }
        private readonly string description;

        public ItemInfo(Guid id, string singularName, string pluralName, string description)
        {
            this.id = id;
            this.name = singularName;
            this.pluralName = pluralName;
            this.description = description;
        }

        public abstract void Use();
    }

    /// <summary>
    /// A simple potion item that is given to the <see cref="Player"/> in this case for a bit of added flavor.
    /// </summary>
    public class Potion : ItemInfo
    {
        private readonly uint healAmount;
        public Guid Target { set { target = value; } }
        private Guid target;

        public Potion(uint healAmount, Guid id, string singularName, string pluralName, string description)
            : base(id, singularName, pluralName, description) 
            => this.healAmount = healAmount;

        public override void Use() => GameEvents.OnHealing(new(target, this.Id, healAmount));
    }
}
