namespace AService.XmlOptions
{
    class Options
    {
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }        
        public string Command { get; set; }        
        public Options(string sourcePath, string targetPath, string command)
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;                    
            Command = command;                    
        }
    }
}
