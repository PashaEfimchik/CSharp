using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace FileWatcherService
{
    public partial class Service1 : ServiceBase
    {
        static internal Logger logger;
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger();
            Thread loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }
        internal class Logger
        {
            internal FileSystemWatcher watcher;
            object obj = new object();
            bool enabled = true;
            private string targetDirectoryPath = @"C:\Users\Павел\Desktop\Labs\3_sem\C#\Lab_2\out";
            private string sourceDirectoryPath = @"C:\Users\Павел\Desktop\Labs\3_sem\C#\Lab_2\in";
            public Logger()
            {
                Operations.targetDirectoryPath = targetDirectoryPath;
                Operations.sourceDirectoryPath = sourceDirectoryPath;

                watcher = new FileSystemWatcher(sourceDirectoryPath);
                watcher.Deleted += Watcher_Deleted;
                watcher.Created += Watcher_Created;
                watcher.Created += Operations.OnFileUpdated;
                watcher.Changed += Watcher_Changed;
                watcher.Renamed += Watcher_Renamed;
            }

            public void Start()
            {
                watcher.EnableRaisingEvents = true;
                while (enabled)
                {
                    Thread.Sleep(1000);
                }
            }
            public void Stop()
            {
                watcher.EnableRaisingEvents = false;
                enabled = false;
            }
            // переименование файлов
            private void Watcher_Renamed(object sender, RenamedEventArgs e)
            {
                string fileEvent = "переименован в " + e.FullPath;
                string filePath = e.OldFullPath;
                RecordEntry(fileEvent, filePath);
            }
            // изменение файлов
            private void Watcher_Changed(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "изменен";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
            }
            // создание файлов
            private void Watcher_Created(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "создан";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
            }

            // удаление файлов
            private void Watcher_Deleted(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "удален";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
            }

            private void RecordEntry(string fileEvent, string filePath)
            {
                lock (obj)
                {
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Павел\Desktop\Labs\3_sem\C#\Lab_2\templog.txt", true))
                    {
                        writer.WriteLine(String.Format("{0} файл {1} был {2}",
                            DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                        writer.Flush();
                    }
                }
            }
        }
    }
}
