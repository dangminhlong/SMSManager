using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SMSManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        class IPItem
        {
            public string IP { get; set; }
        }

        List<IPItem> ListIP = new List<IPItem>();

        private void ScanIP(int port)
        {
            var gatewayIP = GetDefaultGateway().ToString();
            string a1 = gatewayIP.Split('.')[0];
            string a2 = gatewayIP.Split('.')[1];
            string a3 = gatewayIP.Split('.')[2];
            string a4 = gatewayIP.Split('.')[3];
            for (int i = 0; i < 255; i++)
            {
                if (i != int.Parse(a4))
                {
                    string strIpAddress = string.Format("{0}.{1}.{2}.{3}", a1, a2, a3, i);
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    
                    IPAddress ipAddress;

                    if (IPAddress.TryParse(strIpAddress, out ipAddress))
                    {
                        s.BeginConnect(new IPEndPoint(ipAddress, port), EndConnect, s);
                    }
                }
            }
        }

        void EndConnect(IAsyncResult ar)
        {
            try
            {
                Socket s = ar.AsyncState as Socket;
                s.EndConnect(ar);
                if (s.Connected)
                {
                    string ipAddress = s.RemoteEndPoint.ToString().Split(':')[0];
                    IPItem item = new IPItem();
                    item.IP = ipAddress;
                    ListIP.Add(item);                    
                    s.Disconnect(true);
                    cbbIPLIST.DataSource = ListIP;
                }
            }
            catch (Exception)
            {

            }
        }

        public IPAddress GetDefaultGateway()
        {
            var card = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            if (card == null) return null;
            var address = card.GetIPProperties().GatewayAddresses.FirstOrDefault();
            return address.Address;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cbbIPLIST.DisplayMember = "IP";
            cbbIPLIST.ValueMember = "IP";
            ScanIP(80);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ScanIP(80);
        }
    }
}
