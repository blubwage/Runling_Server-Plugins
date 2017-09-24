using System.Collections.Generic;
using System.Linq;
using DarkRift;
using DarkRift.Server;

namespace RoomSystemPlugin
{
    public class Room : IDarkRiftSerializable
    {
        public string Name { get; }
        public GameType GameType { get; }
        public List<Player> PlayerList = new List<Player>();
        public List<Client> Clients = new List<Client>();
        public byte MaxPlayers => GetMaxPlayers();
        public bool HasStarted { get; }
        public bool IsVisible { get; }

        public Room(string name, GameType gameType, bool isVisible)
        {
            Name = name;
            GameType = gameType;
            IsVisible = isVisible;
            HasStarted = false;
        }

        internal bool AddPlayer(Player player, Client client)
        {
            if (PlayerList.Count >= MaxPlayers || HasStarted)
                return false;

            PlayerList.Add(player);
            Clients.Add(client);
            return true;
        }

        internal bool RemovePlayer(uint playerId)
        {
            if (PlayerList.All(p => p.Id != playerId))
                return false;

            PlayerList.Remove(PlayerList.Find(p => p.Id == playerId));
            return true;
        }

        private byte GetMaxPlayers()
        {
            switch (GameType)
            {
                case GameType.Arena:
                    return 8;
                case GameType.Runling:
                    return 10;
                default:
                    return 0;
            }
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Name);
            e.Writer.Write((byte)GameType);
            e.Writer.Write(MaxPlayers);
            e.Writer.Write((byte)PlayerList.Count);
        }

        public void Deserialize(DeserializeEvent e)
        {
        }
    }
}