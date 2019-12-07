using AiCup2019.Model;

namespace AiCup2019
{
    public static class Extensions
    {
        public static int HealthForRunToMed = 80;

        public static double DistanceSqr(this Vec2Double a, Vec2Double b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }
    }
}