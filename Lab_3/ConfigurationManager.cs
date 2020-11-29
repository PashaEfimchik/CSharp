using System;
using System.IO;


namespace FileWatcherService
{
    public partial class ConfigurationManager : IParse
    {
        IParse pars;
        public ConfigurationManager(string path, string config)
        {
            if (path.EndsWith(".xml"))
            {
                pars = new XmlParser(path, config);
            }
            else if (path.EndsWith(".json"))
            {
                pars = new JsonParser(path, config);
            }
            else
                throw new ArgumentNullException("Change other extension!");
        }

        public Options ParseOptions() => pars.ParsersOptions();

        public Options ParsersOptions()
        {
            throw new NotImplementedException();
        }
    }
}
