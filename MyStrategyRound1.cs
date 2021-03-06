using System;
using AiCup2019.Model;
using aicup2019.Pathfinding;
using AiCup2019.Pathfinding;
using aicup2019.Providers;
using AiCup2019.Providers;

namespace AiCup2019
{
    public class MyStrategyRound1
    {
        private readonly EnemyProvider _enemyProvider = new EnemyProvider();
        private readonly TargetProvider _targetProvider = new TargetProvider();
        private readonly ActionProvider _actionProvider = new ActionProvider();
        private readonly JumpPadHelper _jumpPadHelper = new JumpPadHelper();
        private readonly BoomActionProvider _boomActionProvider = new BoomActionProvider();

        private readonly Map _map = new Map();

        public UnitAction GetAction(Unit unit, Game game, Debug debug)
        {
            var enemyUnit = _enemyProvider.GetEnemy(unit, game);
            if (!enemyUnit.HasValue) return new UnitAction();
            var enemy = enemyUnit.Value;

            var (pos, targetType) = _targetProvider.GetTarget(unit, game, enemy);
            var targetPos = _jumpPadHelper.Shift(pos, game);

            _map.SetMap(game, debug, enemy, targetType);

            UnitAction action;
            if (_targetProvider.BoombCount == 0)
            {
                action = _actionProvider.GetAction(unit, game, enemy, debug, targetPos, _map, WeaponType.Pistol);
            }
            else if(!_targetProvider.ShouldBoom)
            {
                action = _actionProvider.GetAction(unit, game, enemy, debug, targetPos, _map, WeaponType.AssaultRifle);
            }
            else 
            {
                if ( Math.Abs(unit.Position.X - enemy.Position.X) < 3 && 
                     ((enemy.Position.Y >= unit.Position.Y && enemy.Position.Y - unit.Position.Y < 2.8) ||
                      (enemy.Position.Y <= unit.Position.Y && unit.Position.Y - enemy.Position.Y  - 1.8 < 2.8)))
                {
                    if (targetType == TargetEnum.EnemyForBigBoom)
                    {
                        action = _boomActionProvider.GetBigBoomAction(unit, game, enemy, debug, targetPos, _map, WeaponType.AssaultRifle);
                    }
                    else if (targetType == TargetEnum.EnemyForBoom)
                    {
                        action = _boomActionProvider.GetBoomAction(unit, game, enemy, debug, targetPos, _map, WeaponType.AssaultRifle);
                    }
                    else
                    {
                        action = _actionProvider.GetAction(unit, game, enemy, debug, targetPos, _map, WeaponType.AssaultRifle);
                    }
                }
                else
                {
                    action = _actionProvider.GetAction(unit, game, enemy, debug, targetPos, _map, WeaponType.AssaultRifle);
                }
            }
            
            //debug.Draw(new CustomData.Log($"X: {unit.Position.X:F1}, Y: {unit.Position.Y:F1}"));
            //debug.Draw(new CustomData.Log($"X: {enemy.Position.X:F1}, Y: {enemy.Position.Y:F1}"));
            //debug.Draw(new CustomData.Line(new Vec2Float((float)unit.Position.X, (float)unit.Position.Y + 1),
            //    new Vec2Float((float)enemy.Position.X, (float)enemy.Position.Y + 1),
            //    0.1F,
            //    action.Shoot ? new ColorFloat(0, 255, 0, 0.6F) : new ColorFloat(255, 0, 0, 0.6F)));

            //action.Velocity = 0;
            //action.Jump = false;
            //action.Shoot = false;
            return action;
        }
    }
}