using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileReadTest
{
    public partial class frmScanner : Form
    {
        private string[] FileList;
        private bool Hashes;
        private bool Cont = true;
        private StreamWriter FileLog;

        public frmScanner(string[] FileList, bool GenerateHashes)
        {
            InitializeComponent();
            this.FileList = FileList;
            Hashes = GenerateHashes;
            FileLog = File.CreateText(Environment.ExpandEnvironmentVariables(@"%TEMP%\hashes.txt"));
            pbTotal.Maximum = FileList.Length;
        }

        private void ScanFile()
        {
            pbFile.Value = 0;
            if (Cont && pbTotal.Value < FileList.Length)
            {
                string current = null;
                Text = string.Format("File Reader | {0:0.0}%",pbTotal.Value / (double)FileList.Length * 100.0);
                switch (FileReader.ReadFile(current = FileList[pbTotal.Value++], Hashes, NextScan, Progress))
                {
                    case FileReader.ReadResult.ErrorOpening:
                        FileLog?.WriteLine("{0}\t[ERR_]", current);
                        ScanFile();
                        break;
                    case FileReader.ReadResult.FileNotFound:
                        FileLog?.WriteLine("{0}\t[_NA_]", current);
                        ScanFile();
                        break;
                    case FileReader.ReadResult.Reading:
                        lblFile.Text = current;
                        break;
                }
            }
            else
            {
                FileLog.Close();
                FileLog.Dispose();
                Close();
            }
        }

        private void Progress(long Pos, long Len, string FileName)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Progress(Pos, Len, FileName); });
            }
            else
            {
                pbFile.Value = (int)(Pos * 100 / Len);
            }
        }

        private void NextScan(FileReadResultArgs Args)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { NextScan(Args); });
            }
            else
            {
                if (Args.Hash != null)
                {
                    FileLog?.WriteLine("{0}\t{1}\t{2}", Args.FileName, Args.Success ? "[_OK_]" : "[FAIL]", Args.Hash);
                }
                else
                {
                    FileLog?.WriteLine("{0}\t{1}", Args.FileName, Args.Success ? "[_OK_]" : "[FAIL]");
                }
                ScanFile();
            }
        }

        private void frmScanner_Shown(object sender, EventArgs e)
        {
            ScanFile();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cont = false;
        }
    }
}
