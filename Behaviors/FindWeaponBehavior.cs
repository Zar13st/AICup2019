using AiCup2019.Model;
using AiCup2019.Providers;

namespace AiCup2019.Behaviors
{
    public class FindWeaponBehavior
    {
        private readonly JumpProvider _jumpProvider = new JumpProvider();
        private readonly AimProvider _aimProvider = new AimProvider();
        private readonly ShootProvider _shootProvider = new ShootProvider();

        public UnitAction GetAction(Unit unit, Game game, Unit enemy, Debug debug)
        {
            var nearestWeapon = GetWeapon(unit, game);

            var jumpData = _jumpProvider.GetJump(unit, game, nearestWeapon.Value.Position, enemy);

            var aim = _aimProvider.GetAim(unit, enemy);

            var shoot = _shootProvider.IsTargetInSight(unit, enemy, game, debug);

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