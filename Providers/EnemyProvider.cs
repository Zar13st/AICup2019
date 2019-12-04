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
    }
}