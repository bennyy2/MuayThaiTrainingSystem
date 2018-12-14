using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;

namespace MuayThaiTraining.Model
{
    class Pose
    {
        ConnectDB connectDB = new ConnectDB();
        OleDbConnection con = new OleDbConnection();
        ClassRoom classRoom = new ClassRoom();

        string poseName;
        string poseDescription;
        int poseID;
        string type;

        public Pose(){}

        public Pose(int poseID, string poseName, string poseDescription, string type)
        {
            this.PoseID = poseID;
            this.PoseName = poseName;
            this.PoseDescription = poseDescription;
            this.Type = type;
        }

        public string PoseDescription { get => poseDescription; set => poseDescription = value; }
        public string PoseName { get => poseName; set => poseName = value; }
        public int PoseID { get => poseID; set => poseID = value; }
        public string Type { get => type; set => type = value; }

        public List<Pose> getPose(String room)
        {
            List<Pose> listPose = new List<Pose>();

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT p.poseID, p.poseName, p.poseDescription, p.type " +
                    "FROM Pose p " +
                    "INNER JOIN ClassRoom c " +
                    "ON p.classID = c.classId " +
                    "WHERE c.className = @room " +
                    "ORDER BY poseID";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@room", room);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pose pose = new Pose((int)reader["poseID"], reader["poseName"].ToString(), reader["poseDescription"].ToString(), reader["type"].ToString());
                    listPose.Add(pose);
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
            return listPose;
        }


        public Boolean savePoseDetail(String name, String des, String classname, String type)
        {
            bool result = false;

            try
            {
                con = connectDB.connect();
                con.Open();
                int id = classRoom.getClassId(classname);
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = "insert into Pose ([poseName], [poseDescription], [classID], [type]) values (?,?,?,?)";
                cmd.Parameters.AddWithValue("@poseName", name);
                cmd.Parameters.AddWithValue("@poseDescrition", des);
                cmd.Parameters.AddWithValue("@classID", id);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Connection = con;
                int a = cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        public int getPoseId()
        {
            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT top 1 poseID " +
                    "FROM Pose " +
                    "ORDER BY poseID DESC";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    poseID = (int)reader["poseID"];
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

            return poseID;
        }

        public String getPoseDescription(string poseName, string className)
        {
            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT poseDescription " +
                    "FROM Pose p " +
                    "INNER JOIN ClassRoom c " +
                    "ON (p.classID = c.classId) " +
                    "WHERE c.className = @classname " +
                    "AND p.poseName = @posename";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@classname", className);
                cmd.Parameters.AddWithValue("@posename", poseName);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    poseDescription = reader["poseDescription"].ToString();
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

            return poseDescription;
        }

    }
}
