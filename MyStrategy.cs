using AiCup2019.Model;
using AiCup2019.Providers;

namespace AiCup2019
{
    public class MyStrategy
    {
        private readonly AimProvider _aimProvider = new AimProvider();
        private readonly MovementTargetProvider _movementTargetProvider = new MovementTargetProvider();
        private readonly JumpProvider _jumpProvider = new JumpProvider();
        private readonly ShootProvider _shootProvider = new ShootProvider();

        public UnitAction GetAction(Unit unit, Game game, Debug debug)
        {
            var (targetPos, nearestEnemy) = _movementTargetProvider.GetTarget(unit, game);
            if (nearestEnemy == null) return new UnitAction();
            var enemy = (Unit)nearestEnemy;

            debug.Draw(new CustomData.Log($"X: {unit.Position.X:F1}, Y: {unit.Position.Y:F1}"));


            var aim = _aimProvider.GetAim(unit, enemy);

            var jump = _jumpProvider.GetJump(unit, game, targetPos);

            var shoot = _shootProvider.GetShoot(unit, enemy, game);

            var action = new UnitAction
            {
                Velocity = (targetPos.X - unit.Position.X) * 1000,
                Jump = jump,
                JumpDown = !jump,
                Aim = aim,
                Shoot = shoot,
                SwapWeapon = !unit.Weapon.HasValue || unit.Weapon?.Typ == WeaponType.RocketLauncher,
                PlantMine = false
            };

            return action;
        }
    }
}