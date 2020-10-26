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


namespace KullanıcıKayıtRehberi
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private TcpClient client1;
        private TcpClient client2;
        public StreamReader STR;
        public StreamWriter STW;
        public StreamReader STR1;
        public StreamWriter STW1;
        public StreamReader STR2;
        public StreamWriter STW2;
        VeriTabanı v = new VeriTabanı();
        public string recieve="a";
        public string recieve1="a";
        public string recieve2="a";
        public string TextToSend;
        List<TcpClient> list_socket = new List<TcpClient>();
        public Form1()
        {
            
            InitializeComponent();
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach(IPAddress address in localIP)
            {
                if(address.AddressFamily==AddressFamily.InterNetwork)
                {
                    ServerIPtextBox.Text = address.ToString();
                }
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(ServerPorttextBox.Text));
            listener.Start();

            for (int i = 0; i < 3; i++)//3 kullanıcı almak için
            {
                client = listener.AcceptTcpClient();
                list_socket.Add(client);
            }
            label2.Visible = false;
            label3.Visible = false;
            ServerIPtextBox.Visible = false;
            ServerPorttextBox.Visible = false;
            StartButton.Visible = false;
            client = list_socket[0];
            client1 = list_socket[1];
            client2 = list_socket[2];
            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STR1 = new StreamReader(client1.GetStream());
            STW1 = new StreamWriter(client1.GetStream());
            STR2 = new StreamReader(client2.GetStream());
            STW2 = new StreamWriter(client2.GetStream());
            STW.AutoFlush = true;
            STW1.AutoFlush = true;
            STW2.AutoFlush = true;

                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker3.RunWorkerAsync();
                backgroundWorker4.WorkerSupportsCancellation = true;
                backgroundWorker5.RunWorkerAsync();
                backgroundWorker6.WorkerSupportsCancellation = true;
            

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while(client.Connected)
            {
                try
                {
                    recieve = STR.ReadLine();
                    recieve = recieve.ToUpper();
                    String x = v.cevapCek(Convert.ToInt32(TextToSend));
                    if (recieve == x)
                    {
                        this.ChatScreentextBox.Invoke(new MethodInvoker(delegate ()
                            {
                                ChatScreentextBox.AppendText("YARIŞMACI1 CEVABI:\n " + recieve + "\n");
                            }));
                        if (list_socket.Count == 1)
                        {
                            STW.WriteLine("TEBRİKLER KAZANDINIZ");
                            MessageBox.Show("Yarışma Bitti!!YARIMACI1 KAZANDI");
                            Application.Exit();
                        }
                    }
                    else
                    {
                        STW.WriteLine("ELENDİNİZ");
                        STW.Flush();
                        STW.Close();
                        STR.Close();
                        list_socket.Remove(client);
                        client.Close();
                        backgroundWorker2.CancelAsync();
                        backgroundWorker1.CancelAsync();

                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if(client.Connected)
            {
                STW.WriteLine(v.soruCek(Convert.ToInt32(TextToSend)) + "?");
            }
            else
            {

            }
            backgroundWorker2.CancelAsync();
        }

  
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client1.Connected)
            {
                try
                {
                    recieve1 = STR1.ReadLine();
                    recieve1 = recieve1.ToUpper();
                    String x = v.cevapCek(Convert.ToInt32(TextToSend));
                    if (recieve1 == x)
                    {
                        this.ChatScreentextBox.Invoke(new MethodInvoker(delegate ()
                        {
                            ChatScreentextBox.AppendText("YARIŞMACI2 CEVABI:\n " + recieve1 + "\n");
                        }));
                        if (list_socket.Count == 1)
                        {
                            STW1.WriteLine("TEBRİKLER KAZANDINIZ");
                            MessageBox.Show("Yarışma Bitti!! YARIŞMACI2 KAZANDI!!");
                            Application.Exit();
                        }
                    }
                    else
                    {
                        STW1.WriteLine("ELENDİNİZ");
                        STW1.Flush();
                        STW1.Close();
                        STR1.Close();
                        list_socket.Remove(client1);
                        client1.Close();
                        backgroundWorker3.CancelAsync();
                        backgroundWorker4.CancelAsync();

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void ChatScreentextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker4_DoWork_1(object sender, DoWorkEventArgs e)
        {
            if (client1.Connected)
            {
                STW1.WriteLine(v.soruCek(Convert.ToInt32(TextToSend))+ "?");
            }
            else
            {
                
            }
            backgroundWorker2.CancelAsync();
        }

        private void backgroundWorker5_DoWork_1(object sender, DoWorkEventArgs e)
        {
            while (client2.Connected)
            {
                try
                {
                    recieve2 = STR2.ReadLine();
                    recieve2 = recieve2.ToUpper();
                    String x = v.cevapCek(Convert.ToInt32(TextToSend));
                    if (recieve2 == x)
                    {

                        this.ChatScreentextBox.Invoke(new MethodInvoker(delegate ()
                        {
                            ChatScreentextBox.AppendText("YARIŞMACI3 CEVAP:\n " + recieve2 + "\n");
                        }));
                        if (list_socket.Count == 1)
                        {
                            STW2.WriteLine("TEBRİKLER KAZANDINIZ");
                            MessageBox.Show("Yarışma Bitti!! YARIŞMACI3 KAZANDI!!");
                            Application.Exit();
                        }
                    }
                    else
                    {
                        STW2.WriteLine("ELENDİNİZ");
                        STW2.Flush();
                        STW2.Close();
                        STR2.Close();
                        list_socket.Remove(client2);
                        client2.Close();
                        backgroundWorker5.CancelAsync();
                        backgroundWorker6.CancelAsync();

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void backgroundWorker6_DoWork_1(object sender, DoWorkEventArgs e)
        {
            if (client2.Connected)
            {
                STW2.WriteLine(v.soruCek(Convert.ToInt32(TextToSend))+"?");
                this.ChatScreentextBox.Invoke(new MethodInvoker(delegate ()
                {
                    ChatScreentextBox.AppendText("SORU:\n" + v.soruCek(Convert.ToInt32(TextToSend)) + "\n");
                }));
            }
            else
            {
                
            }
            backgroundWorker2.CancelAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (recieve != " " && recieve1 != " " && recieve2 != " ")
            {
                if (MessagetextBox.Text != "")
                {
                    if (list_socket.Count == 1)
                    {
                        if (client.Connected)
                        {
                            STW.WriteLine("TEBRİKLER KAZANDINIZ");
                            MessageBox.Show("YARIŞMA BİTTİ.YARIŞMACI 1 KAZANDI!!");
                            Application.Exit();
                        }
                        if (client1.Connected)
                        {
                            STW1.WriteLine("TEBRİKLER KAZANDINIZ");
                            MessageBox.Show("YARIŞMA BİTTİ.YARIŞMACI 2 KAZANDI!!");
                            Application.Exit();
                        }
                        if (client2.Connected)
                        {
                            STW2.WriteLine("TEBRİKLER KAZANDINIZ");
                            MessageBox.Show("YARIŞMA BİTTİ.YARIŞMACI 3 KAZANDI!!");
                            Application.Exit();
                        }



                    }
                    TextToSend = MessagetextBox.Text;

                    backgroundWorker2.RunWorkerAsync();
                    backgroundWorker4.RunWorkerAsync();
                    backgroundWorker6.RunWorkerAsync();
                    recieve = " ";
                    recieve1 = " ";
                    recieve2 = " ";

                }
                else
                {
                    MessageBox.Show("SORU NUMARASI YAZINIZ!!");
                }
        }
            else
            {
                MessageBox.Show("cevaplar gelmeden yeni soru gönderemezsiniz");
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
