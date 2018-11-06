using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;

namespace MuayThaiTraining.Model
{
    public class ClassRoom
    {
        ConnectDB connectDB = new ConnectDB();
        OleDbConnection con = new OleDbConnection();

        String className;

        public ClassRoom(){}
        public ClassRoom(string className)
        {
            ClassName = className;
        } 

        public string ClassName { get => className; set => className = value; }

        public List<ClassRoom> getClassRoom()
        {
            List<ClassRoom> listClass = new List<ClassRoom>();

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT className FROM [ClassRoom]";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ClassRoom classRoom = new ClassRoom(reader["className"].ToString());
                    listClass.Add(classRoom);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return listClass;
        }

    }
}
