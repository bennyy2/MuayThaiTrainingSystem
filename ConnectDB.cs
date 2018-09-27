using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Configuration;
using System.Windows.Documents;

namespace MuayThaiTraining
{
    class ConnectDB
    {
        OleDbConnection con;

        private OleDbConnection connect()
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["MuayThaiDBConnectionString"].ToString();
            return con;
        }


        //string connectionString = "Data Source=DESKTOP-ODAF6RA;Initial Catalog=MuayThaiDB;Integrated Security=True;Pooling=False;";
        public void getDB(String name)
        {
            
            try
            {
                con = connect();
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

        public List<String> getListMode()
        {
            List<String> listMode = new List<String>();

            try
            {
                con = connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT [modeName] from [mode]";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var dataInTable = Convert.ToString(reader["modeName"].ToString());
                    listMode.Add(dataInTable);
                }
                return listMode;    
                //con = connect();
                //con.Open();
                //OleDbCommand cmd = new OleDbCommand();
                //String sqlQuery = "SELECT [modeName] from [mode]";
                //cmd = new OleDbCommand(sqlQuery, con);
                //cmd.CommandText = sqlQuery;
                ////cmd.Parameters.Add("@CallerName", OleDbType.VarChar).Value = labelProblemDate.Text.Trim();
                ////cmd.Parameters.AddWithValue("@name", name);
                ////cmd.Parameters["@CallerName"].Value = name;
                //cmd.ExecuteNonQuery();
                //cmd.Connection = con;
                //int a = cmd.ExecuteNonQuery();

                //while (row_reader.read()) int rows = row_reader.GetInt32(0);
            }
            catch (Exception)
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
