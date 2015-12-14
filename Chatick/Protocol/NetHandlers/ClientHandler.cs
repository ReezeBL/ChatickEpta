using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Protocol.Packets;

namespace Client.Protocol.NetHandlers
{
    public class ClientHandler : IHandler
    {
        private ConnectionClient msgr;
        public ClientHandler(ConnectionClient msgr)
        {
            this.msgr = msgr;
        }

        public override void HandleDisconnectPacket(PacketDisconnect packet)
        {
            msgr.RaiseDisconnectEvent();
        }
        public override void HandleMessagePacket(PacketMessage packet)
        {
            msgr.RaiseMessageEvent(packet.Message);
        }
        public override void HandleLoginPacket(PacketLogin packet)
        {
            msgr.RaiseLoginEvent();
        }
        public override void HandleRequestPacket(PacketRequest packetRequest)
        {
            throw new NotImplementedException();
        }
    }
}
