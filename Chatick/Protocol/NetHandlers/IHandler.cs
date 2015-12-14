using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Protocol.Packets;

namespace Client.Protocol.NetHandlers
{
    public abstract class IHandler
    {
        public abstract void HandleLoginPacket(PacketLogin packet);
        public abstract void HandleMessagePacket(PacketMessage packet);
        public abstract void HandleDisconnectPacket(PacketDisconnect packet);
        public abstract void HandleRequestPacket(PacketRequest packetRequest);
    }
}
