using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BNP2PExample
{
    public partial class TalkerForm : Form
    {
        private const int DEFAULT_POSITION = -1;

        private HelpForm helpForm;

        public TalkerForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClearStatus();
            helpForm = new HelpForm();
            helpForm.Hide();
            tbTransportAddress.Text = "localhost";
            nudTransportPort.Minimum = 1025;
            nudTransportPort.Maximum = 65536;
            nudTransportPort.Value = 3336;
            //
            // args: transportAddress, Port
            int caLen = Environment.GetCommandLineArgs().Length;
            if (caLen > 1)
            {
                tbTransportAddress.Text = Environment.GetCommandLineArgs()[1];
            }
            if (caLen > 2)
            {
                try
                {
                    int i = int.Parse(Environment.GetCommandLineArgs()[2]);
                    this.nudTransportPort.Value = i;
                }
                catch (Exception) { }
            }
            SetFormTitle();
            Position(DEFAULT_POSITION);
        }

        private void SetFormTitle()
        {
            string debug = (System.Diagnostics.Debugger.IsAttached ? "Debug" : "");
            string processId = Process.GetCurrentProcess().Id.ToString();
            this.Text = "Talker " + Environment.MachineName + " (pid=" + processId + ") " + debug;
        }
        
        private void Talker_FormClosed(object sender, FormClosedEventArgs e)
        {
            helpForm.Close();
            this.Hide(); // could take some time to close all connections - this makes it look more snappy 
            foreach (TreeNode treeNode in tvConnections.Nodes)
            {
                if (treeNode.Parent != null) continue; // only interested in top-level nodes 
                TalkerSession<string> talkerSession = (TalkerSession<string>) treeNode.Tag;
                talkerSession.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearStatus();
            tbReceived.Clear();
        }

        private void Talker2Form_Resize(object sender, EventArgs e)
        {
            int m = tvConnections.Left;
            tvConnections.Width = (this.Width / 2) - m - m;
            tbReceived.Width = tvConnections.Width;
            tbReceived.Left = tvConnections.Width + m + m;
        }

        private void ClearStatus()
        {
            toolStripStatusLabel1.Text = "";
        }

        private void btnNewServer_Click(object sender, EventArgs e)
        {
            ClearStatus();
            TalkerSession<string> talkerSession;
            try
            {
                talkerSession = new TalkerSession<string>(TalkerSession<string>.SessionTypeEnum.Server, GetServerAddressString());
                talkerSession.SubscribeMessageReceived(new TalkerSession<string>.MessageReceiverDelegate(MessageReceivedDistributor));
            }
            catch (Exception e1)
            {
                toolStripStatusLabel1.Text = e1.Message;
                return;
            }
            TreeNode treeNode = tvConnections.Nodes.Add("Server " + GetServerAddressString());
            treeNode.Tag = talkerSession;
            talkerSession.TopLevelTreeNode = treeNode; 
        }

        private void btnNewClient_Click(object sender, EventArgs e)
        {
            ClearStatus();
            TalkerSession<string> talkerSession;
            try
            {
                talkerSession = new TalkerSession<string>(TalkerSession<string>.SessionTypeEnum.Client, GetClientAddressString());
                talkerSession.SubscribeMessageReceived(new TalkerSession<string>.MessageReceiverDelegate(MessageReceivedDistributor));
            }
            catch (Exception e1)
            {
                toolStripStatusLabel1.Text = e1.Message;
                return;
            }
            TreeNode treeNode = tvConnections.Nodes.Add("Client " + GetClientAddressString());
            treeNode.Tag = talkerSession;
            talkerSession.TopLevelTreeNode = treeNode; 
        }

        private string GetServerAddressString()
        {
            return GetAddressString("localhost", nudTransportPort.Value.ToString());
        }

        private string GetClientAddressString()
        {
            return GetAddressString(tbTransportAddress.Text, nudTransportPort.Value.ToString());
        }

        private string GetAddressString(string address, string port)
        {
            return "bnmq://" + address + ":" + port;
        }

        private void btnNewProcess_Click(object sender, EventArgs e)
        {
            ClearStatus();
            string args = tbTransportAddress.Text + " " + nudTransportPort.Value.ToString();
            System.Diagnostics.Process.Start(System.Windows.Forms.Application.ExecutablePath, args);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helpForm.WindowState = FormWindowState.Normal;
            helpForm.Show();
            helpForm.Activate();
        }

        private void menuItem_Click(object sender, System.EventArgs e)
        {
            ClearStatus();
            MenuItem menuItem = (MenuItem)sender;
            TreeNode treeNode = tvConnections.SelectedNode;
            Debug.Assert(treeNode.Tag.GetType() == typeof(TalkerSession<string>));
            TalkerSession<string> talkerSession = (TalkerSession<string>)treeNode.Tag;
            Debug.Assert(talkerSession != null);
            if (menuItem.Text == TEST_MESSAGE)
            {
                talkerSession.SendMessageToAllPeers(treeNode.Text);
                return;
            }
            toolStripStatusLabel1.Text = "Context Item " + menuItem.Text + " not implemented";
        }
        //
        private const string TEST_MESSAGE = "Test Message";
        //
        private void tvConnections_MouseUp(object sender, MouseEventArgs e)
        {
            Point clickPoint = new Point(e.X, e.Y);
            TreeNode clickNode = tvConnections.GetNodeAt(clickPoint);
            if (clickNode == null) return;

            if (clickNode.IsSelected == false)
            {
                tvConnections.SelectedNode = clickNode;
            }

            if (e.Button != MouseButtons.Right) return;

            EventHandler menuItemClickHandler = new System.EventHandler(menuItem_Click);
            MenuItem[] menuItems = new MenuItem[] 
                {
                    new MenuItem(TEST_MESSAGE,      menuItemClickHandler),
                };
            ContextMenu contextMenu = new ContextMenu(menuItems);

            // Convert from Tree coordinates to Screen coordinates
            Point screenPoint = tvConnections.PointToScreen(clickPoint);
            // Convert from Screen coordinates to Form coordinates
            Point formPoint = this.PointToClient(screenPoint);
            // Show context menu
            contextMenu.Show(this, formPoint);
            return;
        }

        #region Position

        /// <summary>
        /// Returns the position number (zero thru five) that is nearest to the forms current position. The position numbers are:
        /// 0 = upper left, 1 = upper right, 2 = middle left, 3 = middle right, 4 = lower left, 5 = lower right
        /// </summary>
        private int Position()
        {
            Rectangle rc = Screen.PrimaryScreen.WorkingArea;
            int w = rc.Width / 2;
            int h = rc.Height / 3;
            int smallestDistance = int.MaxValue;
            int pos = 0;
            int d;
            //
            d = Distance(this.Left, this.Top, 0, 0);
            if (d < smallestDistance)
            {
                smallestDistance = d;
                pos = 0;
            }
            //
            d = Distance(this.Left, this.Top, w, 0);
            if (d < smallestDistance)
            {
                smallestDistance = d;
                pos = 1;
            }
            //
            d = Distance(this.Left, this.Top, 0, h);
            if (d < smallestDistance)
            {
                smallestDistance = d;
                pos = 2;
            }
            //
            d = Distance(this.Left, this.Top, w, h);
            if (d < smallestDistance)
            {
                smallestDistance = d;
                pos = 3;
            }
            //
            d = Distance(this.Left, this.Top, 0, h + h);
            if (d < smallestDistance)
            {
                smallestDistance = d;
                pos = 4;
            }
            //
            d = Distance(this.Left, this.Top, w, h + h);
            if (d < smallestDistance)
            {
                smallestDistance = d;
                pos = 5;
            }
            return pos;
        }

        private int Distance(int x1, int y1, int x2, int y2)
        {
            int xd = x2 - x1;
            int yd = y2 - y1;
            return (int)Math.Sqrt(xd * xd + yd * yd);
        }

        /// <summary>
        /// Given a position number resizes the current window to 1/6 of the screen and positions the window in that slot. A position
        /// number greater than 5 is modulo'd so that the position is always zero thru five. A negative number positions middle center
        /// instead of one of the 6 slots, which are upper left, upper right, middle left, middle right, lower left, lower right. 
        /// </summary>
        private void Position(int pos)
        {
            Rectangle rc = Screen.PrimaryScreen.WorkingArea;
            int w = rc.Width / 2;
            int h = rc.Height / 3;
            this.Width = w;
            this.Height = h;
            // Negative pos means middle center position (not one of the six "slots"). 
            // Random offset helps make it clear when new process is started otherwise
            // forms exactly stack which could confuse a new user.
            if (pos < 0)
            {
                int offset = new Random().Next(-50, 50);
                this.Top = (h / 2) + offset;
                this.Left = w / 2 + offset;
                return;
            }
            //
            pos %= 6; // always zero thru five
            //
            int top;
            int left;
            switch (pos)
            {
                case 0:
                    top = 0;
                    left = 0;
                    break;
                case 1:
                    top = 0;
                    left = w;
                    break;
                case 2:
                    top = h;
                    left = 0;
                    break;
                case 3:
                    top = h;
                    left = w;
                    break;
                case 4:
                    top = h + h;
                    left = 0;
                    break;
                case 5:
                    top = h + h;
                    left = w;
                    break;
                default: // should never happen
                    throw new Exception("Position outside expected range");
            }
            this.Top = top;
            this.Left = left;
        }

        private void upperLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position(0);
        }

        private void upperRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position(1);
        }

        private void middleLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position(2);
        }

        private void middleRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position(3);
        }

        private void lowerLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position(4);
        }

        private void lowerRIghtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position(5);
        }

        #endregion

        private void MessageReceivedDistributor(TalkerSession<string> talkerSession, string message)
        {
            MessageReceiver(talkerSession, message);
        }

        private void MessageReceiver(TalkerSession<string> talkerSession, string message)
        {
            if (this.tbReceived.InvokeRequired) // calling thread same as form thread? 
            {
                TalkerSession<string>.MessageReceiverDelegate d = new TalkerSession<string>.MessageReceiverDelegate(MessageReceiver);
                this.Invoke(d, new object[] { talkerSession, message }); // synchronous call - for asynch use BeginInvoke instead 
            }
            else
            {
                this.Activate();
                this.tbReceived.AppendText(DateTime.Now.ToString("HH:mm:ss ") + message + Environment.NewLine);
            }
        }
    }
}