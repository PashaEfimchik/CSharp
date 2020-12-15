using System.Data;
using System.IO;

namespace FileWatcherService
{
    public class XmlGenerator
    {
        public string CreateXml(DataTable dataT, string xmlDirectory, string xmlName)
        {
            string xmlFilePath = Path.Combine(xmlDirectory, xmlName + ".xml");
            dataT.WriteXml(xmlFilePath);

            string xsdFilePath = Path.Combine(xmlDirectory, xmlName + ".xsd");
            dataT.WriteXmlSchema(xsdFilePath);

            return xmlFilePath;
        }
    }
}
