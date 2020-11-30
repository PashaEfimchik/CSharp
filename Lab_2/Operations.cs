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
		internal static string targetDirectoryPath = @"C:\Users\Павел\Desktop\Labs\3_sem\C#\Lab_2\out";
		internal static string sourceDirectoryPath = @"C:\Users\Павел\Desktop\Labs\3_sem\C#\Lab_2\in";

		static internal void OnFileUpdated(object sender, FileSystemEventArgs e)
		{
			FileUpdated(e.Name);
		}

		static private void FileUpdated(string name)
		{
			try
			{
				//to zip
				Service1.logger.watcher.EnableRaisingEvents = false;
				Zip(sourceDirectoryPath, sourceDirectoryPath, name);
				Service1.logger.watcher.EnableRaisingEvents = true;

				//encrypt
				Encrypt(sourceDirectoryPath, name + ".gz");

				//move
				Move(sourceDirectoryPath, targetDirectoryPath, name + ".gz");

				//unzip
				Decrypt(targetDirectoryPath, name + ".gz");
				Unzip(targetDirectoryPath, targetDirectoryPath, name + ".gz");
				Delete(targetDirectoryPath, name + ".gz");
			}
			catch (Exception ex)
			{
				using (StreamWriter writer = new StreamWriter(Path.Combine(targetDirectoryPath, "textlog.txt"), true))
				{
					writer.WriteLine("\n================\n" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss ") + ex.Message + "\n================\n");
					writer.Flush();
				}
			}
		}

		static void Unzip(string sourceDirectoryPath, string targetDirectoryPath, string name)
		{
			var buf = name.Substring(0, name.Length - 3);//[..^3];

			using (var targetStream = File.Create(Path.Combine(targetDirectoryPath, buf)))
			using (var sourceStream = File.OpenRead(Path.Combine(sourceDirectoryPath, name)))
			using (var decomressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
			{
				decomressionStream.CopyTo(targetStream);
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

		static private void Zip(string sourcePath, string targetPath, string name)
		{
			var buf = new StringBuilder(name);
			buf.Append(".gz");

			using (var targetStream = File.Create(Path.Combine(targetPath, buf.ToString())))
			using (var sourceStream = new FileStream(Path.Combine(sourcePath, name), FileMode.OpenOrCreate, FileAccess.Read))
			using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
			{
				sourceStream.CopyTo(compressionStream);
			}
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
