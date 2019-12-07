using AiCup2019.Model;

namespace AiCup2019.Pathfinding
{
    public class Map
    {
        private readonly Game _game;

        public byte[,] MGrid { get; set; }

        public Map(Game game)
        {
            _game = game;
        }

        public bool IsOneWayPlatform(int x, int y)
        {
            return _game.Level.Tiles[x][y] == Tile.Platform || 
                   _game.Level.Tiles[x][y] == Tile.Ladder ||
                   _game.Level.Tiles[x][y + 1] == Tile.Ladder;
        }

        public bool IsGround(int x, int y)
        {
            return _game.Level.Tiles[x][y] == Tile.Wall ||
                   _game.Level.Tiles[x][y] == Tile.Platform ||
                   _game.Level.Tiles[x][y] == Tile.Ladder ||
                   _game.Level.Tiles[x][y + 1] == Tile.Ladder;
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