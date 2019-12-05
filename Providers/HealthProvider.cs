using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class HealthProvider
    {
        private bool _medOnMap = true;
        LootBox? _blockLootBox = null;

        public LootBox? GetHealth(Unit unit, Game game, Unit enemy)
        {
            LootBox? health = null;
            if (!_medOnMap) return health;


            var unitX = (int)unit.Position.X;
            var unitY = (int)unit.Position.Y;
            var enemyX = (int)enemy.Position.X;
            var enemyY = (int)enemy.Position.Y;

            bool isBlockBox = false;
            foreach (var lootBox in game.LootBoxes)
            {
                if (!(lootBox.Item is Item.HealthPack)) continue;

                if (_blockLootBox.HasValue &&
                    (int) lootBox.Position.X == (int) _blockLootBox.Value.Position.X &&
                    (int) lootBox.Position.Y == (int) _blockLootBox.Value.Position.Y)
                {
                    isBlockBox = true;
                    continue;
                }

                if (!health.HasValue || unit.Position.DistanceSqr(lootBox.Position) < unit.Position.DistanceSqr(health.Value.Position))
                {
                    if ((unitX == enemyX && unitX == (int) lootBox.Position.X &&
                        (unitY >= enemyY && enemyY >= (int) lootBox.Position.Y ||
                         unitY <= enemyY && enemyY <= (int) lootBox.Position.Y)) ||
                        (unitY == enemyY && unitY == (int)lootBox.Position.Y &&
                         (unitX >= enemyX && enemyX >= (int)lootBox.Position.X ||
                          unitX <= enemyX && enemyX <= (int)lootBox.Position.X)))
                    {
                        if (_blockLootBox == null)
                        {
                            isBlockBox = true;
                            _blockLootBox = lootBox;
                            continue;
                        }
                    }

                    health = lootBox;
                }
            }

            if (!isBlockBox) _blockLootBox = null;

            if (health == null) health = _blockLootBox;

            if (health == null) _medOnMap = false;
            return health;
        }
    }
}