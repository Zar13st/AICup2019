using AiCup2019;
using AiCup2019.Model;
using AiCup2019.Pathfinding;
using AiCup2019.Providers;

namespace aicup2019.Providers
{
    public class BoomActionProvider
    {
        public bool Active { get; set; }

        private readonly AimProvider _aimProvider = new AimProvider();
        private readonly ShootProvider _shootProvider = new ShootProvider();

        private int _state = 0;

        public UnitAction GetBoomAction(Unit unit, Game game, Unit enemy, Debug debug, Vec2Double target, Map map, WeaponType weaponType)
        {
            if (map.IsGround((int)unit.Position.X, (int)unit.Position.Y) && enemy.Health <= 50)
            {
                Active = true;

                return new UnitAction
                {
                    Velocity = 0,
                    Jump = false,
                    JumpDown = false,
                    Aim = new Vec2Double(0, -1),
                    Shoot = true,
                    Reload = false,
                    SwapWeapon = false,
                    PlantMine = true,
                };
            }
            else
            {


                return new UnitAction
                {
                    Velocity = 0.5d >= unit.Position.X - target.X ? 10 : -10,
                    Jump = false,
                    JumpDown = false,
                    Aim = _aimProvider.GetAim(unit, enemy),
                    Shoot = _shootProvider.IsTargetInSight(unit, enemy, game, debug),
                    Reload = false,
                    SwapWeapon = false,
                    PlantMine = false,
                };
            }
        }

        public UnitAction GetBigBoomAction(Unit unit, Game game, Unit enemy, Debug debug, Vec2Double target, Map map, WeaponType weaponType)
        {
            if (map.IsGround((int) unit.Position.X, (int) unit.Position.Y))
            {
                Active = true;

                if (_state == 0)
                {
                    _state++;
                    return new UnitAction
                    {
                        Velocity = 0,
                        Jump = false,
                        JumpDown = false,
                        Aim = new Vec2Double(0, -1),
                        Shoot = false,
                        Reload = false,
                        SwapWeapon = false,
                        PlantMine = true,
                    };
                }
                else
                {
                    return new UnitAction
                    {
                        Velocity = 0,
                        Jump = false,
                        JumpDown = false,
                        Aim = new Vec2Double(0, -1),
                        Shoot = true,
                        Reload = false,
                        SwapWeapon = false,
                        PlantMine = true,
                    };
                }
            }
            else
            {
                return new UnitAction
                {
                    Velocity = 0.5d >= unit.Position.X - target.X ? 10 : -10,
                    Jump = false,
                    JumpDown = false,
                    Aim = new Vec2Double(0, -1),
                    Shoot = false,
                    Reload = false,
                    SwapWeapon = false,
                    PlantMine = true,
                };
            }
        }
    }
}