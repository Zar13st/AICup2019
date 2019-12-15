namespace AiCup2019.Model
{
    public struct Bullet
    {
        public WeaponType WeaponType { get; set; }
        public int UnitId { get; set; }
        public int PlayerId { get; set; }
        public Vec2Double Position { get; set; }
        public Vec2Double Velocity { get; set; }
        public int Damage { get; set; }
        public double Size { get; set; }
        public ExplosionParameters? ExplosionParameters { get; set; }
        public Bullet(WeaponType weaponType, int unitId, int playerId, Vec2Double position, Vec2Double velocity, int damage, double size, ExplosionParameters? explosionParameters)
        {
            WeaponType = weaponType;
            UnitId = unitId;
            PlayerId = playerId;
            Position = position;
            Velocity = velocity;
            Damage = damage;
            Size = size;
            ExplosionParameters = explosionParameters;
        }
        public static Bullet ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new Bullet();
            switch (reader.ReadInt32())
            {
            case 0:
                result.WeaponType = WeaponType.Pistol;
                break;
            case 1:
                result.WeaponType = WeaponType.AssaultRifle;
                break;
            case 2:
                result.WeaponType = WeaponType.RocketLauncher;
                break;
            default:
                throw new System.Exception("Unexpected discriminant value");
            }
            result.UnitId = reader.ReadInt32();
            result.PlayerId = reader.ReadInt32();
            result.Position = Vec2Double.ReadFrom(reader);
            result.Velocity = Vec2Double.ReadFrom(reader);
            result.Damage = reader.ReadInt32();
            result.Size = reader.ReadDouble();
            if (reader.ReadBoolean())
            {
                result.ExplosionParameters = Model.ExplosionParameters.ReadFrom(reader);
            } else
            {
                result.ExplosionParameters = null;
            }
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write((int) (WeaponType));
            writer.Write(UnitId);
            writer.Write(PlayerId);
            Position.WriteTo(writer);
            Velocity.WriteTo(writer);
            writer.Write(Damage);
            writer.Write(Size);
            if (!ExplosionParameters.HasValue)
            {
                writer.Write(false);
            } else
            {
                writer.Write(true);
                ExplosionParameters.Value.WriteTo(writer);
            }
        }
    }
}
