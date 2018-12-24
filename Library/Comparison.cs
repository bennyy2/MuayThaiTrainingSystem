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


        //public double calScore(Skeleton s,string poseName, string classRoom, int frame)
        public double calScore(Skeleton s, string poseName, string classRoom, int frame)
        {
            double score = 0;
            double totalScore = 0;
            List<List<JointType>> listsJoint = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };

            foreach (List<JointType> i in listsJoint)
            {
                for (int j = 0; j < i.Count; j++)
                {
                    //trainer
                    Point3D trainerStartpoint = position.getPosition(i[j], poseName, classRoom, frame);
                    Point3D trainerEndpoint = position.getPosition(i[j+1], poseName, classRoom, frame);
                    Vector3D trainerVector = getVector(trainerStartpoint, trainerEndpoint);
                    Vector3D normalizeTrainer = normolizeVector(trainerVector);

                    //trainee
                    SkeletonPoint joint = s.Joints[i[j]].Position;
                    SkeletonPoint nextJoint = s.Joints[i[j + 1]].Position;

                    Point3D traineeStartpoint = new Point3D(joint.X, joint.Y, joint.Z);
                    Point3D traineeEndpoint = new Point3D(nextJoint.X, nextJoint.Y, nextJoint.Z);
                    Vector3D traineeVector = getVector(traineeStartpoint, traineeEndpoint);
                    Vector3D normalizeTrainee = normolizeVector(traineeVector);

                    score = compareVector(normalizeTrainer, normalizeTrainee);
                    Console.WriteLine(i[j].ToString() + " to " + i[j + 1].ToString() + " : " + score);
                    totalScore += score;
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

        

        //for dtw 
        public double calDistance(Skeleton input, int frame)
        {
            double totalScore = 1;
            double score = 0;
            foreach (JointType list in body)
            {
                SkeletonPoint joint = input.Joints[list].Position;

                //trainer
                Point3D point = position.getPosition(list, "Motion", "TestMotion", frame);

                //trainee
                Point3D inputPoint = new Point3D(joint.X, joint.Y, joint.Z);

                score = distance(point, inputPoint);
                //Console.WriteLine(score);
                totalScore += score;
                
            }

            return totalScore;
        }

        




    }
}
