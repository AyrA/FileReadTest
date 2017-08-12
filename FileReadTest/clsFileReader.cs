using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FileReadTest
{
    public delegate void FileReadResult(FileReadResultArgs Args);
    public delegate void FileProgressHandler(long Position, long Length, string FileName);

    public class FileReadResultArgs
    {
        public string FileName
        { get; set; }
        public string Hash
        { get; set; }
        public bool Success
        { get; set; }
    }

    public static class FileReader
    {
        private struct ThreadArgs
        {
            public Thread T;
            public string FileName;
            public FileStream FS;
            public bool GenerateHash;
            public FileReadResult Delegate;
            public FileProgressHandler FileProgress;
        }

        public enum ReadResult : int
        {
            /// <summary>
            /// The file is being read and the result is delivered via the delegate
            /// </summary>
            Reading = 0,
            /// <summary>
            /// Unable to open the file for reading
            /// </summary>
            ErrorOpening = 1,
            /// <summary>
            /// File was not found
            /// </summary>
            FileNotFound = 2
        }

        public static ReadResult ReadFile(string FileName, bool GenerateHash = false, FileReadResult Delegate = null, FileProgressHandler FileProgress = null)
        {
            if (File.Exists(FileName))
            {
                FileStream FS;
                try
                {
                    FS = File.OpenRead(FileName);
                }
                catch
                {
                    return ReadResult.ErrorOpening;
                }
                Thread T = new Thread(reader)
                {
                    IsBackground = true,
                    Name = "File reader for " + FileName,
                    Priority = ThreadPriority.BelowNormal
                };
                T.Start(new ThreadArgs()
                {
                    FileName = FileName,
                    FS = FS,
                    GenerateHash = GenerateHash,
                    T = T,
                    Delegate = Delegate,
                    FileProgress = FileProgress
                });
                return ReadResult.Reading;
            }
            return ReadResult.FileNotFound;
        }

        private static void reader(object o)
        {
            var TA = (ThreadArgs)o;
            //5 MB
            byte[] Buffer = new byte[1000 * 1000 * 5];
            HashAlgorithm HA = null;
            if (TA.GenerateHash)
            {
                HA = SHA256.Create();
            }
            int Readed = 0;
            double Perc = 0;
            using (TA.FS)
            {
                if (TA.FS.Length > 0)
                {
                    while (TA.FS.Position < TA.FS.Length)
                    {
                        try
                        {
                            Readed = TA.FS.Read(Buffer, 0, Buffer.Length);
                            if (HA != null)
                            {
                                if (TA.FS.Position < TA.FS.Length)
                                {
                                    HA.TransformBlock(Buffer, 0, Readed, Buffer, 0);
                                }
                                else
                                {
                                    HA.TransformFinalBlock(Buffer, 0, Readed);
                                }
                            }
                            if (Perc != CalcPerc(TA.FS.Position, TA.FS.Length))
                            {
                                Perc = CalcPerc(TA.FS.Position, TA.FS.Length);
                                TA.FileProgress?.Invoke(TA.FS.Position, TA.FS.Length, TA.FileName);
                            }
                        }
                        catch
                        {
                            if (HA != null)
                            {
                                using (HA)
                                {
                                    HA.Clear();
                                }
                            }
                            TA.Delegate?.Invoke(new FileReadResultArgs()
                            {
                                FileName = TA.FileName,
                                Hash = null,
                                Success = false
                            });
                            return;
                        }
                    }
                }
                else
                {
                    if (HA != null)
                    {
                        HA.TransformFinalBlock(new byte[0], 0, 0);
                    }
                }
            }
            byte[] Hash = null;
            if (HA != null)
            {
                Hash = (byte[])HA.Hash.Clone();
                using (HA)
                {
                    HA.Clear();
                }
            }
            TA.Delegate?.Invoke(new FileReadResultArgs()
            {
                FileName = TA.FileName,
                Hash = BitConverter.ToString(Hash).Replace("-", string.Empty),
                Success = true
            });
        }

        private static double CalcPerc(double curr, double max)
        {
            return curr * 100.0 / max;
        }
    }
}
