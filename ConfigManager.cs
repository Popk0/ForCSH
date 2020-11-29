using System.IO;
using AService.OOptions;
using AService.Parsers;

namespace AService
{
    class ConfigurationManager
    {
        readonly IParser parser;
        public ConfigurationManager(string path, string config)
        {
            if (Path.GetExtension(path) == ".xml")
            {
                parser = new XMLParser(path, config);
            }            
            parser = new JSParser(path);                        
        }
        public Options GetParseOptions() => parser.GetParseOptions();
    }
}
