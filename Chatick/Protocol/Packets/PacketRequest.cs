using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Protocol.NetHandlers;

namespace Client.Protocol.Packets
{
    public class PacketRequest : Packet
    {
        public int request;
        public int roomId;
        public PacketRequest(int req = 0, int roomID = 0)
        {
            this.ID = 4;
            request = req;
            roomId = roomID;
        }

        public override void read(BinaryReader DataInput)
        {
            base.read(DataInput);
            request = DataInput.ReadInt32();
            roomId = DataInput.ReadInt32();
        }

        public override void write(BinaryWriter DataOutput)
        {
            base.write(DataOutput);
            DataOutput.Write(request);
            DataOutput.Write(roomId);
        }

        public override void handle(IHandler handler)
        {
            handler.HandleRequestPacket(this);
        }
    }
}
