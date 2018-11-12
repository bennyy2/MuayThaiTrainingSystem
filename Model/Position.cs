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

        public Position(){}
        public Position(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }

        public Boolean saveSkel(Skeleton skel, string classname)
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
                        cmd.CommandText = "insert into [Position]([axis_x], [axis_y], [axis_z], [poseID], [classID], [jointID]) Values(@axis_x, @axis_y, @axis_z, @poseID, @classID, @jointID)";
                        cmd.Parameters.AddWithValue("@axis_x", skel.Joints[i].Position.X);
                        cmd.Parameters.AddWithValue("@axis_y", skel.Joints[i].Position.Y);
                        cmd.Parameters.AddWithValue("@axis_z", skel.Joints[i].Position.Z);
                        cmd.Parameters.AddWithValue("@poseID", poseid);
                        cmd.Parameters.AddWithValue("@classID", classid);
                        cmd.Parameters.AddWithValue("@jointID", i);
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

        public Point3D getPosition(JointType j, string poseName, string classRoom)
        {
            int s = (int)j;
            Point3D point = new Point3D();

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT p.axis_x, p.axis_y, p.axis_z " +
                    "FROM Position p " +
                    "INNER JOIN ClassRoom c " +
                    "ON p.classID = c.classId " +
                    "INNER JOIN Pose s " +
                    "ON s.poseID = p.poseID " +
                    "WHERE c.className = @room " +
                    "AND s.poseName = @poseName " +
                    "AND p.jointID = @joint";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);
                cmd.Parameters.AddWithValue("@joint", s);
                cmd.CommandType = System.Data.CommandType.Text;

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //v.Add(reader["axis_x"].ToString());
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

    }
}
