using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class AimProvider
    {
        public Vec2Double GetAim(Unit player, Unit nearestEnemy)
        {
            var aim = new Vec2Double(nearestEnemy.Position.X - player.Position.X, nearestEnemy.Position.Y - player.Position.Y - 0.5);

            return aim;
        }
    }
}