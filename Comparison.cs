using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace MuayThaiTraining
{
    class Comparison
    {


        List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

        List<JointType> legRight = new List<JointType> { JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight};

        List<JointType> handLeft = new List<JointType> { JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};

        List<JointType> handRight = new List<JointType> { JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };

        ConnectDB connect = new ConnectDB();
        Vector vector = new Vector();
        Double x;
        Double y;
        Double z;

        public Vector getVector(Double x1, Double y1, Double z1, Double x2, Double y2, Double z2)
        {
            vector = new Vector(x2-x1, y2-y1, z2-z1);
            return vector;
        }

        //public TreeNode createTree()
        //{
        //    TreeNode skelTree = new TreeNode("hipCenter")
        //    {
        //        new TreeNode("hipLeft")
        //        {
        //            new TreeNode("kneeLeft")
        //        },
        //        new TreeNode("spine")
        //        {
        //            new TreeNode("shoulderCenter")
        //        },
        //        new TreeNode("hipRight")
        //        {
        //            new TreeNode("kneeRight")
        //        }
        //    };


        //    return skelTree; 
        //}



        public double calScore(Skeleton s)
        {
            double score = 0;
            double totalScore = 0;


            List<List<JointType>> li = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };

            foreach (List<JointType> j in li)
            {
                for (int i = 0; i < j.Count - 1; i++)
                {
                    //s.Joints[j[i]].Position
                    ////s.Joints[j[i + 1]]
                    Vector traninee = getVector(s.Joints[j[i]].Position.X, s.Joints[j[i]].Position.Y, s.Joints[j[i]].Position.Z,
                        s.Joints[j[i + 1]].Position.X, s.Joints[j[i + 1]].Position.Y, s.Joints[j[i + 1]].Position.Z);
                    Vector traninerUnit = normalize(getTrainnerVector(j[i], j[i + 1]));
                    Vector tranineeUnit = normalize(traninee);
                    score = compareVector(traninerUnit, tranineeUnit);
                    Console.Write(j[i] + ". X: " + s.Joints[j[i]].Position.X);
                    Console.Write(" Y: " + s.Joints[j[i]].Position.Y);
                    Console.Write(" Z: " + s.Joints[j[i]].Position.Z);
                    Console.WriteLine("");
                    Console.WriteLine(j[i] + " to " + j[i + 1] + " " + tranineeUnit.X + " " + tranineeUnit.Y + " " + tranineeUnit.Z);
                    Console.WriteLine(j[i] + " to " + j[i + 1] + " " + traninerUnit.X + " " + traninerUnit.Y + " " + traninerUnit.Z);
                    Console.WriteLine(score);
                   
                }
            }
            return totalScore;
        }

        

        private Vector getTrainnerVector(JointType joint, JointType nextJoint)
        {
            Vector trainer = connect.getJointPosition(joint);
            Vector trainer1 = connect.getJointPosition(nextJoint);
            Vector traniner = getVector(trainer.X, trainer.Y, trainer.Z, trainer1.X, trainer1.Y, trainer1.Z);
            return trainer;
        }

        private Vector normalize(Vector v)
        {
            double vectorSize = Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2) + Math.Pow(v.Z, 2));
            vector = new Vector(v.X/vectorSize, v.Y / vectorSize, v.Z / vectorSize);

            return vector;
        }

        private double compareVector(Vector trainer, Vector trainee)
        {
            double score = 0;
            score = (trainee.X * trainer.X) + (trainee.Y * trainer.Y) + (trainee.Z * trainer.Z);
            return score;
        }

    }
}
