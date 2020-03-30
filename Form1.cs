using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace socket
{
    
    public partial class Form1 : Form
    {
        Socket socketSend;
        Dictionary<String, Socket> dictory = new Dictionary<string, Socket>();
        public Form1()
        {
            InitializeComponent();
          
        }

    
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //当点击开始监听的时候 在服务器端创建一个负责监听ip地址跟端口号的socket
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Any;
                //创建端口对象
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(textPort.Text));
                //监听
                socketWatch.Bind(point);
                ShowMsg("监听成功");
                socketWatch.Listen(10);
              

                Thread th = new Thread(Listen);
                th.IsBackground = true;
                th.Start(socketWatch);
            }
            catch
            { }
        
           
        }
        //被线程执行的带参数函数必须为object类
        void Listen(Object o)
        {
            try
            {
                Socket socketWatch = o as Socket;
                while (true)
                {
                    //建立连接
                    socketSend = socketWatch.Accept();
                    dictory.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
                    cbouser.Items.Add(socketSend.RemoteEndPoint.ToString());
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
                    Thread th = new Thread(Recive);
                    th.IsBackground = true;
                    th.Start(socketSend);
                }
            }
            catch
            { }
      
        }

        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");

        }
       void Recive(Object o)
        {
            Socket socketSend = o as Socket;
            while (true)
            {
                try
                {
                    Byte[] buffer = new byte[1024 * 1024 * 2];
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    String str = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMsg(socketSend.RemoteEndPoint + ":" + str);
                }
                catch
                { }
            
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            Forem2 forem = new Forem2();
            forem.Show();
            Control.CheckForIllegalCrossThreadCalls = false;
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string str = msg.Text;
            byte[] buff = System.Text.Encoding.UTF8.GetBytes(str);
            string ip = cbouser.SelectedItem.ToString();
            dictory[ip].Send(buff);
           // socketSend.Send(buff);
        }
    }
}
