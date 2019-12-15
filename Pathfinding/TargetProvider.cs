using System.Collections.Generic;
using System.Linq;
using AiCup2019.Model;
using aicup2019.Pathfinding;
using AiCup2019.Providers;

namespace AiCup2019.Pathfinding
{
    public class TargetProvider
    {
        public int BoombCount { get; private set; }
        public bool ShouldBoom { get; private set; }
        private bool _leftSide;
        private bool _mineFull;
        private Dictionary<int, Vec2Double> _targets = new Dictionary<int, Vec2Double>();

        private readonly HealthProvider _healthProvider = new HealthProvider();

        public (Vec2Double targetPos, TargetEnum targetType) GetTarget(Unit unit, Game game, Unit enemy)
        {
            if (game.CurrentTick == 0)
            {
                foreach (var lootBox in game.LootBoxes)
                {
                    if (!(lootBox.Item is Item.Mine weapon)) continue;

                    BoombCount++;
                    _targets[(int)lootBox.Position.X] = lootBox.Position;
                }

                if (BoombCount != 0) Extensions.HealthForRunToMed = 65;

                if (unit.Position.X <= 20) _leftSide = true;
            }

            ShouldBoom = game.Players.First(x => x.Id == unit.PlayerId).Score + enemy.Health >
                         game.Players.First(x => x.Id == enemy.PlayerId).Score;

            if (BoombCount == 0 || !ShouldBoom)
            {
                return GetOldTarget(unit, game, enemy);
            }
            else if (BoombCount == 2)
            {
                return GetBoomTarget(unit, game, enemy);
            }
            else
            {
                return GetBigBoomTarget(unit, game, enemy);
            }
        }

        private (Vec2Double targetPos, TargetEnum targetType) GetBigBoomTarget(Unit unit, Game game, Unit enemy)
        {
            var unitHasPistol = unit.Weapon.HasValue && unit.Weapon.Value.Typ == WeaponType.AssaultRifle;
            if (!unitHasPistol)
            {
                var nearestWeapon = GetWeapon(unit, game, WeaponType.AssaultRifle);

                if (_leftSide)
                {
                    for (int x = (int)unit.Position.X; x <= (int)nearestWeapon.Position.X; x++)
                    {
                        if (_targets.ContainsKey(x))
                        {
                            var mine = _targets[x];
                            _targets.Remove(x);
                            return (mine, TargetEnum.Boomb);
                        }
                    }
                }
                else
                {
                    for (int x = (int)unit.Position.X; x >= (int)nearestWeapon.Position.X; x--)
                    {
                        if (_targets.ContainsKey(x))
                        {
                            var mine = _targets[x];
                            _targets.Remove(x);
                            return (mine, TargetEnum.Boomb);
                        }
                    }
                }

                return (nearestWeapon.Position, TargetEnum.Weapon);

            }
            else if (unit.Mines >= 2 || _mineFull)
            {
                _mineFull = true;
                return (enemy.Position, TargetEnum.EnemyForBigBoom);
            }
            else if (unit.Mines < 2 && GetMine(unit, game).HasValue)
            {
                var mine = GetMine(unit, game);
                return (mine.Value.Position, TargetEnum.Boomb);

            }
            else
            {
                return (enemy.Position, TargetEnum.EnemyForBigBoom);
            }
        }

        private (Vec2Double targetPos, TargetEnum targetType) GetBoomTarget(Unit unit, Game game, Unit enemy)
        {
            var unitHasPistol = unit.Weapon.HasValue && unit.Weapon.Value.Typ == WeaponType.AssaultRifle;
            if (!unitHasPistol)
            {
                var nearestWeapon = GetWeapon(unit, game, WeaponType.AssaultRifle);

                if (_leftSide)
                {
                    for (int x = (int)unit.Position.X; x <= (int)nearestWeapon.Position.X; x++)
                    {
                        if (_targets.ContainsKey(x))
                        {
                            var mine = _targets[x];
                            _targets.Remove(x);
                            return (mine, TargetEnum.Boomb);
                        }
                    }
                }
                else
                {
                    for (int x = (int)unit.Position.X; x >= (int)nearestWeapon.Position.X; x--)
                    {
                        if (_targets.ContainsKey(x))
                        {
                            var mine = _targets[x];
                            _targets.Remove(x);
                            return (mine, TargetEnum.Boomb);
                        }
                    }
                }

                return (nearestWeapon.Position, TargetEnum.Weapon);

            }
            else if (unit.Mines >= 2 || _mineFull)
            {
                _mineFull = true;
                return (enemy.Position, TargetEnum.EnemyForBigBoom);
            }
            else if (GetMine(unit, game).HasValue)
            {
                var mine = GetMine(unit, game);
                return (mine.Value.Position, TargetEnum.Boomb);
            }
            else if (enemy.Health <= 50)
            {
                return (enemy.Position, TargetEnum.EnemyForBoom);
            }
            else
            {
                return (enemy.Position, TargetEnum.Enemy);
            }
        }

        private (Vec2Double targetPos, TargetEnum targetType) GetOldTarget(Unit unit, Game game, Unit enemy)
        {
            var unitHasPistol = unit.Weapon.HasValue && unit.Weapon.Value.Typ == WeaponType.Pistol;
            if (!unitHasPistol)
            {
                var nearestWeapon = GetWeapon(unit, game, WeaponType.Pistol);
                return (nearestWeapon.Position, TargetEnum.Weapon);

            }
            else if (unit.Health > Extensions.HealthForRunToMed)
            {
                return (enemy.Position, TargetEnum.Enemy);
            }
            else
            {
                var health = _healthProvider.GetHealth(unit, game, enemy);
                if (health.HasValue)
                {
                    return (health.Value.Position, TargetEnum.Health);
                }
                else
                {
                    return (enemy.Position, TargetEnum.Enemy);
                }
            }
        }

        private LootBox GetWeapon(Unit unit, Game game, WeaponType weaponType)
        {
            LootBox? nearestWeapon = null;
            foreach (var lootBox in game.LootBoxes)
            {
                if (!(lootBox.Item is Item.Weapon weapon)) continue;

                if (weapon.WeaponType != weaponType) continue;

                if (!nearestWeapon.HasValue || unit.Position.DistanceSqr(lootBox.Position) < unit.Position.DistanceSqr(nearestWeapon.Value.Position))
                {
                    nearestWeapon = lootBox;
                }
            }

            if (!nearestWeapon.HasValue) return game.LootBoxes.FirstOrDefault();

            return nearestWeapon.Value;
        }

        private LootBox? GetMine(Unit unit, Game game)
        {
            LootBox? nearestMine = null;
            foreach (var lootBox in game.LootBoxes)
            {
                if (!(lootBox.Item is Item.Mine weapon)) continue;

                if (!nearestMine.HasValue || unit.Position.DistanceSqr(lootBox.Position) < unit.Position.DistanceSqr(nearestMine.Value.Position))
                {
                    nearestMine = lootBox;
                }
            }

            return nearestMine;
        }
    }
}