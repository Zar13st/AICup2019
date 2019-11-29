using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class JumpProvider
    {
        public bool GetJump(Game game, Unit unit, Vec2Double targetPos)
        {
            bool jump = targetPos.Y > unit.Position.Y;
            if (targetPos.X > unit.Position.X && game.Level.Tiles[(int)(unit.Position.X + 1)][(int)(unit.Position.Y)] == Tile.Wall)
            {
                jump = true;
            }
            if (targetPos.X < unit.Position.X && game.Level.Tiles[(int)(unit.Position.X - 1)][(int)(unit.Position.Y)] == Tile.Wall)
            {
                jump = true;
            }

            return jump;
        }
    }
}