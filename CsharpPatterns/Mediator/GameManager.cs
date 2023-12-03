namespace Mediator
{
    /// <summary>
    /// A class that represents a typical Game Manager from a game. It keeps track 
    /// of overall game state by broadcasting that state to the console. What's more 
    /// is that the Game Manager does not need to know where any of those events come
    /// from or who triggered them as all the information needed is included in the
    /// event data objects.
    /// </summary>
    public class GameManager
    {
        private readonly Dictionary<Guid, Unit> unitMap;
        private readonly Dictionary<Guid, ItemInfo> itemMap;
        private readonly ItemInfo potion;

        public GameManager(Dictionary<Guid, Unit> unitMap, int rngSeed)
        {
            this.unitMap = unitMap;
            this.itemMap = new();

            this.potion = new Potion(15, Guid.NewGuid(), "Potion", "Potions", "Will heal minor injuries.");
            itemMap.Add(this.potion.Id, this.potion);

            GameEvents.DamageTaken += Damage;
            GameEvents.HealingReceived += Healing;
            GameEvents.ItemReceived += Item;
            GameEvents.Death += Death;
            SystemEvents.GameEnd += GameEnd;

            
            int potionAmount = new Random(rngSeed).Next(0, 4);
            Unit player = unitMap.First(x => x.Value is Player).Value;
            ((Potion)this.potion).Target = player.Id;
            for(int count = 0; count < potionAmount; count++)
            {
                player.AddItemToInventory(potion);
            }
        }

        private void Damage(DamageInfo info)
        {
            Unit instigator = unitMap[info.Instigator];
            Unit target = unitMap[info.Target];
            Console.WriteLine($"{instigator.Name} did {info.Damage} damage to {target.Name}. {target.Name} has {target.Health} health left.");
        }

        private void Healing(HealingInfo info)
        {
            Unit target = unitMap[info.Target];
            Console.WriteLine($"{target.Name} healed for {info.Amount} ({itemMap[info.Source].Name})");
        }

        private void Item(Guid receiver, ItemInfo item, uint amount)
        {
            Console.WriteLine($"{unitMap[receiver].Name} received {amount} {(amount == 1 ? item.Name : item.PluralName)}: '{item.Description}'");
        }

        private void Death(Guid killer, Guid target)
        {
            Console.WriteLine($"{unitMap[killer].Name} killed {unitMap[target].Name}");
            SystemEvents.OnGameEnd();
        }

        private void GameEnd() => Console.WriteLine("The Game Has Ended");
    }
}
