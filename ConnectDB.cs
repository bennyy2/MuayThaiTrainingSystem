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
        List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

        List<JointType> legRight = new List<JointType> { JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight};

        List<JointType> handLeft = new List<JointType> { JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};

        List<JointType> handRight = new List<JointType> { JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight};

        OleDbConnection con;
        Vector vector = new Vector();

        public OleDbConnection connect()
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
            int s = (int) j;
            //List<String> v = new List<string>();

            try
            {
                con = connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT axis_x, axis_y, axis_z FROM [Position] WHERE jointID = "+s;
                //cmd.Parameters.AddWithValue("@j", j);
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

        public string testGetJointPosition(JointType j)
        {
            string aa = "test";

            int a = (int)JointType.ElbowLeft;
            try
            {
                con = connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT axis_x FROM [Position] WHERE jointID = "+a;
                //cmd.Parameters.Add(new OleDbParameter("@joint", 2));
                //cmd.Parameters.AddWithValue("@j", j);
                //cmd.Parameters.AddWithValue("@poseID", 7);
                //cmd.Parameters.AddWithValue("@modeID", 2);
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    aa = reader["axis_x"].ToString();
                    //v.Add(reader["axis_x"].ToString());
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
            return aa;

        }

        public Boolean saveSkel(Skeleton skel)
        {
            Boolean result = false;
            List<List<JointType>> li = new List<List<JointType>> { legLeft, legRight, handLeft, handRight};
            try
            {
                con = connect();
                con.Open();
                foreach (List<JointType> j in li)
                {
                    
                    foreach (JointType i in  j)
                    {

                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandText = "insert into [Position]([axis_x], [axis_y], [axis_z], [poseID], [classID], [jointID]) Values(@axis_x, @axis_y, @axis_z, @poseID, @classID, @jointID)";
                        cmd.Parameters.AddWithValue("@axis_x", skel.Joints[i].Position.X);
                        cmd.Parameters.AddWithValue("@axis_y", skel.Joints[i].Position.Y);
                        cmd.Parameters.AddWithValue("@axis_z", skel.Joints[i].Position.Z);
                        cmd.Parameters.AddWithValue("@poseID", 7);
                        cmd.Parameters.AddWithValue("@classID", 2);
                        cmd.Parameters.AddWithValue("@jointID", i);
                        cmd.Connection = con;
                        int a = cmd.ExecuteNonQuery();
                        Console.WriteLine(i+" X: " + skel.Joints[i].Position.X + " Y: " + skel.Joints[i].Position.Y);
                    }
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
