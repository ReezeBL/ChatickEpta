using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Protocol.Packets;
using Server.Protocol.Users;

namespace Server.Protocol.NetHandlers
{
    public class ServerHandler : IHandler
    {
        private Server server;
        private User client;
        public ServerHandler(Server serv, User cli)
        {
            server = serv;
            client = cli;
        }

        public override void HandleDisconnectPacket(PacketDisconnect packet)
        {
            server.UnregisterUser(client);
            Console.WriteLine(client.username + " has disconnected!");
        }

        public override void HandleMessagePacket(PacketMessage packet)
        {
            if (packet.Message.StartsWith("/"))
            {
                String[] parts = packet.Message.Split(' ');
                switch (parts[0])
                {
                    case "/users":
                        new Protocol.Packets.PacketRequest(0,0).handle(this);
                        break;
                    case "/rooms":
                        new Protocol.Packets.PacketRequest(1, 0).handle(this);
                        break;
                    case "/room":
                       new Protocol.Packets.PacketRequest(2, int.Parse(parts[1])).handle(this);
                        break;
                }
            }
            else
            {
                Room r = server.getRoomByID(client.roomId);
                r.SendMessage(String.Format("{0} : {1}", client.username, packet.Message), client);
                Console.WriteLine("{0}) {1} : {2}", client.roomId, client.username, packet.Message);
            }
        }

        public override void HandleLoginPacket(PacketLogin packet)
        {
            client.username = packet.Name;
            if (!server.RegisterUser(client))
            {
                client.SendPacket(new PacketMessage("Данное имя уже используется!"));
                client.SendPacket(new PacketDisconnect());
            }
            else
            {
                client.SendPacket(new PacketLogin(""));
                client.SendPacket(new PacketMessage("Добро пожаловать!"));
                Console.WriteLine(client.username + " has connected");
            }
        }

        public override void HandleRequestPacket(PacketRequest packet)
        {
            Console.WriteLine("Another fucking request..");
            if (packet.request == 0)
            {
                Room r = server.getRoomByID(client.roomId);
                client.SendPacket(new PacketMessage(r.formUserList()));
            }
            if(packet.request == 1)
            {
                client.SendPacket(new PacketMessage(server.formRoomList()));
            }
            if(packet.request == 2)
            {
                Room r = server.getRoomByID(packet.roomId);
                if(r == null)
                {
                    server.CreateRoom(packet.roomId);
                    r = server.getRoomByID(packet.roomId);
                }
                server.getRoomByID(client.roomId).RemoveUser(client);
                r.AddUser(client);                
                Console.WriteLine("User {0} has been moved from room {1} to {2}", client.username, client.roomId, r.ID);
                client.roomId = r.ID;
            }
        }
      
    }
}
