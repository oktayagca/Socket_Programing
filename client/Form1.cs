using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace CLİENT
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string recieve=" ";
        public string TextToSend;
        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint ıPEnd = new IPEndPoint(IPAddress.Parse(ClientIPtextBox.Text), int.Parse(ClientPorttextBox.Text));
            label4.Visible = false;
            label5.Visible = false;
            ClientIPtextBox.Visible = false;
            ClientPorttextBox.Visible = false;
            ConnectButton.Visible = false;

            try
            {
                client.Connect(ıPEnd);
                if (client.Connected)
                {
                    ChatScreentextBox.AppendText("Connected to Server" + "\n");
                    STR = new StreamReader(client.GetStream());
                    STW = new StreamWriter(client.GetStream());
                    STW.AutoFlush = true;
                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker2.WorkerSupportsCancellation = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            while (client.Connected)
            {
                try
                {

                    recieve = STR.ReadLine();
                    this.ChatScreentextBox.Invoke(new MethodInvoker(delegate ()
                    {
                        ChatScreentextBox.AppendText("SORU:\n " + recieve + "\n");
                    }));
                    if (recieve == "ELENDİNİZ")
                    {
                        MessageBox.Show("elendiniz");
                        
                        client.Close();
                        this.Close();
                        Application.Exit();
                    }
                    recieve = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                STW.WriteLine(TextToSend);
                this.ChatScreentextBox.Invoke(new MethodInvoker(delegate ()
                {
                    ChatScreentextBox.AppendText("CEVAP:\n" + TextToSend + "\n");
                }));
            }
            else
            {
                MessageBox.Show("Sending Failed");
            }
            backgroundWorker2.CancelAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (recieve != " ")
            {
                if (MessagetextBox.Text != "")
                {

                    TextToSend = MessagetextBox.Text;
                    backgroundWorker2.RunWorkerAsync();
                    recieve = " ";
                }
                MessagetextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Soru gelmeden cevap veremezsiniz");
            }
        }



    }
}
