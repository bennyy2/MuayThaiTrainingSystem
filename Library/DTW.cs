using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuayThaiTraining
{
    class DTW
    {
        Position position = new Position();
        Comparison comparison = new Comparison();

        public double DTWDistance(List<BodyJoint> input)
        {
            //int rows = input.Count;
            //int columns = position.lenghtFrame("Motion", "TestMotion");
            int rows = 4;
            int columns = 3;
            //int columns = template.Count / 19;
            if (rows < (double)(columns / 2) || columns < (double)(rows / 2))
            {
                return double.MaxValue;
            }

            double[,] dtw = new double[rows, columns];
            dtw[0, 0] = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 1; j < columns; j++)
                {
                    //double cost = comparison.calDistance(input[i], j);
                    double cost = comparison.calDistance(i, j);
                    if (i == 0 && j == 0)
                        dtw[i, j] = cost;
                    else if (i == 0)
                        dtw[i, j] = cost + dtw[i, j - 1];
                    else if (j == 0)
                        dtw[i, j] = cost + dtw[i - 1, j];
                    else
                        dtw[i, j] = (cost + Math.Min(dtw[i - 1, j], dtw[i - 1, j - 1]));// insert ,match
                }
            }
            return dtw[rows - 1, columns - 1];
        }

        public double DTW_improved(List<BodyJoint> input)
        {
            int rows = position.lenghtFrame("Motion", "TestMotion");
            int columns = position.lenghtFrame("MotionCompare", "TestMotion");//input
            // Don't compare two sequences if one of their lengths is half the other's
            if (columns <= (0.5 * rows) || rows <= (0.5 * columns))
                return double.PositiveInfinity;
            
            

            double[] c1 = new double[rows];
            double[] c2 = new double[rows];
            double[] temp; // To hold address only (use it in swapping address) 
            //c1[0] = comparison.calDistance(input[0].Skel, 0);
            c1[0] = comparison.calDistance(0, 0);
            for (int i = 1; i < rows; i++)
                c1[i] = c1[i - 1] + comparison.calDistance(0, i);
            for (int i = 1; i < columns; i++)
            {
                c2[0] = comparison.calDistance(i, 0) + c1[0];
                c2[1] = comparison.calDistance(i, 1) + Math.Min(c1[0], c1[1]);
                // Calculating first 2 elements of the array before the loop
                //since they both need special conditions
                for (int j = 2; j < rows; j++)
                    c2[j] = Math.Min(c1[j], Math.Min(c1[j - 1], c1[j - 2])) + comparison.calDistance(i, j);

                if (i != columns - 1) // Swapping addresses of c1 & c2
                {
                    temp = c2;
                    c2 = c1;
                    c1 = temp;
                }
            }
            return c2[rows - 1] / (0.5 * (columns + rows)); // Normalization: Dividing edit distance by average of input length & template length
        }





    }
}
