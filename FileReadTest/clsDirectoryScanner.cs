using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using static FileReadTest.DirectoryScanner;

namespace FileReadTest
{
    public delegate void ThreadScanEventHandler(DirectoryScanner Sender, ThreadScanEventArgs Args);
    public class DirectoryScanner
    {
        public class ThreadScanEventArgs
        {
            public string Message;
            public int FileCount;
            public bool IsComplete;
        }

        public DateTime StartTime
        { get; private set; }
        public event ThreadScanEventHandler ThreadScanEvent;
        public string RootDirectory
        { get; private set; }
        public string[] FileList
        { get; private set; }

        private volatile bool cont;
        private object sync;
        private Thread T;

        public DirectoryScanner(string RootDir)
        {
            sync = new object();
            RootDirectory = RootDir;
        }

        /// <summary>
        /// Starts scanning RootDirectory
        /// </summary>
        public void Start()
        {
            lock (sync)
            {
                if (T == null)
                {
                    StartTime = DateTime.UtcNow;
                    T = new Thread(scan)
                    {
                        IsBackground = true,
                        Name = "Scanner: " + RootDirectory,
                        Priority = ThreadPriority.BelowNormal
                    };
                    T.Start();
                }
            }
        }

        /// <summary>
        /// Aborts a scan process
        /// </summary>
        public void Abort()
        {
            lock (sync)
            {
                if (T != null)
                {
                    cont = false;
                    T.Join();
                    T = null;
                }
            }
        }

        /// <summary>
        /// Scans RootDirectory and its children for files
        /// </summary>
        private void scan()
        {
            cont = true;
            Stack<string> Dirs = new Stack<string>();
            List<string> Files = new List<string>();
            Dirs.Push(RootDirectory);
            while (cont && Dirs.Count > 0)
            {
                var Current = Dirs.Pop();
                string[] temp;
                try
                {
                    temp = Directory.GetDirectories(Current);
                }
                catch(Exception ex)
                {
                    temp = null;
                    ThreadScanEvent?.Invoke(this, new ThreadScanEventArgs()
                    {
                        Message = ex.Message,
                        FileCount = Files.Count,
                        IsComplete = false
                    });
                }
                if (temp != null)
                {
                    foreach (var dir in temp)
                    {
                        Dirs.Push(dir);
                    }
                }
                try
                {
                    temp = Directory.GetFiles(Current);
                }
                catch (Exception ex)
                {
                    temp = null;
                    ThreadScanEvent?.Invoke(this, new ThreadScanEventArgs()
                    {
                        Message = ex.Message,
                        FileCount = Files.Count,
                        IsComplete = false
                    });
                }
                if (temp != null)
                {
                    Files.AddRange(temp);
                    ThreadScanEvent?.Invoke(this, new ThreadScanEventArgs()
                    {
                        Message = null,
                        FileCount = Files.Count,
                        IsComplete = false
                    });
                }
            }
            FileList = Files.ToArray();
            T = null;
            ThreadScanEvent?.Invoke(this, new ThreadScanEventArgs()
            {
                Message = null,
                FileCount = Files.Count,
                IsComplete = true
            });
        }
    }
}
