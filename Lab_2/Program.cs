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
            string DirectoryPath = @"C:\Users\Павел\Desktop\Labs\3_sem\C#\Lab_2";
            try 
            {
            ServiceBase.Run(ServicesToRun);
            }
            catch(Exception e)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(DirectoryPath, "Exceptions.txt"), true))
                    {
                        sw.WriteLine($"Exception: ( {e.Message} ) ---------- {DateTime.Now: dd/MM/yyyy HH:mm:ss}");
                    }
            }
            }
    }
}
