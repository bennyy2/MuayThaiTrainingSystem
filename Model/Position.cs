using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using Microsoft.Kinect;
using MuayThaiTraining.Model;
using System.Windows.Media.Media3D;
using System.Configuration;

namespace MuayThaiTraining
{
    public class Position
    {
        double x;
        double y;
        double z;
        int imx;
        int imy;
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

        public Position(double x, double y, double z, int imx, int imy, int joint, int frame)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Imx = imx;
            this.Imy = imy;
            this.Joint = joint;
            this.Frame = frame;
        }
        

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }
        public int Imx { get => imx; set => imx = value; }
        public int Imy { get => imy; set => imy = value; }
        public int Joint { get => joint; set => joint = value; }
        public int Frame { get => frame; set => frame = value; }

        public Boolean saveSkel(List<Position> skel, string classname, int frame)
        {
            Boolean result = false;
            try
            {
                con = connectDB.connect();
                con.Open();
                int classid = classRoom.getClassId(classname);
                int poseid = pose.getPoseId();

                var f = skel.Max(x => x.Frame);
                var body = skel.Where(x=>x.Frame == f).ToList();

                foreach (var i in body)
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "insert into [Data]([X], [Y], [Z], [imX], [imY], [poseID], [classID], [jointID], [frame]) Values(@x, @y, @z, @imx, @imy, @poseID, @classID, @jointID, @frame)";
                    cmd.Parameters.AddWithValue("@x", i.X);
                    cmd.Parameters.AddWithValue("@y", i.Y);
                    cmd.Parameters.AddWithValue("@z", i.Z);
                    cmd.Parameters.AddWithValue("@imx", i.Imx);
                    cmd.Parameters.AddWithValue("@imy", i.Imy);
                    cmd.Parameters.AddWithValue("@poseID", poseid);
                    cmd.Parameters.AddWithValue("@classID", classid);
                    cmd.Parameters.AddWithValue("@jointID", i.Joint);
                    cmd.Parameters.AddWithValue("@frameNo", 0);
                    cmd.Connection = con;
                    int a = cmd.ExecuteNonQuery();
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
            using (OleDbConnection conn = new OleDbConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MuayThaiDBConnectionString"].ToString();

                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT JP.axis_x, JP.axis_y, JP.axis_z " +
                    "FROM ((JointPosition AS JP " +
                    "INNER JOIN ClassRoom AS CR ON CR.classId = JP.classID) " +
                    "INNER JOIN Pose AS P ON P.poseID = JP.poseID) " +
                    "WHERE CR.className = @room " +
                    "AND P.poseName = @poseName " +
                    "AND JP.frameNo = @frame " +
                    "AND JP.jointID = " + s;
                cmd = new OleDbCommand(sqlQuery, conn);
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
            
            return point;

        }

        

        public Boolean saveMotionPoint(List<Position> bodyJoint, string classname)
        {
            Boolean result = false;
            try
            {
                con = connectDB.connect();
                con.Open();
                int classid = classRoom.getClassId(classname);
                int poseid = pose.getPoseId();

                foreach (var i in bodyJoint)
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "insert into [Data]([X], [Y], [Z], [imX], [imY], [poseID], [classID], [jointID], [frame]) Values(@x, @y, @z, @imx, @imy, @poseID, @classID, @jointID, @frame)";
                    cmd.Parameters.AddWithValue("@x", i.X);
                    cmd.Parameters.AddWithValue("@y", i.Y);
                    cmd.Parameters.AddWithValue("@z", i.Z);
                    cmd.Parameters.AddWithValue("@imx", i.Imx);
                    cmd.Parameters.AddWithValue("@imy", i.Imy);
                    cmd.Parameters.AddWithValue("@poseID", poseid);
                    cmd.Parameters.AddWithValue("@classID", classid);
                    cmd.Parameters.AddWithValue("@jointID", i.Joint);
                    cmd.Parameters.AddWithValue("@frameNo", i.Frame);
                    cmd.Connection = con;
                    int a = cmd.ExecuteNonQuery();
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
        

        public List<Position> getMotionByFrame(string classRoom, string poseName)
        {
            List<Position> trainerMotion = new List<Position>();

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT X, Y, Z, imX, imY, jointID, frame " +
                    "FROM ((Data " +
                    "INNER JOIN ClassRoom ON ClassRoom.classId = Data.classID) " +
                    "INNER JOIN Pose ON Pose.poseID = Data.poseID) " +
                    "WHERE ClassRoom.className = @room " +
                    "AND Pose.poseName = @poseName ";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Position position = new Position((double)reader["X"], (double)reader["Y"], (double)reader["Z"], (int)reader["imX"], (int)reader["imY"], (int)reader["jointID"], (int)reader["frame"]);
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
                String sqlQuery = "SELECT X, Y, Z, imX, imY,jointID, frame " +
                    "FROM ((Data " +
                    "INNER JOIN ClassRoom ON ClassRoom.classId = Data.classID) " +
                    "INNER JOIN Pose ON Pose.poseID = Data.poseID) " +
                    "WHERE ClassRoom.className = @room " +
                    "AND Pose.poseName = @poseName ";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Position position = new Position((double)reader["X"], (double)reader["Y"], (double)reader["Z"], (int)reader["imX"], (int)reader["imY"], (int)reader["jointID"], (int)reader["frame"]);
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
