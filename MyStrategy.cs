using System;
using AiCup2019.Behaviors;
using AiCup2019.Model;
using AiCup2019.Pathfinding;
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

            var map = new Map(game){ MGrid = new byte[64, 64] }; 
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    if (game.Level.Tiles[x][y] != Tile.Wall && !(game.Level.Tiles[x][y + 1] == Tile.Wall && game.Level.Tiles[x][y - 1] == Tile.Wall))
                    {
                        if (game.Level.Tiles[x][y] != Tile.JumpPad)
                        {
                            map.MGrid[x, y] = 1;
                        }
                    }
                    else
                    {
                        debug.Draw(new CustomData.Rect(new Vec2Float(x, y), new Vec2Float(1, 1), new ColorFloat(255, 0, 0, 0.4F)));
                    }
                }
            }




            UnitAction action;
            var unitHasPistol = unit.Weapon.HasValue && unit.Weapon.Value.Typ == WeaponType.Pistol;
            if (!unitHasPistol)
            {
                action = _findWeaponBehavior.GetAction(unit, game, enemy, debug, map);
            }
            else if (unit.Health > Extensions.HealthForRunToMed)
            {
                action = _fullHpRushBehavior.GetAction(unit, game, enemy, debug);
            }
            else
            {
                var health = _healthProvider.GetHealth(unit, game, enemy);
                if (health.HasValue)
                {
                    action = _findHealthBehavior.GetAction(unit, game, enemy, debug, health.Value);
                }
                else
                {
                    action = _fullHpRushBehavior.GetAction(unit, game, enemy, debug);
                }
            }

            //debug.Draw(new CustomData.Log($"X: {unit.Position.X:F1}, Y: {unit.Position.Y:F1}"));
            //debug.Draw(new CustomData.Log($"X: {enemy.Position.X:F1}, Y: {enemy.Position.Y:F1}"));
            //debug.Draw(new CustomData.Line(new Vec2Float((float)unit.Position.X, (float)unit.Position.Y + 1),
            //    new Vec2Float((float)enemy.Position.X, (float)enemy.Position.Y + 1),
            //    0.1F,
            //    action.Shoot ? new ColorFloat(0, 255, 0, 0.6F) : new ColorFloat(255, 0, 0, 0.6F)));

            action.Shoot = false;
            //action.Velocity = 0;
            //action.Jump = false;
            return action;
        }
    }
}