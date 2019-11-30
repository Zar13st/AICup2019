using System.Linq;
using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class MovementTargetProvider
    {
        public (Vec2Double targetPos, Unit? nearestEnemy) GetTarget(Unit unit, Game game)
        {
            Unit? nearestEnemy = null;
            foreach (var other in game.Units)
            {
                if (other.PlayerId != unit.PlayerId)
                {
                    if (!nearestEnemy.HasValue || unit.Position.DistanceSqr(other.Position) < unit.Position.DistanceSqr(nearestEnemy.Value.Position))
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

                    if (!nearestWeapon.HasValue || unit.Position.DistanceSqr(lootBox.Position) < unit.Position.DistanceSqr(nearestWeapon.Value.Position))
                    {
                        nearestWeapon = lootBox;
                    }
                }

                if (lootBox.Item is Item.HealthPack)
                {
                    if (!nearestHealthPack.HasValue || unit.Position.DistanceSqr(lootBox.Position) < unit.Position.DistanceSqr(nearestHealthPack.Value.Position))
                    {
                        nearestHealthPack = lootBox;
                    }
                }
            }

            var inMid = !(unit.Position.X > 19.5 && unit.Position.X < 20.5);

            var targetPos = unit.Position;
            if ((!unit.Weapon.HasValue || unit.Weapon?.Typ == WeaponType.RocketLauncher) && nearestWeapon.HasValue)
            {
                targetPos = nearestWeapon.Value.Position;
            }
            else if (nearestEnemy.HasValue && unit.Health > 80 && inMid)
            {
                targetPos = nearestEnemy.Value.Position;
            }
            else if (nearestHealthPack.HasValue)
            {
                targetPos = nearestHealthPack.Value.Position;
            }
            else if (nearestEnemy.HasValue && game.Players.FirstOrDefault(x => x.Id == unit.PlayerId).Score <= game.Players.FirstOrDefault(x => x.Id != unit.PlayerId).Score && inMid)
            {
                targetPos = nearestEnemy.Value.Position;
            }

            return (targetPos, nearestEnemy);
        }
    }
}