namespace MuayThaiTraining
{
    public class Vector
    {
        public double x;
        public double y;
        public double z;

        public Vector(){}

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }
    }
}