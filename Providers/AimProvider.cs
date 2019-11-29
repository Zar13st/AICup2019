using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class AimProvider
    {
        public Vec2Double GetAim(Unit player, Unit? nearestEnemy)
        {
            Vec2Double aim = new Vec2Double(0, 0);

            if (nearestEnemy.HasValue)
            {
                aim = new Vec2Double(nearestEnemy.Value.Position.X - player.Position.X, nearestEnemy.Value.Position.Y - player.Position.Y);
            }

            return aim;
        }
    }
}