namespace AiCup2019.Model
{
    public struct LootBox
    {
        public Vec2Double Position { get; set; }
        public Vec2Double Size { get; set; }
        public Item Item { get; set; }
        public LootBox(Vec2Double position, Vec2Double size, Item item)
        {
            Position = position;
            Size = size;
            Item = item;
        }
        public static LootBox ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new LootBox();
            result.Position = Vec2Double.ReadFrom(reader);
            result.Size = Vec2Double.ReadFrom(reader);
            result.Item = Item.ReadFrom(reader);
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            Position.WriteTo(writer);
            Size.WriteTo(writer);
            Item.WriteTo(writer);
        }
    }
}
