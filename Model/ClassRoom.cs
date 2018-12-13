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
        int classID;

        public ClassRoom(){}
        public ClassRoom(string className)
        {
            ClassName = className;
        }

        public string ClassName { get => className; set => className = value; }
        public int ClassID { get => classID; set => classID = value; }

        public List<ClassRoom> getClassRoom()
        {
            List<ClassRoom> listClass = new List<ClassRoom>();

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT className FROM [ClassRoom]" +
                    " ORDER BY classId";
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

        public Boolean addClass(String className)
        {
            bool result = false;

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = "insert into ClassRoom ([className]) values (?)";
                cmd.Parameters.AddWithValue("@classname", className);
                cmd.Connection = con;
                int a = cmd.ExecuteNonQuery();
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

        public int getClassId(String room)
        {
            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT classId " +
                    "FROM ClassRoom " +
                    "WHERE className = @room";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@room", room);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    classID = (int)reader["classId"];
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
            return classID;
        }

    }
}
