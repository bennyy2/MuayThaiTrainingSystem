using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using Microsoft.Kinect;

namespace MuayThaiTraining
{
    class Vector
    {
        double x;
        double y;
        double z;

        //List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
        //    JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

        //List<JointType> legRight = new List<JointType> { JointType.HipRight,
        //    JointType.KneeRight, JointType.AnkleRight, JointType.FootRight};

        //List<JointType> handLeft = new List<JointType> { JointType.Spine, JointType.ShoulderCenter,
        //    JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};

        //List<JointType> handRight = new List<JointType> { JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };

        OleDbConnection con;
        ConnectDB connectDB = new ConnectDB();


        public Vector() { }

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }

        //public Boolean saveSkel(Skeleton skel)
        //{
        //    Boolean result = false;
        //    List<List<JointType>> li = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };
        //    try
        //    {
        //        con = connectDB.connect();
        //        con.Open();
        //        foreach (List<JointType> j in li)
        //        {

        //            foreach (JointType i in j)
        //            {

        //                OleDbCommand cmd = new OleDbCommand();
        //                cmd.CommandText = "insert into [Position]([axis_x], [axis_y], [axis_z], [poseID], [classID], [jointID]) Values(@axis_x, @axis_y, @axis_z, @poseID, @classID, @jointID)";
        //                cmd.Parameters.AddWithValue("@axis_x", skel.Joints[i].Position.X);
        //                cmd.Parameters.AddWithValue("@axis_y", skel.Joints[i].Position.Y);
        //                cmd.Parameters.AddWithValue("@axis_z", skel.Joints[i].Position.Z);
        //                cmd.Parameters.AddWithValue("@poseID", 7);
        //                cmd.Parameters.AddWithValue("@classID", 2);
        //                cmd.Parameters.AddWithValue("@jointID", i);
        //                cmd.Connection = con;
        //                int a = cmd.ExecuteNonQuery();
        //                Console.WriteLine(i + " X: " + skel.Joints[i].Position.X + " Y: " + skel.Joints[i].Position.Y);
        //            }
        //        }

        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;

        //}

    }
}
