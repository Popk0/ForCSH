using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;
using AService.OOptions;

namespace AService.Parsers
{
    class XMLParser : IParser
    {
        private string xmlPath { get; }
        private string xsdPath { get; }
        public XMLParser(string _xmlPath, string _xsdPath)
        {
            xmlPath = _xmlPath;
            xsdPath = _xsdPath;
        }
        public Options GetParseOptions()
        {
            try
            {
                //XmlReaderSettings settings = new XmlReaderSettings();
                //settings.Schemas.Add("", "config.xsd");
                //settings.ValidationType = ValidationType.Schema;
                //XmlReader reader = XmlReader.Create("config.xml", settings);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                //xmlDoc.Load(reader);
                XPathNavigator navigator = xmlDoc.CreateNavigator();
                xmlDoc.Schemas.Add("", xsdPath);
                ValidationEventHandler validation = new ValidationEventHandler(SchemaValidationHandler);

                xmlDoc.Validate(validation);

                Options op = XMLDeserialize();
                return new Options(op.SourcePath, op.TargetPath, op.TemplogPath, op.IsForEncrypt, op.IsForArchive);
            }
            catch
            {
                throw new Exception(@"XMLParse.Exception (");
            }
        }
        static void SchemaValidationHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    throw new Exception(@"Schema Validation Error");                    
                case XmlSeverityType.Warning:
                    throw new Exception(@"Schema Validation Warning");                                        
            }
        }
        public Options XMLDeserialize()
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(Options));
                                
                using (FileStream fs = new FileStream(xmlPath, FileMode.OpenOrCreate))
                {
                    return (Options)xml.Deserialize(fs);
                }                
            }
            catch
            {
                throw new Exception(@"Error in xml deserialize");
            }
        }
    }
}
