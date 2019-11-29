using AiCup2019.Model;

namespace AiCup2019
{
    public class MyStrategy
    {
        static double DistanceSqr(Vec2Double a, Vec2Double b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }
        public UnitAction GetAction(Unit unit, Game game, Debug debug)
        {
            Unit? nearestEnemy = null;
            foreach (var other in game.Units)
            {
                if (other.PlayerId != unit.PlayerId)
                {
                    if (!nearestEnemy.HasValue || DistanceSqr(unit.Position, other.Position) < DistanceSqr(unit.Position, nearestEnemy.Value.Position))
                    {
                        nearestEnemy = other;
                    }
                }
            }

            LootBox? nearestHealthPack = null;
            LootBox? nearestWeapon = null;
            foreach (var lootBox in game.LootBoxes)
            {
                if (lootBox.Item is Item.Weapon weapon)
                {
                    if (weapon.WeaponType == WeaponType.RocketLauncher)
                    {
                        continue;
                    }

                    if (!nearestWeapon.HasValue || DistanceSqr(unit.Position, lootBox.Position) < DistanceSqr(unit.Position, nearestWeapon.Value.Position))
                    {
                        nearestWeapon = lootBox;
                    }
                }

                if (lootBox.Item is Item.HealthPack)
                {
                    if (!nearestHealthPack.HasValue || DistanceSqr(unit.Position, lootBox.Position) < DistanceSqr(unit.Position, nearestHealthPack.Value.Position))
                    {
                        nearestHealthPack = lootBox;
                    }
                }
            }

            Vec2Double targetPos = unit.Position;
            if ((!unit.Weapon.HasValue || unit.Weapon?.Typ == WeaponType.RocketLauncher) && nearestWeapon.HasValue)
            {
                targetPos = nearestWeapon.Value.Position;
            }
            else if (nearestEnemy.HasValue && unit.Health >= 100)
            {
                targetPos = nearestEnemy.Value.Position;
            }
            else if (nearestHealthPack.HasValue)
            {
                targetPos = nearestHealthPack.Value.Position;
            }
            else if (nearestEnemy.HasValue)
            {
                targetPos = nearestEnemy.Value.Position;
            }

            debug.Draw(new CustomData.Log("Target pos: " + targetPos));
            Vec2Double aim = new Vec2Double(0, 0);
            if (nearestEnemy.HasValue)
            {
                aim = new Vec2Double(nearestEnemy.Value.Position.X - unit.Position.X, nearestEnemy.Value.Position.Y - unit.Position.Y);
            }
            bool jump = targetPos.Y > unit.Position.Y;
            if (targetPos.X > unit.Position.X && game.Level.Tiles[(int)(unit.Position.X + 1)][(int)(unit.Position.Y)] == Tile.Wall)
            {
                jump = true;
            }
            if (targetPos.X < unit.Position.X && game.Level.Tiles[(int)(unit.Position.X - 1)][(int)(unit.Position.Y)] == Tile.Wall)
            {
                jump = true;
            }

            var action = new UnitAction
            {
                Velocity = (targetPos.X - unit.Position.X) * 1000,
                Jump = jump,
                JumpDown = !jump,
                Aim = aim,
                Shoot = unit.Weapon.HasValue && unit.Weapon?.Typ != WeaponType.RocketLauncher,
                SwapWeapon = !unit.Weapon.HasValue || unit.Weapon?.Typ == WeaponType.RocketLauncher,
                PlantMine = false
            };

            return action;
        }
    }
}