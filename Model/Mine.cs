namespace AiCup2019.Model
{
    public struct Mine
    {
        public int PlayerId { get; set; }
        public Vec2Double Position { get; set; }
        public Vec2Double Size { get; set; }
        public MineState State { get; set; }
        public double? Timer { get; set; }
        public double TriggerRadius { get; set; }
        public ExplosionParameters ExplosionParameters { get; set; }
        public Mine(int playerId, Vec2Double position, Vec2Double size, MineState state, double? timer, double triggerRadius, ExplosionParameters explosionParameters)
        {
            PlayerId = playerId;
            Position = position;
            Size = size;
            State = state;
            Timer = timer;
            TriggerRadius = triggerRadius;
            ExplosionParameters = explosionParameters;
        }
        public static Mine ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new Mine();
            result.PlayerId = reader.ReadInt32();
            result.Position = Vec2Double.ReadFrom(reader);
            result.Size = Vec2Double.ReadFrom(reader);
            switch (reader.ReadInt32())
            {
            case 0:
                result.State = MineState.Preparing;
                break;
            case 1:
                result.State = MineState.Idle;
                break;
            case 2:
                result.State = MineState.Triggered;
                break;
            case 3:
                result.State = MineState.Exploded;
                break;
            default:
                throw new System.Exception("Unexpected discriminant value");
            }
            if (reader.ReadBoolean())
            {
                result.Timer = reader.ReadDouble();
            } else
            {
                result.Timer = null;
            }
            result.TriggerRadius = reader.ReadDouble();
            result.ExplosionParameters = ExplosionParameters.ReadFrom(reader);
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(PlayerId);
            Position.WriteTo(writer);
            Size.WriteTo(writer);
            writer.Write((int) (State));
            if (!Timer.HasValue)
            {
                writer.Write(false);
            } else
            {
                writer.Write(true);
                writer.Write(Timer.Value);
            }
            writer.Write(TriggerRadius);
            ExplosionParameters.WriteTo(writer);
        }
    }
}
