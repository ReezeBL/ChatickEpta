using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Protocol;

namespace Client
{
    public partial class ChatForm : Form
    {
        private ConnectionClient client;
        private delegate void printStr(String msg);
        public ChatForm(ConnectionClient manager)
        {
            InitializeComponent();
            client = manager;                     
        }

        private void Client_DisconnectEvent(object sender, EventArgs e)
        {
            MessageBox.Show("Отключен от сервера");
            Close();
        }

        private void Client_MessageEvent(object sender, string e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, string>(Client_MessageEvent), sender, e);
            }
            else
            {
                chatTextBox.AppendText(e);
                chatTextBox.AppendText(Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String message = messageTextBox.Text;
            if (message == "")
                return;
            if (message.StartsWith("/"))
            {
                client.Manager.sendPacket(new Protocol.Packets.PacketMessage(message));
            }
            else
            {
                client.Manager.sendPacket(new Protocol.Packets.PacketMessage(message));
                chatTextBox.AppendText("Me: " + message);
                chatTextBox.AppendText(Environment.NewLine);
            }
            messageTextBox.Text = "";
        }

        public void handleCommand(String commad)
        {
            String[] parts = commad.Split(' ');
            switch (parts[0])
            {
                case "/users":
                    client.Manager.sendPacket(new Protocol.Packets.PacketMessage(commad));
                    break;
                case "/rooms":
                    client.Manager.sendPacket(new Protocol.Packets.PacketRequest(1,0));
                    break;
                case "/room":
                    client.Manager.sendPacket(new Protocol.Packets.PacketRequest(2,int.Parse(parts[1])));
                    break;
            }
        }

        private void messageTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void ChatForm_Load(object sender, EventArgs e)  
        {
            client.RunReadingThread();
            client.MessageEvent += Client_MessageEvent;
            client.DisconnectEvent += Client_DisconnectEvent;
        }
        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Manager.sendPacket(new Protocol.Packets.PacketDisconnect());
        }
    }
}
