using Client.Protocol;
using Client.Protocol.NetHandlers;
using Client.Protocol.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Protocol
{
    public class ConnectionClient
    {

        public String username = "";

        public event EventHandler<String> MessageEvent;
        public event EventHandler LoginEvent;
        public event EventHandler DisconnectEvent;
        internal void RaiseDisconnectEvent()
        {
            if(DisconnectEvent != null)
                DisconnectEvent(this,null);
        }
        internal void RaiseMessageEvent(string message)
        {
            if(MessageEvent != null)
                MessageEvent(this, message);
        }
        internal void RaiseLoginEvent()
        {
            if(LoginEvent!=null)
                LoginEvent(this, null);
        }

        private IHandler handler;
        private ConnectionManager manager;
        private Thread readingThread;
        public ConnectionClient()
        {
            handler = new ClientHandler(this);
        }   
        
        public ConnectionClient(ConnectionManager manager)
        {
            handler = new ClientHandler(this);
            this.manager = manager;
        } 

        public void Connect(String Address)
        {
            String[] param = Address.Split(':');
            try {
                TcpClient client = new TcpClient(param[0], int.Parse(param[1]));          
                manager = new ConnectionManager(client);
                RunReadingThread();       
                manager.sendPacket(new PacketLogin(username));              
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to connect to server!");
            }
        }
        public ConnectionManager Manager { get { return manager; } }
        public void RunReadingThread()
        {
            readingThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        manager.getPacket().handle(handler);                        
                    }
                    catch (IOException){
                        RaiseDisconnectEvent();                 
                        break;                        
                    }
                }
            });
            readingThread.IsBackground = true;
            readingThread.Start();
        }
        public void StopReadingThread()
        {
            if(readingThread!=null)
                readingThread.Abort();
        }
    }
}
