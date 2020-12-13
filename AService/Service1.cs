using System;
using System.ServiceProcess;
using System.IO;
using System.Threading;
using AService.XmlOptions;

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
            else
            {
                throw new ArgumentNullException($"No config file");
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

                Operations.sourceDirectoryPath = op.SourcePath;
                Operations.targetDirectoryPath = op.TargetPath;
                Operations.command = op.Command;           

                watcher = new FileSystemWatcher(op.SourcePath);                
                watcher.Created += Operations.OnFileUpdated;                
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
            
        }
    }
}