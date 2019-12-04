using AiCup2019.Model;
using AiCup2019.Providers;

namespace AiCup2019.Behaviors
{
    public class FindWeaponBehavior
    {
        private readonly JumpProvider _jumpProvider = new JumpProvider();

        public UnitAction GetAction(Unit unit, Game game, Unit enemy)
        {
            var nearestWeapon = GetWeapon(unit, game);

            var jumpData = _jumpProvider.GetJump(unit, game, nearestWeapon.Value.Position, enemy);

            return new UnitAction
            {
                Velocity = (nearestWeapon.Value.Position.X - unit.Position.X) * 1000,
                Jump = jumpData.jump,
                JumpDown = jumpData.jumpDown,
                Aim = new Vec2Double(0, 0),
                Shoot = false,
                Reload = false,
                SwapWeapon = !unit.Weapon.HasValue || unit.Weapon?.Typ == WeaponType.RocketLauncher,
                PlantMine = false
            };
        }

        private LootBox? GetWeapon(Unit unit, Game game)
        {
            LootBox? nearestWeapon = null;
            foreach (var lootBox in game.LootBoxes)
            {
                if (!(lootBox.Item is Item.Weapon weapon)) continue;

                if (weapon.WeaponType == WeaponType.RocketLauncher) continue;

                if (!nearestWeapon.HasValue || unit.Position.DistanceSqr(lootBox.Position) < unit.Position.DistanceSqr(nearestWeapon.Value.Position))
                {
                    nearestWeapon = lootBox;
                }
            }

            return nearestWeapon;
        }
    }
}