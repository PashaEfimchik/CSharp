using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public partial class Options
    {
        public string targetPath { get; set; }
        public string sourcePath { get; set; }
        public string templogPath { get; set; }
        public bool encryptOptions { get; set; }
        public bool archiveOptions { get; set; }
        public Options(string _targetPath, string _sourcePath, string _templogPath, bool _encrypt, bool _archive)
        {
            targetPath = _targetPath;
            sourcePath = _sourcePath;
            templogPath = _templogPath;
            encryptOptions = _encrypt;
            archiveOptions=_archive;
        }

    }
}
