using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Net.Sockets;
using AiCup2019.Model;

namespace AiCup2019
{
    public class Runner
    {
        private BinaryReader reader;
        private BinaryWriter writer;

        private int _firstPlayerId;
        private int _secondPlayerId;

        private bool _isTwo;

        private int BoombCount { get; set; }

        public Runner(string host, int port, string token)
        {
            var client = new TcpClient(host, port) { NoDelay = true };
            var stream = new BufferedStream(client.GetStream());
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            var tokenData = System.Text.Encoding.UTF8.GetBytes(token);
            writer.Write(tokenData.Length);
            writer.Write(tokenData);
            writer.Flush();
        }
        public void Run()
        {
            var myStrategyR1 = new MyStrategyRound1();
            var myStrategyR2First = new MyStrategyRound2First();
            var myStrategyR2Rocket = new MyStrategyRound2Rocket();

            var debug = new Debug(writer);
            while (true)
            {
                ServerMessageGame message = ServerMessageGame.ReadFrom(reader);
                if (!message.PlayerView.HasValue)
                {
                    break;
                }

                PlayerView playerView = message.PlayerView.Value;

                if (message.PlayerView.Value.Game.CurrentTick == 0)
                {
                    _isTwo = playerView.Game.Units.Length >= 3;

                    if (_isTwo) SetPlayers(playerView);
                }

                var actions = new Dictionary<int, UnitAction>();

                if (_isTwo)
                {
                    Unit firstPl = new Unit();
                    Unit secondPl = new Unit();
                    foreach (var unit in playerView.Game.Units)
                    {
                        if (unit.Id == _firstPlayerId)
                        {
                            firstPl = unit;
                        }
                        else if (unit.Id == _secondPlayerId)
                        {
                            secondPl = unit;
                        }
                    }

                    if (BoombCount == 0)
                    {
                        if (firstPl.Health > 0)
                            actions.Add(firstPl.Id, myStrategyR2Rocket.GetAction(firstPl, playerView.Game, debug, secondPl));
                        if (secondPl.Health > 0)
                            actions.Add(secondPl.Id, myStrategyR2First.GetAction(secondPl, playerView.Game, debug, firstPl));
                    }
                    else
                    {
                        if (firstPl.Health > 0)
                            actions.Add(firstPl.Id, myStrategyR2First.GetAction(firstPl, playerView.Game, debug, secondPl));
                        if (secondPl.Health > 0)
                            actions.Add(secondPl.Id, myStrategyR2Rocket.GetAction(secondPl, playerView.Game, debug, firstPl));
                    }
                }
                else
                {
                    foreach (var unit in playerView.Game.Units)
                    {
                        if (unit.PlayerId == playerView.MyId)
                        {
                            actions.Add(unit.Id, myStrategyR1.GetAction(unit, playerView.Game, debug));
                        }
                    }
                }

                new PlayerMessageGame.ActionMessage(new Versioned(actions)).WriteTo(writer);
                writer.Flush();
            }
        }
        public static void Main(string[] args)
        {
            string host = args.Length < 1 ? "127.0.0.1" : args[0];
            int port = args.Length < 2 ? 31001 : int.Parse(args[1]);
            string token = args.Length < 3 ? "0000000000000000" : args[2];
            new Runner(host, port, token).Run();
        }

        private void SetPlayers(PlayerView playerView)
        {
            int firstId = -1;
            int secondId = -1;
            var lestSide = false;
            double firstX = 0;
            double secondX = 0;

            foreach (var unit in playerView.Game.Units)
            {
                if (unit.PlayerId == playerView.MyId)
                {
                    if (firstId == -1)
                    {
                        firstId = unit.Id;
                        firstX = unit.Position.X;
                        if (unit.Position.X <= 20)
                        {
                            lestSide = true;
                        }
                    }
                    else
                    {
                        secondId = unit.Id;
                        secondX = unit.Position.X;
                    }
                }
            }

            if (lestSide)
            {
                if (firstX > secondX)
                {
                    _firstPlayerId = firstId;
                    _secondPlayerId = secondId;
                }
                else
                {
                    _firstPlayerId = secondId;
                    _secondPlayerId = firstId;
                }
            }
            else
            {
                if (firstX > secondX)
                {
                    _firstPlayerId = secondId;
                    _secondPlayerId = firstId;
                }
                else
                {
                    _firstPlayerId = firstId;
                    _secondPlayerId = secondId;
                }
            }

            foreach (var lootBox in playerView.Game.LootBoxes)
            {
                if (!(lootBox.Item is Item.Mine weapon)) continue;
                BoombCount++;
            }
        }
    }
}