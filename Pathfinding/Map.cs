using AiCup2019.Model;
using aicup2019.Pathfinding;

namespace AiCup2019.Pathfinding
{
    public class Map
    {
        private Game _game;

        public byte[,] MGrid { get; set; } = new byte[64, 64];

        public void SetMap(Game game, Debug debug, Unit enemy, TargetEnum targetType)
        {
            _game = game;
            for (int x = 1; x < 39; x++)
            {
                for (int y = 1; y < 29; y++)
                {
                    if ((game.Level.Tiles[x][y] != Tile.Wall && !(game.Level.Tiles[x][y + 1] == Tile.Wall && game.Level.Tiles[x][y - 1] == Tile.Wall))&&
                        (game.Level.Tiles[x][y] != Tile.JumpPad && game.Level.Tiles[x + 1][y] != Tile.JumpPad && game.Level.Tiles[x - 1][y] != Tile.JumpPad))
                    {
                        if (targetType != TargetEnum.Enemy && x == (int)enemy.Position.X  && (y == (int)enemy.Position.Y || y == (int)enemy.Position.Y + 1))
                        {
                            MGrid[x, y] = 0;
                            debug.Draw(new CustomData.Rect(new Vec2Float(x, y), new Vec2Float(1, 1), new ColorFloat(255, 0, 0, 0.4F)));
                        }
                        else
                        {
                            MGrid[x, y] = 1;
                        }
                    }
                    else
                    {
                        MGrid[x, y] = 0;
                        debug.Draw(new CustomData.Rect(new Vec2Float(x, y), new Vec2Float(1, 1), new ColorFloat(255, 0, 0, 0.4F)));
                    }
                }
            }
        }

        public bool IsOneWayPlatform(int x, int y)
        {
            return _game.Level.Tiles[x][y - 1] == Tile.Platform ||
                   _game.Level.Tiles[x][y - 1] == Tile.Ladder ||
                   _game.Level.Tiles[x][y] == Tile.Ladder;
        }

        public bool IsGround(int x, int y)
        {
            return _game.Level.Tiles[x][y - 1] == Tile.Wall ||
                   _game.Level.Tiles[x][y - 1] == Tile.Platform ||
                   _game.Level.Tiles[x][y - 1] == Tile.Ladder ||
                   _game.Level.Tiles[x][y] == Tile.Ladder ||
                   _game.Level.Tiles[x][y] == Tile.JumpPad;
        }

        public bool IsJumpPad(int x, int y)
        {
            return _game.Level.Tiles[x][y] == Tile.JumpPad;
        }

        public bool IsJumpPadJumpStop(int x, int y)
        {
            return _game.Level.Tiles[x][y + 2] == Tile.Wall ||
                   _game.Level.Tiles[x][y - 1] == Tile.Wall ||
                   _game.Level.Tiles[x][y] == Tile.Ladder ||
                   _game.Level.Tiles[x][y + 1] == Tile.Ladder;
        }
    }
}