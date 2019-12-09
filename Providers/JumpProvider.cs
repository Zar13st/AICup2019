using System;
using AiCup2019.Model;
using AiCup2019.Pathfinding;

namespace AiCup2019.Providers
{
    public class JumpProvider
    {
        private bool _lastTickOnPlatform = false;

        private int _lastPlatformX = 0;
        private int _lastPlatformY = 0;

        public (bool jump, bool jumpDown) GetJumpForPath(Unit unit, Game game, Vector2I next)
        {
            if ((int)(unit.Position.X) != _lastPlatformX && (int)(unit.Position.Y) != _lastPlatformY + 1 && IsOverPlatform(unit, game))
            {
                _lastPlatformX = (int) (unit.Position.X);
                _lastPlatformY = (int) (unit.Position.Y) - 1;
                return (false, false);
            }

            bool jump = next.Y > (int)unit.Position.Y;

            bool jumpDown = next.Y < (int) unit.Position.Y;

            return (jump, jumpDown);
        }

        public (bool jump, bool jumpDown) GetJump(Unit unit, Game game, Vec2Double targetPos, Unit enemy)
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

            if (unit.Position.DistanceSqr(enemy.Position) < 9)
            {
                jump = true;
            }

            if (Math.Abs(unit.Position.X - enemy.Position.X) < 0.9 && 
                unit.Position.Y - enemy.Position.Y > 1 &&
                unit.Position.Y - enemy.Position.Y < 3 &&
                unit.Health > Extensions.HealthForRunToMed)
            {
                jump = false;
            }

            return (jump, !jump);
        }

        private bool IsOnPlatform(Unit unit, Game game)
        {
            return game.Level.Tiles[(int) (unit.Position.X)][(int) (unit.Position.Y)] == Tile.Platform;
        }

        private bool IsOverPlatform(Unit unit, Game game)
        {
            return game.Level.Tiles[(int)(unit.Position.X)][(int)(unit.Position.Y) - 1] == Tile.Platform;
        }
    }
}