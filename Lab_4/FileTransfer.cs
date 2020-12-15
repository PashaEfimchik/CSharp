using System.IO;

namespace FileWatcherService
{
    public class FileTransfer
    {
        string ftpFold;

        string outFold;

        public void SendFileToFtp(string fileName)
        {
            if (File.Exists(Path.Combine(ftpFold, fileName)))
            {
                File.Delete(Path.Combine(ftpFold, fileName));
            }
            File.Copy(Path.Combine(outFold, fileName), Path.Combine(ftpFold, fileName));
        }

        public FileTransfer(string outFold, string ftpFold)
        {
            this.ftpFold = ftpFold;

            this.outFold = outFold;
        }
    }
}
