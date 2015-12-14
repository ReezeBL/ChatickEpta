using Server.Protocol.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Protocol.Users
{
    public class Room
    {
        public List<User> users = new List<User>();
        public readonly int ID = 0;
        public string Name = "";
        public Room(int id)
        {
            ID = id;
        }

        public void AddUser(User user)
        {
            users.Add(user);
            SendMessage(String.Format("К нам пришел {0}!", user.username), user);
        }
        public void AddUserAnonimus(User user)
        {
            users.Add(user);
        }
        public void RemoveUser(User user)
        {
            users.Remove(user);
            SendMessage(String.Format("{0} покинул нас(", user.username), user);
        }

        public void SendMessage(String message, User user)
        {
            foreach (var u in users)
            {
                if(u!=user)
                    u.SendPacket(new PacketMessage(message));
            }
        }

        public String formUserList()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Список пользователей: ");
            foreach(User u in users)
            {
                sb.AppendLine("\t" + u.username);
            }
            return sb.ToString();
        }
    }
}
