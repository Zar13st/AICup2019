using AiCup2019.Behaviors;
using AiCup2019.Model;
using AiCup2019.Providers;

namespace AiCup2019
{
    public class MyStrategy
    {
        private readonly EnemyProvider _enemyProvider = new EnemyProvider();
        private readonly HealthProvider _healthProvider = new HealthProvider();

        private readonly FindWeaponBehavior _findWeaponBehavior = new FindWeaponBehavior();
        private readonly FullHpRushBehavior _fullHpRushBehavior = new FullHpRushBehavior();
        private readonly FindHealthBehavior _findHealthBehavior = new FindHealthBehavior();


        public UnitAction GetAction(Unit unit, Game game, Debug debug)
        {
            var enemyUnit = _enemyProvider.GetEnemy(unit, game);
            if (!enemyUnit.HasValue) return new UnitAction();
            var enemy = enemyUnit.Value;

            UnitAction action;
            if (!unit.Weapon.HasValue || unit.Weapon.Value.Typ == WeaponType.RocketLauncher)
            {
                action = _findWeaponBehavior.GetAction(unit, game);
            }
            else if (unit.Health > 80)
            {
                action = _fullHpRushBehavior.GetAction(unit, game, enemy, debug);
            }
            else
            {
                var health = _healthProvider.GetHealth(unit, game);
                if (health.HasValue)
                {
                    action = _findHealthBehavior.GetAction(unit, game, enemy, debug, health.Value);
                }
                else
                {
                    action = _fullHpRushBehavior.GetAction(unit, game, enemy, debug);
                }
            }

            debug.Draw(new CustomData.Log($"X: {unit.Position.X:F1}, Y: {unit.Position.Y:F1}"));
            debug.Draw(new CustomData.Log($"X: {enemy.Position.X:F1}, Y: {enemy.Position.Y:F1}"));
            debug.Draw(new CustomData.Line(new Vec2Float((float)unit.Position.X, (float)unit.Position.Y + 1),
                new Vec2Float((float)enemy.Position.X, (float)enemy.Position.Y + 1),
                0.1F,
                action.Shoot ? new ColorFloat(0, 255, 0, 0.6F) : new ColorFloat(255, 0, 0, 0.6F)));

            return action;
        }
    }
}