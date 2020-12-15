using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace FileWatcherService
{
    public class DataIO
    {
        readonly string connectionString;

        public DataIO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InsertIn(string message)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = new SqlCommand("AddInsight", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                try
                {
                    SqlParameter messageParam = new SqlParameter
                    {
                        ParameterName = "@message",
                        Value = message
                    };

                    SqlParameter timeParam = new SqlParameter
                    {
                        ParameterName = "@time",
                        Value = DateTime.Now
                    };

                    command.Parameters.AddRange(new[] { messageParam, timeParam });

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exceptions.txt"), true))
                    {
                        sw.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} 2Exception: {ex.Message}");
                    }

                    transaction.Rollback();
                }
            }
        }

        public void ClearInsights()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();

                SqlCommand command = new SqlCommand("ClearInsight", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = trans;

                try
                {
                    command.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exceptions.txt"), true))
                    {
                        sw.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} Exception: {ex.Message}");
                    }

                    trans.Rollback();
                }
            }
        }

        public void GetCustomers(string outputFolder, DataIO appInsights, string customersFileName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = new SqlCommand("GetPerson", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataSet dataSet = new DataSet("Person");

                    DataTable dataTable = new DataTable("Person");

                    dataSet.Tables.Add(dataTable);

                    adapter.Fill(dataSet.Tables["Person"]);

                    XmlGenerator xmlGenerator = new XmlGenerator(outputFolder);

                    xmlGenerator.WriteToXml(dataSet, customersFileName);

                    appInsights.InsertIn("Message successfully received");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    appInsights.InsertIn("Exception: " + ex.Message);

                    transaction.Rollback();
                }
            }
        }

        public void WriteInsightsToXml(string outputFolder)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = new SqlCommand("GetInsight", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataSet dataSet = new DataSet("Insights");

                    DataTable dataTable = new DataTable("Insight");

                    dataSet.Tables.Add(dataTable);

                    adapter.Fill(dataSet.Tables["Insight"]);

                    XmlGenerator xmlGenerator = new XmlGenerator(outputFolder);

                    xmlGenerator.WriteToXml(dataSet, "appIn");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exceptions.txt"), true))
                    {
                        sw.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} 3Exception: {ex.Message}");
                    }

                    transaction.Rollback();
                }
            }
        }
    }
}
