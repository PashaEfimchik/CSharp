using System;
using System.IO;
using System.ServiceProcess;

namespace FileWatcherService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            string DirectoryPath = $"{AppDomain.CurrentDomain.BaseDirectory}";
            try
            {
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(DirectoryPath, "textlog.txt"), true))
                {
                    writer.WriteLine("\n--------\n" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss ") + ex.Message + "\n---------\n");
                    writer.Flush();
                }
            }
        }
    }
}
