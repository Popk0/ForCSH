using System;
using System.ServiceProcess;
using System.IO;
using System.Threading;
using AService.OOptions;

namespace AService
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
            FileInfo info;            

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml")) && File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xsd")))
            {
                configManager = new ConfigurationManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xsd"));
                info = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml"));
            }
            else if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json")))
            {
                configManager = new ConfigurationManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"), string.Empty);
                info = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
            }
            else
            {
                throw new ArgumentNullException($"Check for a config file");
            }
            Options op = configManager.GetParseOptions();
            logger = new Logger(op, info);
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
            private readonly Options options;
            private readonly FileInfo fileInfo;           
            public Logger(Options op, FileInfo info)
            {
                options = op;
                fileInfo = info;
                Operations.targetDirectoryPath = op.TargetPath;
                Operations.sourceDirectoryPath = op.SourcePath;
                Operations.IsForEncrypt = op.IsForEncrypt;
                Operations.IsForArchive = op.IsForArchive;

                watcher = new FileSystemWatcher(op.SourcePath);
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
                string fileEvent = "переименован в " + e.FullPath;
                string filePath = e.OldFullPath;
                RecordEntry(fileEvent, filePath);
            }         
            private void Watcher_Changed(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "изменен";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);                
            }            
            private void Watcher_Created(object sender, FileSystemEventArgs e)
            {              
                string fileEvent = "создан";
                string filePath = e.FullPath;                
                RecordEntry(fileEvent, filePath);      
            }                      
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
                    using (StreamWriter writer = new StreamWriter("D:\\Visual Studio\\TotalC#\\Lab2_C#(2sem)\\templog.txt", true))
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