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
    public partial class LoginForm : Form
    {
        ConnectionClient client = new ConnectionClient();
        public LoginForm()
        {
            InitializeComponent();
            client.LoginEvent += Client_LoginEvent;
            client.MessageEvent += Client_MessageEvent;
            client.DisconnectEvent += Client_DisconnectEvent;
        }

        private void Client_DisconnectEvent(object sender, EventArgs e)
        {
            client.StopReadingThread();
        }

        private void Client_MessageEvent(object sender, string e)
        {
            MessageBox.Show(e);
        }

        private void Client_LoginEvent(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, EventArgs>(Client_LoginEvent), sender, e);
            }
            else
            {
                Hide();
                client.StopReadingThread();
                new ChatForm(new ConnectionClient(client.Manager)).ShowDialog();
                Close();
            }
        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            client.Connect(ipTextBox.Text);
        }

        private void nicknameTextBox_TextChanged(object sender, EventArgs e)
        {
            client.username = nicknameTextBox.Text;
        }
    }
}
