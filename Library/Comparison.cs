using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using MathNet.Numerics;
//using MathNet.Numerics.LinearAlgebra;
//using MathNet.Numerics.LinearAlgebra.Double;
//using MathNet.Numerics.Data.Text;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;
using MuayThaiTraining.Model;

namespace MuayThaiTraining
{
    class Comparison
    {
        
        List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

        List<JointType> legRight = new List<JointType> { JointType.HipCenter, JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight};

        List<JointType> handLeft = new List<JointType> { JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};

        List<JointType> handRight = new List<JointType> { JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };

        List<JointType> body = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft, JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight, JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft,
            JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };


        ConnectDB connect = new ConnectDB();
        Position position = new Position();


        public double calScore(Skeleton s, string poseName, string classRoom, int frame)
        {
            double score = 0;
            double totalScore = 1;


            List<List<JointType>> listsJoint = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };

            foreach (List<JointType> list in listsJoint)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    SkeletonPoint joint = s.Joints[list[i]].Position;
                    SkeletonPoint nextJoint = s.Joints[list[i + 1]].Position;

                    //trainer
                    Point3D trainerStartpoint = position.getPosition(list[i], poseName, classRoom, frame);
                    Point3D trainerEndpoint = position.getPosition(list[i + 1], poseName, classRoom, frame);
                    Vector3D trainerVector = getVector(trainerStartpoint, trainerEndpoint);
                    Vector3D normalizeTrainer = normolizeVector(trainerVector);

                    //trainee
                    Point3D traineeStartpoint = new Point3D(joint.X, joint.Y, joint.Z);
                    Point3D traineeEndpoint = new Point3D(nextJoint.X, nextJoint.Y, nextJoint.Z);
                    Vector3D traineeVector = getVector(traineeStartpoint, traineeEndpoint);
                    Vector3D normalizeTrainee = normolizeVector(traineeVector);

                    score = compareVector(normalizeTrainer, normalizeTrainee);
                    Console.WriteLine(score);
                    totalScore *= score;
                }
            }

            return totalScore;
        }

        private float distance(Point3D template, Point3D input)
        {
            double tempSum = 0;
            tempSum += Math.Pow(Math.Abs(input.X - template.X), 2);
            tempSum += Math.Pow(Math.Abs(input.Y - template.Y), 2);
            tempSum += Math.Pow(Math.Abs(input.Z - template.Z), 2);
            return (float)Math.Sqrt(tempSum);
        }

        private double compareVector(Vector3D trainer, Vector3D trainee)
        {
            double score = 0;
            score = (trainee.X * trainer.X) + (trainee.Y * trainer.Y) + (trainee.Z * trainer.Z);
            return score;
        }

        public Vector3D getVector(Point3D start, Point3D end)
        {
            Vector3D vector = new Vector3D();
            vector.X = end.X - start.X;
            vector.Y = end.Y - start.Y;
            vector.Z = end.Z - start.Z;
            return vector;
        }

        private Vector3D normolizeVector(Vector3D v)
        {
            Vector3D vectorResult = new Vector3D(v.X, v.Y, v.Z);
            vectorResult.Normalize();
            return vectorResult;
        }


        public double calDistance(int input, int frame)
        //public double calDistance(Skeleton input, int frame)
        {
            double totalScore = 1;
            double score = 0;
            foreach (JointType list in body)
            {
                //SkeletonPoint joint = input.Skel.Joints[list].Position;

                //trainer
                Point3D point = position.getPosition(list, "Motion", "TestMotion", frame);
                Point3D inputPoint = position.getPosition(list, "MotionCompare", "TestMotion", input);



                //trainee
                //Point3D inputPoint = new Point3D(joint.X, joint.Y, joint.Z);
                
                score = distance(point, inputPoint);
                Console.WriteLine(score);
                totalScore += score;
                
            }

            return totalScore;
        }



        //public double calScore(Skeleton s)
        //{
        //    double score = 0;
        //    double totalScore = 0;


        //    List<List<JointType>> li = new List<List<JointType>> { legLeft, legRight, handLeft, handRight};

        //    foreach (List<JointType> j in li)
        //    {
        //        for (int i = 0; i < j.Count - 1; i++)
        //        {
        //              //s.Joints[j[i]].Position
        //            //s.Joints[j[i + 1]]
        //            Vector traninee = getVector(s.Joints[j[i]].Position.X, s.Joints[j[i]].Position.Y, s.Joints[j[i]].Position.Z,
        //                s.Joints[j[i + 1]].Position.X, s.Joints[j[i + 1]].Position.Y, s.Joints[j[i + 1]].Position.Z);

        //            Vector traninerUnit = normalize(getTrainnerVector(j[i], j[i + 1]));
        //            Vector tranineeUnit = normalize(traninee);
        //            score = compareVector(traninerUnit, tranineeUnit);
        //            //Console.Write(j[i] + ". X: " + s.Joints[j[i]].Position.X);
        //            //Console.Write(" Y: " + s.Joints[j[i]].Position.Y);
        //            //Console.Write(" Z: " + s.Joints[j[i]].Position.Z);
        //            //Console.WriteLine("");
        //            //Console.WriteLine(j[i] + " to " + j[i + 1] + " " + tranineeUnit.X + " " + tranineeUnit.Y + " " + tranineeUnit.Z);
        //            //Console.WriteLine(j[i] + " to " + j[i + 1] + " " + traninerUnit.X + " " + traninerUnit.Y + " " + traninerUnit.Z);
        //            Console.WriteLine(score);

        //        }
        //    }
        //    return totalScore;
        //}



        //private Vector getTrainnerVector(JointType joint, JointType nextJoint)
        //{
        //    Vector trainer = connect.getJointPosition(joint);
        //    Vector trainer1 = connect.getJointPosition(nextJoint);

        //    Vector vector = getVector(trainer.X, trainer.Y, trainer.Z, trainer1.X, trainer1.Y, trainer1.Z);

        //    return vector;
        //}



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
