using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
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
                //String sqlQuery = "SELECT [modeName] from [mode]";
                //con = new SqlConnection(connectionString);
                //con.Open();
                //cmd = new SqlCommand(sqlQuery, con);
                //reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    var dataInTable = Convert.ToString(reader["modeName"].ToString());
                //    listMode.Add(dataInTable);
                //}
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

    }
}
