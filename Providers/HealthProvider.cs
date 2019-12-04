using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class HealthProvider
    {
        public LootBox? GetHealth(Unit unit, Game game)
        {
            LootBox? health = null;
            foreach (var lootBox in game.LootBoxes)
            {
                if (!(lootBox.Item is Item.HealthPack)) continue;

                if (!health.HasValue || unit.Position.DistanceSqr(lootBox.Position) < unit.Position.DistanceSqr(health.Value.Position))
                {
                    health = lootBox;
                }
            }

            return health;
        }
    }
}