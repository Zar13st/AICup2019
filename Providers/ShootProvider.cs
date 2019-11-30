using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class ShootProvider
    {
        public bool GetShoot(Unit player, Unit target, Game game)
        {
            if (!player.Weapon.HasValue) return false;

            return IsTargetInSight(player, target, game);
        }

        private bool IsTargetInSight(Unit player, Unit target, Game game)
        {
            int dirX = player.Position.X < target.Position.X ? 1 : -1;
            int dirY = player.Position.Y < target.Position.Y ? 1 : -1;

            int playerX = (int)player.Position.X;
            int playerY = (int)player.Position.Y;

            int enemyX = (int)target.Position.X;
            int enemyY = (int)target.Position.Y;

            if (playerX != enemyX)
            {
                for (int i = playerX; enemyX - i != dirX; i += dirX)
                {
                    if (game.Level.Tiles[i][playerY] == Tile.Wall)
                    {
                        if (playerY == enemyY) return false;
                    }
                }
            }

            return true;
        }


    }
}