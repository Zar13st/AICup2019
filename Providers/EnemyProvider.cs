using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class EnemyProvider
    {
        public Unit? GetEnemy(Unit unit, Game game)
        {
            Unit? nearestEnemy = null;
            foreach (var other in game.Units)
            {
                if (other.PlayerId != unit.PlayerId)
                {
                    if (!nearestEnemy.HasValue || unit.Position.DistanceSqr(other.Position) < unit.Position.DistanceSqr(nearestEnemy.Value.Position))
                    {
                        nearestEnemy = other;
                    }
                }
            }

            return nearestEnemy;
        }

        public Unit? GetEnemy1(Unit unit, Game game)
        {
            Unit? nearestEnemy = null;
            foreach (var other in game.Units)
            {
                if (other.PlayerId != unit.PlayerId)
                {
                    if (!nearestEnemy.HasValue || unit.Id < nearestEnemy.Value.Id)
                    {
                        nearestEnemy = other;
                    }
                }
            }

            return nearestEnemy;
        }

        public Unit? GetEnemy2(Unit unit, Game game)
        {
            Unit? nearestEnemy = null;
            foreach (var other in game.Units)
            {
                if (other.PlayerId != unit.PlayerId)
                {
                    if (!nearestEnemy.HasValue || unit.Id > nearestEnemy.Value.Id)
                    {
                        nearestEnemy = other;
                    }
                }
            }

            return nearestEnemy;
        }
    }
}