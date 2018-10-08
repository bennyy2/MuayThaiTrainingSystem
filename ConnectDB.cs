using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace MuayThaiTraining
{
    class ConnectDB
    {
        OleDbConnection con;
        Vector vector = new Vector();

        private OleDbConnection connect()
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["MuayThaiDBConnectionString"].ToString();
            return con;
        }


        

        public void savePosition(Skeleton skeletons, String name, String des)
        {
            try
            {
                con = connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = "insert into Pose ([poseName], [poseDescription], [jointID], [modeID]) values (?,?,?,?)";
                cmd.Parameters.AddWithValue("@poseName", name);
                cmd.Parameters.AddWithValue("@poseDescrition", des);
                cmd.Parameters.AddWithValue("@jointID", 1);
                cmd.Parameters.AddWithValue("@modeID", 1);
                cmd.Connection = con;
                int a = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            //double x = skeletons.Joints[JointType.HipCenter].Position.Y;
            //Console.WriteLine(x);
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

        //public List<String> getListMode()
        //{
        //    List<String> listMode = new List<String>();

        //    try
        //    {
        //        con = connect();
        //        con.Open();
        //        OleDbCommand cmd = new OleDbCommand();
        //        String sqlQuery = "SELECT [modeName] from [mode]";
        //        cmd = new OleDbCommand(sqlQuery, con);
        //        cmd.CommandType = System.Data.CommandType.Text;

        //        OleDbDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            var dataInTable = reader["modeName"].ToString();
        //            listMode.Add(dataInTable);
        //        }
        //        return listMode;    

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        con.Close();
                
        //    }
            
        //}

        public Vector getJointPosition(JointType j)
        {
            String s = j.ToString();
            //List<String> v = new List<string>();

            try
            {
                con = connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT axis_x, axis_y, axis_z FROM [Position] WHERE jointID = 1";
                //cmd.Parameters.AddWithValue("@jointID", Int32.Parse("1"));
                //cmd.Parameters.AddWithValue("@poseID", 7);
                //cmd.Parameters.AddWithValue("@modeID", 2);
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //v.Add(reader["axis_x"].ToString());
                    vector = new Vector((double)reader["axis_x"], (double)reader["axis_y"], (double)reader["axis_z"]);
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
            return vector;

        }



    }
}
