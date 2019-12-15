namespace AiCup2019.Model
{
    public struct ServerMessageGame
    {
        public PlayerView? PlayerView { get; set; }
        public ServerMessageGame(PlayerView? playerView)
        {
            PlayerView = playerView;
        }
        public static ServerMessageGame ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new ServerMessageGame();
            if (reader.ReadBoolean())
            {
                result.PlayerView = Model.PlayerView.ReadFrom(reader);
            } else
            {
                result.PlayerView = null;
            }
            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (!PlayerView.HasValue)
            {
                writer.Write(false);
            } else
            {
                writer.Write(true);
                PlayerView.Value.WriteTo(writer);
            }
        }
    }
}
