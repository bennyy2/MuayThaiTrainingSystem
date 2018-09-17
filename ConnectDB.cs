using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Configuration;

namespace MuayThaiTraining
{
    class ConnectDB
    {
        //string connectionString = "Data Source=DESKTOP-ODAF6RA;Initial Catalog=MuayThaiDB;Integrated Security=True;Pooling=False;";
        public void getDB(String name)
        {
            OleDbConnection con = new OleDbConnection();
            try
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["MuayThaiDBConnectionString"].ToString();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = "insert into [Joint](jointName) Values(@name)";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Connection = con;
                int a = cmd.ExecuteNonQuery();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            

        }





        //public void connect()
        //{
        //    string provider = ConfigurationManager.AppSettings["provider"];
        //    string connectionString = ConfigurationManager.AppSettings["connectionString"];

        //    DbProviderFactory factory = DbProviderFactories.GetFactory(provider);

        //    using (DbConnection connection = factory.CreateConnection())
        //    {
        //        if (connection == null)
        //        {
        //            Console.WriteLine("Connection Error");
        //            Console.ReadLine();
        //            return;
        //        }

        //        connection.ConnectionString = connectionString;
        //        connection.Open();
        //        DbCommand command = factory.CreateCommand();

        //        if (command == null)
        //        {
        //            Console.WriteLine("Command Error");
        //            Console.ReadLine();
        //            return;
        //        }

        //        command.Connection = connection;
        //        command.CommandText = "Select * From Joint";

        //        using (DbDataReader dataReader = command.ExecuteReader())
        //        {
        //            while (dataReader.Read())
        //            {
        //                Console.WriteLine($"{ dataReader["jointName"]}");
        //            }
        //        }

        //        Console.ReadLine();


        //    }

        //}




    }
}
