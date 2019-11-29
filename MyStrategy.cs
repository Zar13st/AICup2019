using AiCup2019.Model;
using AiCup2019.Providers;

namespace AiCup2019
{
    public class MyStrategy
    {
        private readonly AimProvider _aimProvider = new AimProvider();
        private readonly MovementTargetProvider _movementTargetProvider = new MovementTargetProvider();
        private readonly JumpProvider _jumpProvider = new JumpProvider();

        public UnitAction GetAction(Unit unit, Game game, Debug debug)
        {
            var (targetPos, nearestEnemy) = _movementTargetProvider.GetTarget(unit, game);

            debug.Draw(new CustomData.Log("Target pos: " + targetPos));

            var aim = _aimProvider.GetAim(unit, nearestEnemy);

            var jump = _jumpProvider.GetJump(game, unit, targetPos);

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