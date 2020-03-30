using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace socket
{
    public partial class Forem2 : Form
    {
        Socket socket;
        public Forem2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse(textBox1.Text);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, Convert.ToInt32(textBox2.Text));
            socket.Connect(iPEndPoint);
            ShowMsg("连接成功");
            Thread th = new Thread(recive);
            th.IsBackground = true;
            th.Start(socket);
            

        }
        void ShowMsg(String msg)
        {
            textBox3.AppendText(msg + "\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String str = textBox4.Text;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            socket.Send(buffer);
        }
        void recive(Object o)
        {
            Socket socket = (Socket)o as Socket;
            try
            {
                while (true)
                {
                    byte[] vs = new byte[1024 * 1024 * 2];
                    int r = socket.Receive(vs);
                    //实际接收到有效的字节数
                    if (r == 0)
                    {
                        break;
                    }
                    if (vs[0] == 0)
                    {
         
                        string str = Encoding.UTF8.GetString(vs, 1, r-1);
                        ShowMsg(socket.RemoteEndPoint + ":" + str);
                    }else if(vs[0] == 1)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
                        saveFileDialog.Title = "请选择要保存的问价";
                        saveFileDialog.Filter = "所有文件|*.*";
                        saveFileDialog.ShowDialog(this);
                        string path = saveFileDialog.FileName;
                        using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fs.Write(vs,1, r - 1);
                        }
                        MessageBox.Show("保存成功");
                    }else if(vs[0] == 2)
                    {
                        ZD();
                    }
                  
                   
                    
                }
            }
            catch { }
        }

        void ZD()
        {
            for (int i=0;i< 500; i++)
            {
                this.Location = new Point(200, 250);
                this.Location = new Point(250, 260);
            }
        }
        private void Forem2_Load(object sender, EventArgs e)
        {
            ContainerControl.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
