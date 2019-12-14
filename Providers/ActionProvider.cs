using System.Collections.Generic;
using AiCup2019;
using AiCup2019.Model;
using AiCup2019.Pathfinding;
using AiCup2019.Providers;

namespace aicup2019.Providers
{
    public class ActionProvider
    {
        private readonly JumpProvider _jumpProvider = new JumpProvider();
        private readonly AimProvider _aimProvider = new AimProvider();
        private readonly ShootProvider _shootProvider = new ShootProvider();

        private List<Vector2I> _path;
        private int _i;
        private int _pathLength;
        private int _lastTargetX;
        private int _lastTargetY;

        public UnitAction GetAction(Unit unit, Game game, Unit enemy, Debug debug, Vec2Double target, Map map, WeaponType weaponType)
        {
            var aim = _aimProvider.GetAim(unit, enemy);

            var shoot = _shootProvider.IsTargetInSight(unit, enemy, game, debug);

            if (map.IsGround((int)unit.Position.X, (int)unit.Position.Y) &&
                (_path == null || _lastTargetX != (int)target.X || _lastTargetY != (int)target.Y || _pathLength > 60))
            {
                DoPath(map, unit, target);
            }

            _lastTargetX = (int) target.X;
            _lastTargetY = (int) target.Y;

            if (_path != null && _path.Count > 0)
            {
                //var last = new Vec2Float(_path[0].X, _path[0].Y);
                //foreach (var vector2I in _path)
                //{
                //    debug.Draw(new CustomData.Line(last, new Vec2Float(vector2I.X + 0.5f, vector2I.Y + 0.5f), 0.1F, new ColorFloat(0, 255, 0, 0.6F)));
                //    last = new Vec2Float(vector2I.X + 0.5f, vector2I.Y + 0.5f);
                //}

                var next = _path[_i];
                if (next.X == (int)unit.Position.X && next.Y == (int)unit.Position.Y)
                {
                    _pathLength = 0;

                    if (_i < _path.Count - 1)
                    {
                        _i++;
                        next = _path[_i];
                    }
                    else
                    {
                        DoPath(map, unit, target);
                        if (_path != null && _path.Count > 0)
                        {
                            next = _path[_i];
                            if (next.X == (int) unit.Position.X && next.Y == (int) unit.Position.Y)
                            {
                                if (_i < _path.Count - 1)
                                {
                                    _i++;
                                    next = _path[_i];
                                }
                                else
                                {
                                    return GetOldAction(unit, game, enemy, target, aim, shoot, weaponType);
                                }
                            }
                        }

                    }
                }
                else
                {
                    _pathLength++;
                }

                var jumpData = _jumpProvider.GetJumpForPath(unit, game, next);

                return new UnitAction
                {
                    Velocity = 0.5d >= unit.Position.X - next.X ? 10 : -10,
                    Jump = jumpData.jump,
                    JumpDown = jumpData.jumpDown,
                    Aim = aim,
                    Shoot = unit.Weapon.HasValue && shoot,
                    Reload = false,
                    SwapWeapon = !unit.Weapon.HasValue || unit.Weapon.Value.Typ != weaponType,
                    PlantMine = false
                };
            }
            else
            {
                return GetOldAction(unit, game, enemy, target, aim, shoot, weaponType);
            }

        }

        private void DoPath(Map map, Unit unit, Vec2Double target)
        {
            var pathF = new PathFinderFast(map.MGrid, map);

            _path = pathF.FindPath(new Vector2I((int)unit.Position.X, (int)unit.Position.Y), new Vector2I((int)target.X, (int)target.Y), 1, 2, 5, 10);
            _path?.Reverse();
            _i = 0;
            _pathLength = 0;
        }

        private UnitAction GetOldAction(Unit unit, Game game, Unit enemy, Vec2Double target, Vec2Double aim, bool shoot, WeaponType weaponType)
        {
            var jumpData = _jumpProvider.GetJump(unit, game, target, enemy);

            return new UnitAction
            {
                Velocity = 0.5d >= unit.Position.X - target.X ? 10 : -10,
                Jump = jumpData.jump,
                JumpDown = jumpData.jumpDown,
                Aim = aim,
                Shoot = unit.Weapon.HasValue && shoot,
                Reload = false,
                SwapWeapon = !unit.Weapon.HasValue || unit.Weapon.Value.Typ != weaponType,
                PlantMine = false
            };
        }
    }
}