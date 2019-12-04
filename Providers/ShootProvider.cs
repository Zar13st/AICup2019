using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class ShootProvider
    {
        public bool IsTargetInSight(Unit player, Unit target, Game game, Debug debug)
        {
            int playerX = (int)player.Position.X;
            int playerY = (int)player.Position.Y + 1;

            int enemyX = (int)target.Position.X;
            int enemyY = (int)target.Position.Y + 1;

            if (playerX == enemyX && playerY == enemyY) return true;

            double playerXdouble = player.Position.X;
            double playerYdouble = player.Position.Y + 1;

            double enemyXdouble = target.Position.X;
            double enemyYdouble = target.Position.Y + 1;

            int dirX = playerXdouble < enemyXdouble ? 1 : -1;
            int dirY = playerYdouble < enemyYdouble ? 1 : -1;

            var shootAngle = GetAngle(playerXdouble, playerYdouble, enemyXdouble, enemyYdouble);
            if (double.IsInfinity(shootAngle))
            {
                if (playerX == enemyX) return true;

                for (int i = playerX; enemyX - i != dirX; i += dirX)
                {
                    if (game.Level.Tiles[i][playerY] == Tile.Wall || game.Level.Tiles[i][playerY - 1] == Tile.Wall)
                    {
                        return false;
                    }
                }
                return true;
            }

            if (playerY == enemyY || playerY + 1 == enemyY)
            {
                for (int i = playerX; enemyX - i != dirX; i += dirX)
                {
                    if (game.Level.Tiles[i][playerY - 1] == Tile.Wall)
                    {
                        return false;
                    }
                }

                return true;
            }

            for (int i = playerX; i != enemyX + dirX; i += dirX)
            {
                for (int j = playerY; j != enemyY + dirY; j += dirY)
                {
                    if (game.Level.Tiles[i][j] == Tile.Wall)
                    {
                        bool isCrossTile;
                        if (dirX == 1)
                        {
                            if (dirY == 1)
                            {
                                var top = new Vec2Double(i, j + 1);
                                var bot = new Vec2Double(i + 1, j);
                                isCrossTile = IsCrossTile(playerXdouble, playerYdouble, shootAngle, ref top, ref bot);

                                debug.Draw(new CustomData.Polygon(new ColoredVertex[]
                                {
                                    new ColoredVertex(new Vec2Float((float)playerXdouble, (float)playerYdouble), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                    new ColoredVertex(new Vec2Float(i , j + 1), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                    new ColoredVertex(new Vec2Float(i +1, j), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                }));
                            }
                            else
                            {
                                var top = new Vec2Double(i + 1, j + 1);
                                var bot = new Vec2Double(i, j);
                                isCrossTile = IsCrossTile(playerXdouble, playerYdouble, shootAngle, ref top, ref bot);

                                debug.Draw(new CustomData.Polygon(new ColoredVertex[]
                                {
                                    new ColoredVertex(new Vec2Float((float)playerXdouble, (float)playerYdouble), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                    new ColoredVertex(new Vec2Float(i + 1, j + 1), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                    new ColoredVertex(new Vec2Float(i, j), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                }));
                            }
                        }
                        else
                        {
                            if (dirY == 1)
                            {
                                var top = new Vec2Double(i, j);
                                var bot = new Vec2Double(i + 1, j + 1);
                                isCrossTile = IsCrossTile(playerXdouble, playerYdouble, shootAngle, ref top, ref bot);

                                debug.Draw(new CustomData.Polygon(new ColoredVertex[]
                                {
                                    new ColoredVertex(new Vec2Float((float)playerXdouble, (float)playerYdouble), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                    new ColoredVertex(new Vec2Float(i+1, j + 1), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                    new ColoredVertex(new Vec2Float(i, j), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                }));
                            }
                            else
                            {
                                var top = new Vec2Double(i + 1, j);
                                var bot = new Vec2Double(i, j + 1);
                                isCrossTile = IsCrossTile(playerXdouble, playerYdouble, shootAngle, ref top, ref bot);

                                debug.Draw(new CustomData.Polygon(new ColoredVertex[]
                                {
                                    new ColoredVertex(new Vec2Float((float)playerXdouble, (float)playerYdouble), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                    new ColoredVertex(new Vec2Float(i, j + 1), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                    new ColoredVertex(new Vec2Float(i+1, j), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                                }));
                            }
                        }

                        if (isCrossTile) return false;
                    }
                }
            }

            return true;
        }

        private bool IsCrossTile(double playerX, double playerY, double shootAngle, ref Vec2Double top, ref Vec2Double bot)
        {
            var topAngle = GetAngle(playerX, playerY, top.X, top.Y);

            var botAngle = GetAngle(playerX, playerY, bot.X, bot.Y);

            var isCrossTile = topAngle >= shootAngle && (botAngle <= shootAngle || botAngle > 10000000 || botAngle < -10000000);

            return isCrossTile;
        }

        private double GetAngle(double x1, double y1, double x2, double y2)
        {
            var angle = (x2 - x1) / (y1 - y2);
            return angle;
        }
    }
}