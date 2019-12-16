using System.Collections.Generic;
using AiCup2019.Model;

namespace AiCup2019.Sim
{
    public class SimProcessor
    {
        private int _tickDepth = 18;

        private Dictionary<DirectionType, int> _pointByDir = new Dictionary<DirectionType, int>();

        private Dictionary<int, List<Bullet>> _bulletListByTick = new Dictionary<int, List<Bullet>>();

        public UnitAction GetAction(Game game, Unit player, UnitAction oldAction)
        {
            _pointByDir.Clear();
            if (game.Bullets.Length == 0) return oldAction;



            return oldAction;
        }

        private void CalculateBulletPositions()
        {

        }
    }
}