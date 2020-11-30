using System;
using AService.OOptions;
using System.IO;
using Newtonsoft.Json;

namespace AService.Parsers
{
    class JSParser : IParser
    {
        private string jsPath { get; }
        public JSParser(string _jsPath) => jsPath = _jsPath;
        public Options GetParseOptions()
        {
            try
            {                
                Options op = JSDeserialize();
                return new Options(op.SourcePath, op.TargetPath, op.TemplogPath, op.IsForEncrypt, op.IsForArchive);
            }
            catch
            {
                throw new Exception(@"JSParse.Exception (");
            }
        }
        public Options JSDeserialize()
        {
            try
            {
                Options op = null;
                JsonSerializer js = new JsonSerializer();
                if (File.Exists(jsPath))
                {
                    StreamReader sr = new StreamReader(jsPath);
                    JsonReader jsReader = new JsonTextReader(sr);
                    op = js.Deserialize(jsReader) as Options;
                    jsReader.Close();
                    sr.Close();                    
                }
                return op;
            }
            catch
            {
                throw new Exception(@"Error in js deserialize");
            }
        }
    }
}
