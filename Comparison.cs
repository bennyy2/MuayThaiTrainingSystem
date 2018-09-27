using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuayThaiTraining
{
    class Comparison
    {
        Vector vector = new Vector();
        List<Vector> vectors = new List<Vector>();
        Double x;
        Double y;
        Double z;

        public List<Vector> generateVector(Double x1, Double y1, Double z1, Double x2, Double y2, Double z2)
        {
            vector = new Vector(x2-x1, y2-y1, z2-z1);
            vectors.Add(vector);

            return vectors;
        }




    }
}
