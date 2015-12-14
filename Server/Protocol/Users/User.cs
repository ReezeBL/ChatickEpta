using Server.Protocol.NetHandlers;
using Server.Protocol.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Protocol.Users
{
    public class User
    {
        private String name = null;
        private BinaryReader input;
        private BinaryWriter output;
        private Thread userThread;
        private IHandler handler;

        public int roomId;

        public String username
        {
            get { return name == null ? "" : name; }
            set { name = value; }
        }

        public User(TcpClient client, Server server)
        {
            input = new BinaryReader(client.GetStream());
            output = new BinaryWriter(client.GetStream());
            handler = new ServerHandler(server, this);
            userThread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Packet p = GetPacket();
                        p.handle(handler);
                        Thread.Sleep(100);
                    }
                }
                catch (Exception)
                {
                    server.UnregisterUser(this);
                }
            });
            userThread.IsBackground = true;
            userThread.Start();
        }

        private Packet GetPacket()
        {
            byte ID = input.ReadByte();
            Packets.Packet packet = (Packets.Packet)Activator.CreateInstance(Packets.Packet.idMap[ID]);
            packet.read(input);
            return packet;
        }

        public void SendPacket(Packet packet)
        {
            try
            {
                packet.write(output);
            }
            catch (IOException) { }
        }
    }
}
