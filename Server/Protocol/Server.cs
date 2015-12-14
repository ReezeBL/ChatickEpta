using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows;
using System.IO;
using Server.Protocol.Users;

namespace Server.Protocol
{
    public class Server
    {      
        TcpListener serverListener;
        bool isRunning;
        List<User> users = new List<User>();
        List<Room> rooms = new List<Room>();
        Room defaultRoom = new Room(0);
        public bool Running
        {
            get { return isRunning; }
        }
        public Server()
        {
            rooms.Add(defaultRoom);
        }
        public void Run()
        {
            serverListener = new TcpListener(IPAddress.Any, 1488);
            serverListener.Start();
            while (true)
            {
                TcpClient client = serverListener.AcceptTcpClient();
                new User(client, this);
                Thread.Sleep(100);
            }
        }   

        public bool RegisterUser(User user)
        {
            if (users.FirstOrDefault(u => u.username == user.username) != null)
                return false;
            users.Add(user);
            defaultRoom.AddUser(user);
            return true;
        }

        public void UnregisterUser(User user)
        {
            Room r = rooms.First(rm => rm.ID == user.roomId);
            r.RemoveUser(user);
            users.Remove(user);
        }

        public Room getRoomByID(int ID)
        {
            return rooms.FirstOrDefault(r => r.ID == ID);
        }

        public String formRoomList()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Список комнат: ");
            foreach (Room r in rooms)
            {
                sb.AppendLine("\t" + r.ID.ToString() + ") - пользователей - " + r.users.Count);
            }
            return sb.ToString();
        }

        public void CreateRoom(int ID)
        {
            rooms.Add(new Room(ID));
        }
    }
}
