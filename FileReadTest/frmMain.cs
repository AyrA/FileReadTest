using System;
using System.IO;
using System.Windows.Forms;

namespace FileReadTest
{
    public partial class frmMain : Form
    {
        private DateTime LastUpdate;
        public frmMain()
        {
            InitializeComponent();
        }

        private void SetControlState(bool Enabled)
        {
            btnTest.Enabled = btnBrowse.Enabled = tbSource.Enabled = cbHashes.Enabled = Enabled;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbSource.Text) && Directory.Exists(tbSource.Text))
            {
                SetControlState(false);
                LastUpdate = DateTime.Now;
                tbLog.Text = "";
                var Scanner = new DirectoryScanner(tbSource.Text);
                Scanner.ThreadScanEvent += Logger;
                Scanner.Start();
            }
        }

        private void Logger(DirectoryScanner Sender, DirectoryScanner.ThreadScanEventArgs x)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke((MethodInvoker)(delegate { Logger(Sender, x); }));
                }
                catch
                {
                    Sender.Abort();
                    SetControlState(false);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(x.Message) || x.IsComplete || DateTime.UtcNow.Subtract(LastUpdate).TotalMilliseconds >= 500)
                {
                    LastUpdate = DateTime.UtcNow;
                    if (!string.IsNullOrEmpty(x.Message))
                    {
                        tbLog.Text = $"Error: {x.Message}\r\n" + tbLog.Text;
                    }
                    Text = $"Files: {x.FileCount}";
                    if (x.IsComplete)
                    {
                        tbLog.Text = "Operation Completed\r\n" + tbLog.Text;
                        SetControlState(false);
                    }
                }
            }
        }
    }
}
