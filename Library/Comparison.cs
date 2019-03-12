using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
            JointType.KneeLeft};

        List<JointType> legRight = new List<JointType> { JointType.HipCenter, JointType.HipRight,
            JointType.KneeRight};

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
        public Tuple<List<Tuple<string, double>>, double> calScore(Skeleton s, string poseName, string classRoom, int frame)
        {
            double score = 0;
            double totalScore = 0;
            List<List<JointType>> listsJoint = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };
            
            List<Tuple<string, double>> tupleList = new List<Tuple<string, double>>();

            foreach (List<JointType> i in listsJoint)
            {
                for (int j = 0; j < i.Count-1; j++)
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
                    Tuple<string, double> tuple = new Tuple<string, double>(i[j].ToString(), score);
                    tupleList.Add(tuple);

                    totalScore += score;
                }
            }

            Console.WriteLine("-----------------------------------------");     

            return new Tuple<List<Tuple<string, double>>, double>(tupleList, totalScore/16);
        }
       

        public double compareScoreByJoint(JointType first, JointType sec, string poseName, string classRoom, Skeleton skel, int frameTrainee, int frameTrainer)
        {
            double score = 0;
            double totalscore = 0;
            List<JointType> joint = new List<JointType> { first, sec };
            

            for (int i = 0; i < joint.Count - 1; i++)
            {

                //trainee input
                Point3D start = position.getPosition(first, poseName, classRoom, frameTrainee);
                Point3D end = position.getPosition(sec, poseName, classRoom, frameTrainee);
                Vector3D trainee = getVector(start, end);
                Vector3D traineeN = normolizeVector(trainee);


                //trainer template
                Point3D startPoint = new Point3D(skel.Joints[first].Position.X, skel.Joints[first].Position.Y, skel.Joints[first].Position.Z);
                Point3D endPoint = new Point3D(skel.Joints[sec].Position.X, skel.Joints[sec].Position.Y, skel.Joints[sec].Position.Z);
                Vector3D trainer = getVector(startPoint, endPoint);
                Vector3D trainerN = normolizeVector(trainer);
                //compare
                score = compareVector(trainerN, traineeN);
                Console.WriteLine(joint[i].ToString() + " to " + joint[i + 1].ToString() + " : " + score);

                totalscore += score;


            }


            return totalscore;
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
