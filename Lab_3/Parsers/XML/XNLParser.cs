using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FileWatcherService
{
    class XmlParser : IParse
    {
        string configPath { get; }
        string path { get; }
        public XmlParser(string _path, string _configPath)
        {
            path = _path;
            configPath = _configPath;
        }

        public Options ParsersOptions()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(path);
                xmlDocument.Schemas.Add("", configPath);
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                xmlDocument.Validate(eventHandler);

                var opts = XMLDeserialize(path);
                return new Options(opts.sourcePath, opts.targetPath, opts.templogPath, opts.encryptOptions, opts.archiveOptions);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public Options XMLDeserialize(string _path)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Options));
                
                using (FileStream fileStream = new FileStream(_path, FileMode.Open))
                {
                    return (Options)xmlSerializer.Deserialize(fileStream);
                }
            }
            catch
            {
                throw new Exception("Failed to deserialize this XML file");
            }
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType xmlSeverityType = XmlSeverityType.Warning;
            if (!Enum.TryParse<XmlSeverityType>("Error", out xmlSeverityType))
            {
                if (xmlSeverityType == XmlSeverityType.Error)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
