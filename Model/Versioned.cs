namespace AiCup2019.Model
{
    public struct Versioned
    {
        public System.Collections.Generic.IDictionary<int, UnitAction> Inner { get; set; }
        public Versioned(System.Collections.Generic.IDictionary<int, UnitAction> inner)
        {
            Inner = inner;
        }
        public static Versioned ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new Versioned();
            int InnerSize = reader.ReadInt32();
            result.Inner = new System.Collections.Generic.Dictionary<int, UnitAction>(InnerSize);
            for (int i = 0; i < InnerSize; i++)
            {
                int InnerKey;
                InnerKey = reader.ReadInt32();
                UnitAction InnerValue;
                InnerValue = UnitAction.ReadFrom(reader);
                result.Inner.Add(InnerKey, InnerValue);
            }
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(43981);
            writer.Write(Inner.Count);
            foreach (var InnerEntry in Inner)
            {
                var InnerKey = InnerEntry.Key;
                var InnerValue = InnerEntry.Value;
                writer.Write(InnerKey);
                InnerValue.WriteTo(writer);
            }
        }
    }
}
