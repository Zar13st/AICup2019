namespace AiCup2019.Model
{
    public struct Unit
    {
        public int PlayerId { get; set; }
        public int Id { get; set; }
        public int Health { get; set; }
        public Vec2Double Position { get; set; }
        public Vec2Double Size { get; set; }
        public JumpState JumpState { get; set; }
        public bool WalkedRight { get; set; }
        public bool Stand { get; set; }
        public bool OnGround { get; set; }
        public bool OnLadder { get; set; }
        public int Mines { get; set; }
        public Weapon? Weapon { get; set; }
        public Unit(int playerId, int id, int health, Vec2Double position, Vec2Double size, JumpState jumpState, bool walkedRight, bool stand, bool onGround, bool onLadder, int mines, Weapon? weapon)
        {
            PlayerId = playerId;
            Id = id;
            Health = health;
            Position = position;
            Size = size;
            JumpState = jumpState;
            WalkedRight = walkedRight;
            Stand = stand;
            OnGround = onGround;
            OnLadder = onLadder;
            Mines = mines;
            Weapon = weapon;
        }
        public static Unit ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new Unit();
            result.PlayerId = reader.ReadInt32();
            result.Id = reader.ReadInt32();
            result.Health = reader.ReadInt32();
            result.Position = Vec2Double.ReadFrom(reader);
            result.Size = Vec2Double.ReadFrom(reader);
            result.JumpState = JumpState.ReadFrom(reader);
            result.WalkedRight = reader.ReadBoolean();
            result.Stand = reader.ReadBoolean();
            result.OnGround = reader.ReadBoolean();
            result.OnLadder = reader.ReadBoolean();
            result.Mines = reader.ReadInt32();
            if (reader.ReadBoolean())
            {
                result.Weapon = Model.Weapon.ReadFrom(reader);
            } else
            {
                result.Weapon = null;
            }
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(PlayerId);
            writer.Write(Id);
            writer.Write(Health);
            Position.WriteTo(writer);
            Size.WriteTo(writer);
            JumpState.WriteTo(writer);
            writer.Write(WalkedRight);
            writer.Write(Stand);
            writer.Write(OnGround);
            writer.Write(OnLadder);
            writer.Write(Mines);
            if (!Weapon.HasValue)
            {
                writer.Write(false);
            } else
            {
                writer.Write(true);
                Weapon.Value.WriteTo(writer);
            }
        }
    }
}
