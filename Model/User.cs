using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuayThaiTraining
{
    class User
    {
        ConnectDB connectDB = new ConnectDB();
        OleDbConnection con = new OleDbConnection();
        public string username { get; set; }
        public string password { get; set; }

        public User()
        {

        }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public Boolean checkUser(string user, string pass)
        {
            bool result = false;
            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT username FROM [User] Where username = @user and password = @pass";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    this.username = reader["username"].ToString();
                }
                result = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}
