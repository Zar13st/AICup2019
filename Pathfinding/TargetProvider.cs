using AiCup2019.Model;
using AiCup2019.Providers;

namespace AiCup2019.Pathfinding
{
    public class TargetProvider
    {
        private readonly HealthProvider _healthProvider = new HealthProvider();

        public Vec2Double GetTarget(Unit unit, Game game, Unit enemy)
        {
            var unitHasPistol = unit.Weapon.HasValue && unit.Weapon.Value.Typ == WeaponType.Pistol;
            if (!unitHasPistol)
            {
                var nearestWeapon = GetWeapon(unit, game);
                return nearestWeapon.Position;

            }
            else if (unit.Health > Extensions.HealthForRunToMed)
            {
                return enemy.Position;
            }
            else
            {
                var health = _healthProvider.GetHealth(unit, game, enemy);
                if (health.HasValue)
                {
                    return health.Value.Position;
                }
                else
                {
                    return enemy.Position;
                }
            }
        }

        private LootBox GetWeapon(Unit unit, Game game)
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

            return nearestWeapon.Value;
        }
    }
}