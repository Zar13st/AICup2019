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

            //цель на одной прямой
            var shootAngle = GetAngle(playerXdouble, playerYdouble, enemyXdouble, enemyYdouble);
            if (double.IsInfinity(shootAngle) || shootAngle > 1000000 || shootAngle < -1000000)
            {
                var topAngle = GetAngle(playerXdouble, playerYdouble, playerX + 1, playerY + 1);
                if (double.IsInfinity(topAngle))
                {
                    if (playerX < enemyX)
                    {
                        for (int i = playerX + 1; i < enemyX; i++)
                        {
                            if (game.Level.Tiles[i][playerY - 1] == Tile.Wall) return false;
                        }
                    }

                    if (playerX > enemyX)
                    {
                        for (int i = playerX - 1; i > enemyX; i--)
                        {
                            if (game.Level.Tiles[i][playerY - 1] == Tile.Wall) return false;
                        }
                    }
                }

                if (playerX < enemyX)
                {
                    for (int i = playerX + 1; i < enemyX; i++)
                    {
                        if (game.Level.Tiles[i][playerY] == Tile.Wall) return false;
                    }
                }

                if (playerX > enemyX)
                {
                    for (int i = playerX - 1; i > enemyX; i--)
                    {
                        if (game.Level.Tiles[i][playerY] == Tile.Wall) return false;
                    }
                }

                return true;
            }

            // </
            if (playerX >= enemyX && playerY > enemyY)
            {
                for (int i = playerX; i >= enemyX; i --)
                {
                    for (int j = playerY; j >= enemyY; j --)
                    {
                        if (game.Level.Tiles[i][j] != Tile.Wall) continue;

                        var top = new Vec2Double(i + 1, j);
                        var bot = new Vec2Double(i, j + 1);
                        var isCrossTile = IsCrossTile(playerXdouble, playerYdouble, shootAngle, ref top, ref bot);

                        debug.Draw(new CustomData.Polygon(new ColoredVertex[]
                        {
                            new ColoredVertex(new Vec2Float((float)playerXdouble, (float)playerYdouble), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                            new ColoredVertex(new Vec2Float(i, j + 1), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                            new ColoredVertex(new Vec2Float(i+1, j), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                        }));

                        if (isCrossTile) return false;
                    }
                }

                return true;
            }

            // <\
            if (playerX > enemyX && playerY <= enemyY)
            {
                for (int i = playerX; i >= enemyX; i--)
                {
                    for (int j = playerY; j <= enemyY; j++)
                    {
                        if (game.Level.Tiles[i][j] != Tile.Wall) continue;

                        var top = new Vec2Double(i, j);
                        var bot = new Vec2Double(i + 1, j + 1);
                        var isCrossTile = IsCrossTile(playerXdouble, playerYdouble, shootAngle, ref top, ref bot);

                        debug.Draw(new CustomData.Polygon(new ColoredVertex[]
                        {
                            new ColoredVertex(new Vec2Float((float)playerXdouble, (float)playerYdouble), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                            new ColoredVertex(new Vec2Float(i+1, j + 1), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                            new ColoredVertex(new Vec2Float(i, j), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                        }));

                        if (isCrossTile) return false;
                    }
                }

                return true;
            }

            // \>
            if (playerX <= enemyX && playerY > enemyY)
            {
                for (int i = playerX; i <= enemyX; i++)
                {
                    for (int j = playerY; j >= enemyY; j--)
                    {
                        if (game.Level.Tiles[i][j] != Tile.Wall) continue;

                        var top = new Vec2Double(i + 1, j + 1);
                        var bot = new Vec2Double(i, j);
                        var isCrossTile = IsCrossTile(playerXdouble, playerYdouble, shootAngle, ref top, ref bot);

                        debug.Draw(new CustomData.Polygon(new ColoredVertex[]
                        {
                            new ColoredVertex(new Vec2Float((float)playerXdouble, (float)playerYdouble), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                            new ColoredVertex(new Vec2Float(i + 1, j + 1), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                            new ColoredVertex(new Vec2Float(i, j), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                        }));

                        if (isCrossTile) return false;
                    }
                }

                return true;
            }

            // />
            if (playerX < enemyX && playerY <= enemyY)
            {
                for (int i = playerX; i <= enemyX; i++)
                {
                    for (int j = playerY; j <= enemyY; j++)
                    {
                        if (game.Level.Tiles[i][j] != Tile.Wall) continue;

                        var top = new Vec2Double(i, j + 1);
                        var bot = new Vec2Double(i + 1, j);
                        var isCrossTile = IsCrossTile(playerXdouble, playerYdouble, shootAngle, ref top, ref bot);

                        debug.Draw(new CustomData.Polygon(new ColoredVertex[]
                        {
                            new ColoredVertex(new Vec2Float((float)playerXdouble, (float)playerYdouble), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                            new ColoredVertex(new Vec2Float(i , j + 1), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                            new ColoredVertex(new Vec2Float(i +1, j), isCrossTile ? new ColorFloat(255,0,0,0.4f) : new ColorFloat(0,255,0,0.4f)),
                        }));

                        if (isCrossTile) return false;
                    }
                }

                return true;
            }
            return true;
        }

        private bool IsCrossTile(double playerX, double playerY, double shootAngle, ref Vec2Double top, ref Vec2Double bot)
        {
            var topAngle = GetAngle(playerX, playerY, top.X, top.Y);

            var botAngle = GetAngle(playerX, playerY, bot.X, bot.Y);

            if (double.IsInfinity(topAngle))
            {
                return botAngle <= shootAngle;
            }

            if (double.IsInfinity(botAngle))
            {
                return topAngle >= shootAngle;
            }

            return (topAngle >= shootAngle || topAngle > 1000000 || topAngle < -1000000) && (botAngle <= shootAngle || botAngle > 1000000 || botAngle < -1000000);
        }

        private double GetAngle(double x1, double y1, double x2, double y2)
        {
            var angle = (x2 - x1) / (y1 - y2);
            return angle;
        }
    }
}