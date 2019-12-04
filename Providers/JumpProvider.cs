using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class JumpProvider
    {
        private bool _lastTickOnPlatform = false;

        public (bool jump, bool jumpDown) GetJump(Unit unit, Game game, Vec2Double targetPos)
        {
            var isOnPlatform = IsOnPlatform(unit, game);

            if (_lastTickOnPlatform && !isOnPlatform)
            {
                _lastTickOnPlatform = false;
                return (false, false);
            }

            _lastTickOnPlatform = isOnPlatform;

            bool jump = targetPos.Y > unit.Position.Y;
            if (targetPos.X > unit.Position.X && game.Level.Tiles[(int)(unit.Position.X + 1)][(int)(unit.Position.Y)] == Tile.Wall)
            {
                jump = true;
            }
            if (targetPos.X < unit.Position.X && game.Level.Tiles[(int)(unit.Position.X - 1)][(int)(unit.Position.Y)] == Tile.Wall)
            {
                jump = true;
            }

            return (jump, !jump);
        }

        private bool IsOnPlatform(Unit unit, Game game)
        {
            return game.Level.Tiles[(int) (unit.Position.X)][(int) (unit.Position.Y)] == Tile.Platform;
        }
    }
}