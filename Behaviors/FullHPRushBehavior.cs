using AiCup2019.Model;
using AiCup2019.Providers;

namespace AiCup2019.Behaviors
{
    public class FullHpRushBehavior
    {
        private readonly JumpProvider _jumpProvider = new JumpProvider();
        private readonly AimProvider _aimProvider = new AimProvider();
        private readonly ShootProvider _shootProvider = new ShootProvider();

        public UnitAction GetAction(Unit unit, Game game, Unit enemy, Debug debug)
        {
            var jumpData = _jumpProvider.GetJump(unit, game, enemy.Position);

            var aim = _aimProvider.GetAim(unit, enemy);

            var shoot = _shootProvider.IsTargetInSight(unit, enemy, game, debug);

            return new UnitAction
            {
                Velocity = (enemy.Position.X - unit.Position.X) * 1000,
                Jump = jumpData.jump,
                JumpDown = jumpData.jumpDown,
                Aim = aim,
                Shoot = shoot,
                Reload = false,
                SwapWeapon = false,
                PlantMine = false
            };
        }
    }
}