namespace AiCup2019.Model
{
    public abstract class PlayerMessageGame
    {
        public abstract void WriteTo(System.IO.BinaryWriter writer);
        public static PlayerMessageGame ReadFrom(System.IO.BinaryReader reader)
        {
            switch (reader.ReadInt32())
            {
                case CustomDataMessage.TAG:
                    return CustomDataMessage.ReadFrom(reader);
                case ActionMessage.TAG:
                    return ActionMessage.ReadFrom(reader);
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }
        }

        public class CustomDataMessage : PlayerMessageGame
        {
            public const int TAG = 0;
            public CustomData Data { get; set; }
            public CustomDataMessage() {}
            public CustomDataMessage(CustomData data)
            {
                Data = data;
            }
            public static new CustomDataMessage ReadFrom(System.IO.BinaryReader reader)
            {
                var result = new CustomDataMessage();
                result.Data = CustomData.ReadFrom(reader);
                return result;
            }
            public override void WriteTo(System.IO.BinaryWriter writer)
            {
                writer.Write(TAG);
                Data.WriteTo(writer);
            }
        }

        public class ActionMessage : PlayerMessageGame
        {
            public const int TAG = 1;
            public Versioned Action { get; set; }
            public ActionMessage() {}
            public ActionMessage(Versioned action)
            {
                Action = action;
            }
            public static new ActionMessage ReadFrom(System.IO.BinaryReader reader)
            {
                var result = new ActionMessage();
                result.Action = Versioned.ReadFrom(reader);
                return result;
            }
            public override void WriteTo(System.IO.BinaryWriter writer)
            {
                writer.Write(TAG);
                Action.WriteTo(writer);
            }
        }
    }
}
