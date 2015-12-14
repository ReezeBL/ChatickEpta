using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Protocol.NetHandlers;

namespace Server.Protocol.Packets
{
    public class PacketDisconnect : Packet
    {
        public PacketDisconnect() { this.ID = 2; }

        public override void handle(IHandler handler)
        {
            handler.HandleDisconnectPacket(this);
        }
    }
}
