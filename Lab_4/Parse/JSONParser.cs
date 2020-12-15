using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public partial class JsonParser : IParse
    {
        string path { get; }
        string configPath { get; }

        public JsonParser(string _path, string _configPath)
        {
            path = _path;
            configPath = _configPath;
        }

        public Options ParsersOptions()
        {
            try
            {
                JsonOptions jsopts;
                string jsfile;
                using(StreamReader streamReader = new StreamReader(path))
                {
                    jsfile = streamReader.ReadToEnd();
                    jsopts = JsonSerializer.Deserialize<JsonOptions>(jsfile);
                }
                return new Options(jsopts.targetPath, jsopts.sourcePath, jsopts.templogPath, jsopts.encryptOptions, jsopts.archiveOptions);
            }
            catch
            {
                throw new Exception("JSON Parse error");
            }
            
        }
    }
}
