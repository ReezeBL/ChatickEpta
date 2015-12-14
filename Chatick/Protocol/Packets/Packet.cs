using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Protocol.NetHandlers;

namespace Client.Protocol.Packets
{
    public class Packet
    {
        protected byte ID;
        public static Dictionary<int, Type> idMap = new Dictionary<int, Type>();      
        public static void Init()
        {
            idMap.Add(0, typeof(PacketLogin));
            idMap.Add(1, typeof(PacketMessage));
            idMap.Add(2, typeof(PacketDisconnect));
            idMap.Add(3, typeof(PacketRequest));    
        }
        public virtual void read(BinaryReader DataInput) { }
        public virtual void write(BinaryWriter DataOutput) {
            DataOutput.Write(ID);
        }    

        public virtual void handle(IHandler handler) { }
    }
}
