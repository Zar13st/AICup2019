using AiCup2019.Model;

namespace AiCup2019.Providers
{
    public class ShootProvider
    {
        public bool GetShoot(Unit player)
        {
            if (!player.Weapon.HasValue || player.Weapon?.Typ == WeaponType.RocketLauncher) return false;

            return true;
        }
    }
}