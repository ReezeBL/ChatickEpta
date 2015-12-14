using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Protocol.NetHandlers;

namespace Server.Protocol.Packets
{
    public class PacketLogin : Packet
    {
        private String nickname;
        public String Name
        {
            get { return nickname; }
        }
        
        public PacketLogin() { this.ID = 0; }
        public PacketLogin(String nickname)
        {
            this.ID = 0;
            this.nickname = nickname;
        }

        public override void read(BinaryReader DataInput)
        {
            base.read(DataInput);
            nickname = DataInput.ReadString();
        }

        public override void write(BinaryWriter DataOutput)
        {
            base.write(DataOutput);
            DataOutput.Write(nickname);
        }

        public override void handle(IHandler handler)
        {
            handler.HandleLoginPacket(this);
        }
    }
}
