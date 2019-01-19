using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace MuayThaiTraining
{
    public class BodyJoint
    {
        Skeleton skel;
        int frame;

        public BodyJoint(){}

        public BodyJoint(Skeleton skel, int frame)
        {
            this.Skel = skel;
            this.Frame = frame;
        }

        public Skeleton Skel { get => skel; set => skel = value; }

        public int Frame { get => frame; set => frame = value; }
    }
}
