using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;

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
            ConfigurationManager configManager;
            FileInfo fileInf;

            string fileXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options.xml");
            string fileXSDconfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options.xsd");
            string fileJSON = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options.json");

            if (File.Exists(fileJSON))
            {
                configManager = new ConfigurationManager(fileJSON, string.Empty);
                fileInf = new FileInfo(fileJSON);
            }
            else if (File.Exists(fileXML) && File.Exists(fileXSDconfig))
            {
                configManager = new ConfigurationManager(fileXML, fileXSDconfig);
                fileInf = new FileInfo(fileXML);
            }
            else
                throw new ArgumentException("Configuration file is absent");

            Options opts = configManager.ParseOptions();
            logger = new Logger(opts, fileInf);
            Thread loggerT = new Thread(new ThreadStart(logger.Start));
            logger.Start();
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

            private readonly Options option;
            private readonly FileInfo fileInf;
            public Logger(Options opts, FileInfo info)
            {
                option = opts;
                fileInf = info;
                watcher = new FileSystemWatcher(opts.sourcePath);
                    
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

            private void Watcher_Renamed(object sender, RenamedEventArgs e)
            {
                string fileEvent = "renamed to " + e.FullPath;
                string filePath = e.OldFullPath;
                RecordEntry(fileEvent, filePath);
            }

            private void Watcher_Changed(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "changed";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
            }

            private void Watcher_Created(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "created";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
            }

            private void Watcher_Deleted(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "deleted";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
            }

            private void RecordEntry(string fileEvent, string filePath)
            {
                lock (obj)
                {
				using (StreamWriter writer = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory},templog.txt", true))
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
