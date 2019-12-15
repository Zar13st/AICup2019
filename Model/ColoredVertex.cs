namespace AiCup2019.Model
{
    public struct ColoredVertex
    {
        public Vec2Float Position { get; set; }
        public ColorFloat Color { get; set; }
        public ColoredVertex(Vec2Float position, ColorFloat color)
        {
            Position = position;
            Color = color;
        }
        public static ColoredVertex ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new ColoredVertex();
            result.Position = Vec2Float.ReadFrom(reader);
            result.Color = ColorFloat.ReadFrom(reader);
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            Position.WriteTo(writer);
            Color.WriteTo(writer);
        }
    }
}
