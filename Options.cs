namespace AService.OOptions
{
    class Options
    {
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string TemplogPath { get; set; }
        public bool IsForEncrypt { get; set; }
        public bool IsForArchive { get; set; }
        public Options(string _sourcePath, string _targetPath, string _loggerPath, bool _isForEncrypt, bool _isForArchive)
        {
            SourcePath = _sourcePath;
            TargetPath = _targetPath;
            TemplogPath = _loggerPath;
            IsForEncrypt = _isForEncrypt;
            IsForArchive = _isForArchive;
        }
    }
}
