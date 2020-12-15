using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public partial class DataManager : ServiceBase
    {
        readonly DataIO appIn;

        readonly DataOptions dataOptions;

        public DataManager(DataOptions dataOptions, DataIO appIn)
        {
            InitializeComponent();

            this.dataOptions = dataOptions;

            this.appIn = appIn;
        }

        protected override void OnStart(string[] args)
        {
            DataIO reader = new DataIO(dataOptions.ConnectionString);

            FileTransfer fileTransfer = new FileTransfer(dataOptions.TargetFolder, dataOptions.SourcePath);

            string customersFileName = "person";

            reader.GetCustomers(dataOptions.TargetFolder, appIn, customersFileName);

            fileTransfer.SendFileToFtp($"{customersFileName}.xml");
            fileTransfer.SendFileToFtp($"{customersFileName}.xsd");

            appIn.InsertIn("Files successfully sent");
        }

        protected override void OnStop()
        {
            appIn.InsertIn("Service stopped!");
        }
    }
}
