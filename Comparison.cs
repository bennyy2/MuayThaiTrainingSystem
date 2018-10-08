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
        List<JointType> limb1 = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

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

        public TreeNode createTree()
        {
            TreeNode skelTree = new TreeNode("hipCenter")
            {
                new TreeNode("hipLeft")
                {
                    new TreeNode("kneeLeft")
                },
                new TreeNode("spine")
                {
                    new TreeNode("shoulderCenter")
                },
                new TreeNode("hipRight")
                {
                    new TreeNode("kneeRight")
                }
            };


            return skelTree;
        }


        public double calScore(Skeleton s1)
        {
            return calScoreLimb(s1);
        }

        public double calScoreLimb(Skeleton s)
        {
            double score = 0;

            for (int i=0; i < limb1.Count-1; i++)
            {
                Vector traninee = getVector(s.Joints[limb1[i]].Position.X, s.Joints[limb1[i]].Position.Y, s.Joints[limb1[i]].Position.Z,
                    s.Joints[limb1[i+1]].Position.X, s.Joints[limb1[i+1]].Position.Y, s.Joints[limb1[i+1]].Position.Z);
                Vector traninerUnit = normalize(getTrainnerVector(limb1[i], limb1[1 + 1]));
                Vector tranineeUnit = normalize(traninee);
                score = compareVector(traninerUnit, tranineeUnit);

            }
            return score;
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
