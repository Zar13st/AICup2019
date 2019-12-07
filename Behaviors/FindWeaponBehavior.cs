using System.Collections.Generic;
using System.Linq;
using AiCup2019.Model;
using AiCup2019.Pathfinding;
using AiCup2019.Providers;

namespace AiCup2019.Behaviors
{
    public class FindWeaponBehavior
    {
        private readonly JumpProvider _jumpProvider = new JumpProvider();
        private readonly AimProvider _aimProvider = new AimProvider();
        private readonly ShootProvider _shootProvider = new ShootProvider();

        private List<Vector2i> _path;


        public UnitAction GetAction(Unit unit, Game game, Unit enemy, Debug debug, Map map)
        {
            var nearestWeapon = GetWeapon(unit, game);

            var jumpData = _jumpProvider.GetJump(unit, game, nearestWeapon.Value.Position, enemy);

            var aim = _aimProvider.GetAim(unit, enemy);

            var shoot = _shootProvider.IsTargetInSight(unit, enemy, game, debug);

            var pathF = new PathFinderFast(map.MGrid, map);

            //if (_path == null /*||_path.Count < 2 || (_path[_path.Count - 2].x == (int)unit.Position.X && _path[_path.Count - 2].y == (int)unit.Position.Y) && map.IsGround((int)unit.Position.X, (int)unit.Position.Y)*/)
            //{
            _path = pathF.FindPath(new Vector2i((int)unit.Position.X, (int)unit.Position.Y),
                new Vector2i((int)nearestWeapon.Value.Position.X, (int)nearestWeapon.Value.Position.Y), 1, 2, 5, 10);
            //}

            //if (_path != null && _path.Count > 1)
            //{
                var last = new Vec2Float((float)enemy.Position.X, (float)enemy.Position.Y);
                foreach (var vector2I in _path)
                {
                    debug.Draw(new CustomData.Line(last,
                        new Vec2Float((float)(vector2I.x) + 0.5f, (float)(vector2I.y) + 0.5f),
                        0.1F,
                        new ColorFloat(0, 255, 0, 0.6F)));
                    last = new Vec2Float((float)(vector2I.x) + 0.5f, (float)(vector2I.y) + 0.5f);
                }

                //var next = _path[_path.Count - 2];
                //if (next.x == (int) unit.Position.X && next.y == (int) unit.Position.Y && _path.Count > 3)
                //{
                //    next = _path[_path.Count - 3];
                //}
                //return new UnitAction
                //{
                //    Velocity = ((double)(next.x) + 0.5d - unit.Position.X) * 100,
                //    Jump = next.y > (int)unit.Position.Y,
                //    JumpDown = next.y < (int)unit.Position.Y,
                //    Aim = aim,
                //    Shoot = unit.Weapon.HasValue && shoot,
                //    Reload = false,
                //    SwapWeapon = true,
                //    PlantMine = false
                //};
            //}
            //else
            //{
                return new UnitAction
                {
                    Velocity = (nearestWeapon.Value.Position.X - unit.Position.X) * 1000,
                    Jump = jumpData.jump,
                    JumpDown = jumpData.jumpDown,
                    Aim = aim,
                    Shoot = unit.Weapon.HasValue && shoot,
                    Reload = false,
                    SwapWeapon = true,
                    PlantMine = false
                };
           // }
        }

        private LootBox? GetWeapon(Unit unit, Game game)
        {
            LootBox? nearestWeapon = null;
            foreach (var lootBox in game.LootBoxes)
            {
                if (!(lootBox.Item is Item.Weapon weapon)) continue;

                if (weapon.WeaponType != WeaponType.Pistol) continue;

                if (!nearestWeapon.HasValue || unit.Position.DistanceSqr(lootBox.Position) < unit.Position.DistanceSqr(nearestWeapon.Value.Position))
                {
                    nearestWeapon = lootBox;
                }
            }

            return nearestWeapon;
        }
    }
}