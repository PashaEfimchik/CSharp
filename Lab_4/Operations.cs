using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class Operations
    {
        internal static string targetPath;
        internal static string sourcePath;
        internal static bool encryptOpt;
        internal static bool archiveOpt;

        static internal void OnFileUpdated(object sender, FileSystemEventArgs e)
        {
            FileUpdated(e.Name);
        }

        static private void FileUpdated(string name)
        {
            try
            {
                Service1.logger.watcher.EnableRaisingEvents = false;
                if (encryptOpt)
                {
                    Encrypt(sourcePath, name + ".gz");
                    Decrypt(targetPath, name + ".gz");
                }
                if (archiveOpt)
                {
                    Zip(sourcePath, sourcePath, name);
                    Unzip(targetPath, targetPath, name + ".gz");
                    Delete(targetPath, name + ".gz");
                }
                Service1.logger.watcher.EnableRaisingEvents = true;

                Move(sourcePath, targetPath, name + ".gz");

            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(targetPath, "textlog.txt"), true))
                {
                    writer.WriteLine("\n---\n" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss ") + ex.Message + "\n---\n");
                    writer.Flush();
                }
            }
        }

        static private void Zip(string sourcePath, string targetPath, string name)
        {
            var buf = new StringBuilder(name);
            buf.Append(".gz");

            using (var targetStream = File.Create(Path.Combine(targetPath, buf.ToString())))

            {
                using (var sourceStream = new FileStream(Path.Combine(sourcePath, name), FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        static void Unzip(string sourceDirectoryPath, string targetDirectoryPath, string name)
        {
            var buf = name.Substring(0, name.Length - 3);

            using (var targetStream = File.Create(Path.Combine(targetDirectoryPath, buf)))
            {
                using (var sourceStream = File.OpenRead(Path.Combine(sourceDirectoryPath, name)))
                {
                    using (var decomressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decomressionStream.CopyTo(targetStream);
                    }
                }
            }
        }

        static private void Move(string sourcePath, string targetPath, string name)
        {
            if (File.Exists(Path.Combine(targetPath, name)))
            {
                Delete(targetPath, name);
            }

            File.Move(Path.Combine(sourcePath, name), Path.Combine(targetPath, name));
        }

        static private void Delete(string filePath, string name)
        {
            var buf = Path.Combine(filePath, name);

            File.Delete(buf);
        }

        static private void Encrypt(string filePath, string name)
        {
            File.Encrypt(Path.Combine(filePath, name));
        }

        static private void Decrypt(string filePath, string name)
        {
            File.Decrypt(Path.Combine(filePath, name));
        }
    }
}
