namespace AiCup2019.Model
{
    public struct Level
    {
        public Tile[][] Tiles { get; set; }
        public Level(Tile[][] tiles)
        {
            Tiles = tiles;
        }
        public static Level ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new Level();
            result.Tiles = new Tile[reader.ReadInt32()][];
            for (int i = 0; i < result.Tiles.Length; i++)
            {
                result.Tiles[i] = new Tile[reader.ReadInt32()];
                for (int j = 0; j < result.Tiles[i].Length; j++)
                {
                    switch (reader.ReadInt32())
                    {
                    case 0:
                        result.Tiles[i][j] = Tile.Empty;
                        break;
                    case 1:
                        result.Tiles[i][j] = Tile.Wall;
                        break;
                    case 2:
                        result.Tiles[i][j] = Tile.Platform;
                        break;
                    case 3:
                        result.Tiles[i][j] = Tile.Ladder;
                        break;
                    case 4:
                        result.Tiles[i][j] = Tile.JumpPad;
                        break;
                    default:
                        throw new System.Exception("Unexpected discriminant value");
                    }
                }
            }
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(Tiles.Length);
            foreach (var TilesElement in Tiles)
            {
                writer.Write(TilesElement.Length);
                foreach (var TilesElementElement in TilesElement)
                {
                    writer.Write((int) (TilesElementElement));
                }
            }
        }
    }
}
