using AiCup2019.Model;
using AiCup2019.Pathfinding;
using aicup2019.Providers;
using AiCup2019.Providers;

namespace AiCup2019
{
    public class MyStrategy
    {
        private readonly EnemyProvider _enemyProvider = new EnemyProvider();
        private readonly TargetProvider _targetProvider = new TargetProvider();
        private readonly ActionProvider _actionProvider = new ActionProvider();

        private readonly Map _map = new Map();

        public UnitAction GetAction(Unit unit, Game game, Debug debug)
        {
            var enemyUnit = _enemyProvider.GetEnemy(unit, game);
            if (!enemyUnit.HasValue) return new UnitAction();
            var enemy = enemyUnit.Value;

            _map.SetMap(game, debug, enemy);

            var target = _targetProvider.GetTarget(unit, game, enemy);

            var action = _actionProvider.GetAction(unit, game, enemy, debug, target, _map);
            
            //debug.Draw(new CustomData.Log($"X: {unit.Position.X:F1}, Y: {unit.Position.Y:F1}"));
            //debug.Draw(new CustomData.Log($"X: {enemy.Position.X:F1}, Y: {enemy.Position.Y:F1}"));
            //debug.Draw(new CustomData.Line(new Vec2Float((float)unit.Position.X, (float)unit.Position.Y + 1),
            //    new Vec2Float((float)enemy.Position.X, (float)enemy.Position.Y + 1),
            //    0.1F,
            //    action.Shoot ? new ColorFloat(0, 255, 0, 0.6F) : new ColorFloat(255, 0, 0, 0.6F)));

            //action.Velocity = 0;
            //action.Jump = false;
            action.Shoot = false;
            return action;
        }
    }
}