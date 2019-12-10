using AiCup2019.Model;

namespace aicup2019.Pathfinding
{
    public class JumpPadHelper
    {
        public Vec2Double Shift(Vec2Double target, Game game)
        {
            var targetX = (int) target.X;
            var targetY = (int) target.Y;

            if (game.Level.Tiles[targetX + 1][targetY] == Tile.JumpPad)
            {
                return new Vec2Double(target.X - 0.4f, target.Y);
            }

            if (game.Level.Tiles[targetX - 1][targetY] == Tile.JumpPad)
            {
                return new Vec2Double(target.X + 0.4f, target.Y);
            }

            return target;
        }
    }
}