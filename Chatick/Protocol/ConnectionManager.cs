using Client.Protocol.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Protocol
{
    public class ConnectionManager
    {
        private BinaryWriter output;
        private BinaryReader input;

        public ConnectionManager(TcpClient client)
        {
            output = new BinaryWriter(client.GetStream());
            input = new BinaryReader(client.GetStream());
        }

        public Packet getPacket()
        {
            byte ID = input.ReadByte();
            Packets.Packet packet = (Packets.Packet)Activator.CreateInstance(Packets.Packet.idMap[ID]);
            packet.read(input);
            return packet;
        }
        public void sendPacket(Packet packet)
        {
            packet.write(output);
        }
    }
}
