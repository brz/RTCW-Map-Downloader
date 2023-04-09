using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RTCWMapDownloader
{
    public partial class frmServerPasswordManager : Form
    {
        public frmServerPasswordManager()
        {
            InitializeComponent();
        }

        private void lvwServerPasswordManager_MouseMove(object sender, MouseEventArgs e)
        {
            if (r.Contains(e.Location))
                return;
            int columnIndex = getColumnIndex(e.Location);
            if (columnIndex == 3)
                lvwServerPasswordManager.Cursor = Cursors.Hand;
            else
                lvwServerPasswordManager.Cursor = Cursors.Default;
        }

        Rectangle r = Rectangle.Empty;
        private int getColumnIndex(Point p)
        {
            r = Rectangle.Empty;
            for (int i = 0; i < lvwServerPasswordManager.Columns.Count; i++)
            {
                r = new Rectangle(r.X + r.Width, 0, lvwServerPasswordManager.Columns[i].Width, lvwServerPasswordManager.Height);
                if (r.Contains(p))
                    return i;
            }
            return -1;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIp.Text) || string.IsNullOrWhiteSpace(txtPort.Text) || string.IsNullOrWhiteSpace(txtPassword.Text)) return;

            var currentServerPasswordItems = ServerPasswordManager.GetCurrentItems();

            var serverPasswordItem = new ServerPasswordItem
            {
                Ip = txtIp.Text,
                Port = txtPort.Text,
                Password = txtPassword.Text
            };
            currentServerPasswordItems.Add(serverPasswordItem);

            ServerPasswordManager.SaveItems(currentServerPasswordItems);

            var lvi = new ListViewItem(txtIp.Text);
            lvi.SubItems.Add(txtPort.Text);
            lvi.SubItems.Add(txtPassword.Text);
            lvi.SubItems.Add("X");
            lvwServerPasswordManager.Items.Add(lvi);

            txtIp.Text = null;
            txtPort.Text = null;
            txtPassword.Text = null;
        }

        private void frmServerPasswordManager_Load(object sender, EventArgs e)
        {
            var currentServerPasswordItems = ServerPasswordManager.GetCurrentItems();
            foreach(var item in currentServerPasswordItems)
            {
                var lvi = new ListViewItem(item.Ip);
                lvi.SubItems.Add(item.Port);
                lvi.SubItems.Add(item.Password);
                lvi.SubItems.Add("X");
                lvwServerPasswordManager.Items.Add(lvi);
            }
        }

        private void lvwServerPasswordManager_Click(object sender, EventArgs e)
        {
            Point mousePosition = lvwServerPasswordManager.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hit = lvwServerPasswordManager.HitTest(mousePosition);
            int columnIndex = hit.Item.SubItems.IndexOf(hit.SubItem);
            int rowIndex = hit.Item.Index;

            if(columnIndex == 3)
            {
                lvwServerPasswordManager.Items.RemoveAt(rowIndex);

                var newServerPasswordItems = new List<ServerPasswordItem>();
                foreach (ListViewItem item in lvwServerPasswordManager.Items)
                {
                    var serverPasswordItem = new ServerPasswordItem()
                    {
                        Ip = item.SubItems[0].Text,
                        Port = item.SubItems[1].Text,
                        Password = item.SubItems[2].Text
                    };
                    newServerPasswordItems.Add(serverPasswordItem);
                }

                ServerPasswordManager.SaveItems(newServerPasswordItems);
            }
        }
    }
}
