using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using Microsoft.Kinect;
using MuayThaiTraining.Model;
using System.Windows.Media.Media3D;

namespace MuayThaiTraining
{
    class Position
    {
        double x;
        double y;
        double z;
        int joint;
        int frame;

        ConnectDB connectDB = new ConnectDB();
        OleDbConnection con = new OleDbConnection();
        ClassRoom classRoom = new ClassRoom();
        Pose pose = new Pose();


        List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

        List<JointType> legRight = new List<JointType> { JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight};

        List<JointType> handLeft = new List<JointType> { JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};

        List<JointType> handRight = new List<JointType> { JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };

        List<JointType> body = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft, JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight, JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft,
            JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };

        public Position(){}
        public Position(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Position(double x, double y, double z, int joint, int frame)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Joint = joint;
            this.Frame = frame;
        }
        

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }
        public int Joint { get => joint; set => joint = value; }
        public int Frame { get => frame; set => frame = value; }

        public Boolean saveSkel(Skeleton skel, string classname, int frame)
        {
            Boolean result = false;
            List<List<JointType>> li = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };
            try
            {
                con = connectDB.connect();
                con.Open();
                int classid = classRoom.getClassId(classname);
                int poseid = pose.getPoseId();

                foreach (List<JointType> j in li)
                {

                    foreach (JointType i in j)
                    {

                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandText = "insert into [JointPosition]([axis_x], [axis_y], [axis_z], [poseID], [classID], [jointID], [frameNo]) Values(@axis_x, @axis_y, @axis_z, @poseID, @classID, @jointID, @frameNo)";
                        cmd.Parameters.AddWithValue("@axis_x", skel.Joints[i].Position.X);
                        cmd.Parameters.AddWithValue("@axis_y", skel.Joints[i].Position.Y);
                        cmd.Parameters.AddWithValue("@axis_z", skel.Joints[i].Position.Z);
                        cmd.Parameters.AddWithValue("@poseID", poseid);
                        cmd.Parameters.AddWithValue("@classID", classid);
                        cmd.Parameters.AddWithValue("@jointID", i);
                        cmd.Parameters.AddWithValue("@frameNo", frame);
                        cmd.Connection = con;
                        int a = cmd.ExecuteNonQuery();
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

        
        public Point3D getPosition(JointType j, string poseName, string classRoom, int frame)
        {
            int s = (int)j;
            Point3D point = new Point3D();


            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT axis_x, axis_y, axis_z " +
                    "FROM ((JointPosition " +
                    "INNER JOIN ClassRoom ON ClassRoom.classId = JointPosition.classID) " +
                    "INNER JOIN Pose ON Pose.poseID = JointPosition.poseID) " +
                    "WHERE ClassRoom.className = @room " +
                    "AND Pose.poseName = @poseName " +
                    "AND frameNo = @frame " +
                    "AND jointID = "+s;
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);
                cmd.Parameters.AddWithValue("@frame", frame);

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    point = new Point3D((double)reader["axis_x"], (double)reader["axis_y"], (double)reader["axis_z"]);
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
            return point;

        }

        public Boolean saveMotionPoint(List<BodyJoint> bodyJoint, string classname)
        {
            Boolean result = false;
            try
            {
                con = connectDB.connect();
                con.Open();
                int classid = classRoom.getClassId(classname);
                int poseid = 18;

                foreach (BodyJoint i in bodyJoint)
                {

                    foreach (JointType j in body)
                    {

                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandText = "insert into [JointPosition]([axis_x], [axis_y], [axis_z], [poseID], [classID], [jointID], [frameNo]) Values(@axis_x, @axis_y, @axis_z, @poseID, @classID, @jointID, @frameNo)";
                        cmd.Parameters.AddWithValue("@axis_x", i.Skel.Joints[j].Position.X);
                        cmd.Parameters.AddWithValue("@axis_y", i.Skel.Joints[j].Position.Y);
                        cmd.Parameters.AddWithValue("@axis_z", i.Skel.Joints[j].Position.Z);
                        cmd.Parameters.AddWithValue("@poseID", poseid);
                        cmd.Parameters.AddWithValue("@classID", classid);
                        cmd.Parameters.AddWithValue("@jointID", j);
                        cmd.Parameters.AddWithValue("@frameNo", i.Frame);
                        cmd.Connection = con;
                        int a = cmd.ExecuteNonQuery();
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

        public List<Position> getMotionByFrame(string poseName, string classRoom, int frame)
        {
            List<Position> trainerMotion = new List<Position>();

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT axis_x, axis_y, axis_z, jointID, frameNo " +
                    "FROM ((JointPosition " +
                    "INNER JOIN ClassRoom ON ClassRoom.classId = JointPosition.classID) " +
                    "INNER JOIN Pose ON Pose.poseID = JointPosition.poseID) " +
                    "WHERE ClassRoom.className = @room " +
                    "AND Pose.poseName = @poseName ";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Position position = new Position((double)reader["axis_x"], (double)reader["axis_y"], (double)reader["axis_z"], (int)reader["jointID"], (int)reader["frameNo"]);
                    trainerMotion.Add(position);
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

            return trainerMotion;

        }



        public List<Position> getMotion(string poseName, string classRoom)
        {
            List<Position> trainerMotion = new List<Position>();

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT axis_x, axis_y, axis_z, jointID, frameNo " +
                    "FROM ((JointPosition " +
                    "INNER JOIN ClassRoom ON ClassRoom.classId = JointPosition.classID) " +
                    "INNER JOIN Pose ON Pose.poseID = JointPosition.poseID) " +
                    "WHERE ClassRoom.className = @room " +
                    "AND Pose.poseName = @poseName ";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Position position = new Position((double)reader["axis_x"], (double)reader["axis_y"], (double)reader["axis_z"], (int)reader["jointID"], (int)reader["frameNo"]);
                    trainerMotion.Add(position);
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

            return trainerMotion;

        }

        public int lenghtFrame(string poseName, string classRoom)
        {
            int len = 0;
            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT MAX(frameNo) as maxFrame " +
                    "FROM ((JointPosition " +
                    "INNER JOIN ClassRoom ON ClassRoom.classId = JointPosition.classID) " +
                    "INNER JOIN Pose ON Pose.poseID = JointPosition.poseID) " +
                    "WHERE ClassRoom.className = @room " +
                    "AND Pose.poseName = @poseName ";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    len = (int)reader["maxFrame"];
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

            return len;

        }

    }
}
