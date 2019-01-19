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
        int rows;
        int columns;
            

        public double DTWDistance(List<BodyJoint> input)
        {
            int rows = input.Count;
            int columns = position.lenghtFrame("Motion", "TestMotion");
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
                    double cost = comparison.calDistance(input[i].Skel , j);
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
            //int columns = position.lenghtFrame("Motion", "TestMotion");//input
            // Don't compare two sequences if one of their lengths is half the other's
            if (columns <= (0.5 * rows) || rows <= (0.5 * columns))
                return double.PositiveInfinity;
            
            

            double[] c1 = new double[rows];
            double[] c2 = new double[rows];
            double[] temp; // To hold address only (use it in swapping address) 
            c1[0] = comparison.calDistance(input[0].Skel, 0);
            for (int i = 1; i < rows; i++)
                c1[i] = c1[i - 1] + comparison.calDistance(input[0].Skel, i);
            for (int i = 1; i < columns; i++)
            {
                c2[0] = comparison.calDistance(input[0].Skel, 0) + c1[0];
                c2[1] = comparison.calDistance(input[0].Skel, 1) + Math.Min(c1[0], c1[1]);
                // Calculating first 2 elements of the array before the loop
                //since they both need special conditions
                for (int j = 2; j < rows; j++)
                    c2[j] = Math.Min(c1[j], Math.Min(c1[j - 1], c1[j - 2])) + comparison.calDistance(input[0].Skel, j);

                if (i != columns - 1) // Swapping addresses of c1 & c2
                {
                    temp = c2;
                    c2 = c1;
                    c1 = temp;
                }
            }
            foreach (double i in c2)
            {
                Console.WriteLine("C2 : " +i);
            }
            foreach (double i in c1)
            {
                Console.WriteLine("C1 : " +i);
            }

            return c2[rows - 1] / (0.5 * (columns + rows)); // Normalization: Dividing edit distance by average of input length & template length
        }


        public Tuple<double[,], double[,]> modifiedDTW(List<BodyJoint> input, string pose, string room)
        {
            int rows = input.Count;
            int columns = position.lenghtFrame(pose, room);

            double[,] table = new double[rows, columns];
            double[,] score = new double[rows, columns];

            for (int i = 0; i <= rows; i++)
            {
                for (int j = 0; j <= columns; j++)
                {
                    Console.WriteLine("Frame "+i.ToString()+" "+j.ToString());

                    double cost = comparison.calScore(input[i].Skel, pose, room, j);
                    if (i == 0 && j == 0)
                    {
                        table[i, j] = cost;
                        score[i, j] = cost;
                    }

                    else if (i == 0)
                    {
                        table[i, j] = cost + table[i, j - 1];
                        score[i, j] = cost;
                    }

                    else if (j == 0)
                    {
                        table[i, j] = cost + table[i - 1, j];
                        score[i, j] = cost;
                    }

                    else
                    {
                        table[i, j] = (cost + Math.Max(table[i - 1, j], Math.Max(table[i - 1, j - 1], table[i, j - 1])));
                        score[i, j] = cost;

                    }
                }
            }

            Tuple<double[,], double[,]> result = new Tuple<double[,], double[,]>(table, score);

            return result;
        }


        public List<Tuple<int, int, double, int>> wrapPath(double[,] table, double[,] score, int rows, int cols)
        {
            List<Tuple<int, int, double, int>> path = new List<Tuple<int, int, double, int>>();
            int round = 1;
            Tuple<int, int, double, int> tuple = new Tuple<int, int, double, int>(rows, cols, score[rows, cols], 1);
            path.Add(tuple);

            while (rows > 0 || cols > 0)
            {
                tuple = check(table, score, rows, cols, round);
                path.Add(tuple);
                rows = tuple.Item1;
                cols = tuple.Item2;
            }

            return path;
        }

        private Tuple<int, int, double, int> check(double[,] table, double[,] score, int row, int col, int round)
        {

            Tuple<int, int, double, int> frames = new Tuple<int, int, double, int>(row, col, score[row, col], round);
            
            if (row == 0 && col > 0)
            {
                frames = new Tuple<int, int, double, int>(row, col - 1, score[row, col - 1], round);
            }
            else if (row > 0 && col == 0)
            {
                frames = new Tuple<int, int, double, int>(row - 1, col, score[row - 1, col], round);
            }
            else if (row > 0 && col > 0)
            {
                if (round == 1)
                {
                    if (table[row - 1, col] >= table[row - 1, col - 1] && table[row - 1, col] >= table[row, col - 1])
                    {
                        round = 2;
                        frames = new Tuple<int, int, double, int>(row - 1, col, score[row - 1, col], round);
                    }
                    else if (table[row, col - 1] > table[row - 1, col - 1] && table[row, col - 1] > table[row - 1, col])
                    {
                        frames = new Tuple<int, int, double, int>(row, col - 1, score[row, col - 1], round);
                    }
                    else
                    {
                        frames = new Tuple<int, int, double, int>(row - 1, col - 1, score[row - 1, col - 1], round);
                    }

                }
                else
                {
                    if (table[row, col - 1] > table[row - 1, col - 1] && table[row, col - 1] > table[row - 1, col])
                    {
                        round = 1;
                        frames = new Tuple<int, int, double, int>(row, col - 1, score[row, col - 1], round);
                    }
                    if (table[row - 1, col] >= table[row - 1, col - 1] && table[row - 1, col] >= table[row, col - 1])
                    {
                        frames = new Tuple<int, int, double, int>(row - 1, col, score[row - 1, col], round);
                    }
                    else
                    {
                        frames = new Tuple<int, int, double, int>(row - 1, col - 1, score[row - 1, col - 1], round);
                    }
                }
            }

            return frames;
        }



    }
}
