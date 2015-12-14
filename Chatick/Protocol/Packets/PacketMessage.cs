using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Protocol.NetHandlers;

namespace Client.Protocol.Packets
{
    public class PacketMessage : Packet
    {
        private string message;
        public String Message
        {
            get {return message; }
        }
        public PacketMessage() { this.ID = 1; }
        public PacketMessage(String message)
        {
            this.ID = 1;
            this.message = message;
        }
       

        public override void read(BinaryReader DataInput)
        {
            base.read(DataInput);
            message = DataInput.ReadString();
        }

        public override void write(BinaryWriter DataOutput)
        {
            base.write(DataOutput);
            DataOutput.Write(message);
        }

        public override void handle(IHandler handler)
        {
            handler.HandleMessagePacket(this);
        }
    }
}
