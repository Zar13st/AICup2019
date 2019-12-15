namespace AiCup2019.Model
{
    public struct Properties
    {
        public int MaxTickCount { get; set; }
        public int TeamSize { get; set; }
        public double TicksPerSecond { get; set; }
        public int UpdatesPerTick { get; set; }
        public Vec2Double LootBoxSize { get; set; }
        public Vec2Double UnitSize { get; set; }
        public double UnitMaxHorizontalSpeed { get; set; }
        public double UnitFallSpeed { get; set; }
        public double UnitJumpTime { get; set; }
        public double UnitJumpSpeed { get; set; }
        public double JumpPadJumpTime { get; set; }
        public double JumpPadJumpSpeed { get; set; }
        public int UnitMaxHealth { get; set; }
        public int HealthPackHealth { get; set; }
        public System.Collections.Generic.IDictionary<WeaponType, WeaponParameters> WeaponParameters { get; set; }
        public Vec2Double MineSize { get; set; }
        public ExplosionParameters MineExplosionParameters { get; set; }
        public double MinePrepareTime { get; set; }
        public double MineTriggerTime { get; set; }
        public double MineTriggerRadius { get; set; }
        public int KillScore { get; set; }
        public Properties(int maxTickCount, int teamSize, double ticksPerSecond, int updatesPerTick, Vec2Double lootBoxSize, Vec2Double unitSize, double unitMaxHorizontalSpeed, double unitFallSpeed, double unitJumpTime, double unitJumpSpeed, double jumpPadJumpTime, double jumpPadJumpSpeed, int unitMaxHealth, int healthPackHealth, System.Collections.Generic.IDictionary<WeaponType, WeaponParameters> weaponParameters, Vec2Double mineSize, ExplosionParameters mineExplosionParameters, double minePrepareTime, double mineTriggerTime, double mineTriggerRadius, int killScore)
        {
            MaxTickCount = maxTickCount;
            TeamSize = teamSize;
            TicksPerSecond = ticksPerSecond;
            UpdatesPerTick = updatesPerTick;
            LootBoxSize = lootBoxSize;
            UnitSize = unitSize;
            UnitMaxHorizontalSpeed = unitMaxHorizontalSpeed;
            UnitFallSpeed = unitFallSpeed;
            UnitJumpTime = unitJumpTime;
            UnitJumpSpeed = unitJumpSpeed;
            JumpPadJumpTime = jumpPadJumpTime;
            JumpPadJumpSpeed = jumpPadJumpSpeed;
            UnitMaxHealth = unitMaxHealth;
            HealthPackHealth = healthPackHealth;
            WeaponParameters = weaponParameters;
            MineSize = mineSize;
            MineExplosionParameters = mineExplosionParameters;
            MinePrepareTime = minePrepareTime;
            MineTriggerTime = mineTriggerTime;
            MineTriggerRadius = mineTriggerRadius;
            KillScore = killScore;
        }
        public static Properties ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new Properties();
            result.MaxTickCount = reader.ReadInt32();
            result.TeamSize = reader.ReadInt32();
            result.TicksPerSecond = reader.ReadDouble();
            result.UpdatesPerTick = reader.ReadInt32();
            result.LootBoxSize = Vec2Double.ReadFrom(reader);
            result.UnitSize = Vec2Double.ReadFrom(reader);
            result.UnitMaxHorizontalSpeed = reader.ReadDouble();
            result.UnitFallSpeed = reader.ReadDouble();
            result.UnitJumpTime = reader.ReadDouble();
            result.UnitJumpSpeed = reader.ReadDouble();
            result.JumpPadJumpTime = reader.ReadDouble();
            result.JumpPadJumpSpeed = reader.ReadDouble();
            result.UnitMaxHealth = reader.ReadInt32();
            result.HealthPackHealth = reader.ReadInt32();
            int WeaponParametersSize = reader.ReadInt32();
            result.WeaponParameters = new System.Collections.Generic.Dictionary<WeaponType, WeaponParameters>(WeaponParametersSize);
            for (int i = 0; i < WeaponParametersSize; i++)
            {
                WeaponType WeaponParametersKey;
                switch (reader.ReadInt32())
                {
                case 0:
                    WeaponParametersKey = WeaponType.Pistol;
                    break;
                case 1:
                    WeaponParametersKey = WeaponType.AssaultRifle;
                    break;
                case 2:
                    WeaponParametersKey = WeaponType.RocketLauncher;
                    break;
                default:
                    throw new System.Exception("Unexpected discriminant value");
                }
                WeaponParameters WeaponParametersValue;
                WeaponParametersValue = Model.WeaponParameters.ReadFrom(reader);
                result.WeaponParameters.Add(WeaponParametersKey, WeaponParametersValue);
            }
            result.MineSize = Vec2Double.ReadFrom(reader);
            result.MineExplosionParameters = ExplosionParameters.ReadFrom(reader);
            result.MinePrepareTime = reader.ReadDouble();
            result.MineTriggerTime = reader.ReadDouble();
            result.MineTriggerRadius = reader.ReadDouble();
            result.KillScore = reader.ReadInt32();
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(MaxTickCount);
            writer.Write(TeamSize);
            writer.Write(TicksPerSecond);
            writer.Write(UpdatesPerTick);
            LootBoxSize.WriteTo(writer);
            UnitSize.WriteTo(writer);
            writer.Write(UnitMaxHorizontalSpeed);
            writer.Write(UnitFallSpeed);
            writer.Write(UnitJumpTime);
            writer.Write(UnitJumpSpeed);
            writer.Write(JumpPadJumpTime);
            writer.Write(JumpPadJumpSpeed);
            writer.Write(UnitMaxHealth);
            writer.Write(HealthPackHealth);
            writer.Write(WeaponParameters.Count);
            foreach (var WeaponParametersEntry in WeaponParameters)
            {
                var WeaponParametersKey = WeaponParametersEntry.Key;
                var WeaponParametersValue = WeaponParametersEntry.Value;
                writer.Write((int) (WeaponParametersKey));
                WeaponParametersValue.WriteTo(writer);
            }
            MineSize.WriteTo(writer);
            MineExplosionParameters.WriteTo(writer);
            writer.Write(MinePrepareTime);
            writer.Write(MineTriggerTime);
            writer.Write(MineTriggerRadius);
            writer.Write(KillScore);
        }
    }
}
