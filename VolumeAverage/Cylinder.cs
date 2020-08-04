using Microsoft.Win32;
using System;

namespace VolumeAverage
{
    public class Cylinder
    {
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }
        public double Radius { get; private set; }
        public double Length { get; private set; }

        public int RadiusSize { get; set; }
        public int LengthSize { get; set; }
        public int ThetaSize { get; set; }

        public double DeltaRadius { get; set; }
        public double DeltaLength { get; set; }
        public double DeltaTheta { get; set; }

        public double[] Dose { get; set; }

        public Cylinder(double radius, double length, int radiusSize, int lengthSize, int thetaSize)
        {
            Radius = radius;
            Length = length;
            RadiusSize = radiusSize;
            LengthSize = lengthSize;
            ThetaSize = thetaSize;

            DeltaRadius = radius / radiusSize;
            DeltaLength = length / lengthSize;
            DeltaTheta = 2 * Math.PI / thetaSize;
        }

        public double VolumeAverageDose()
        {
            double sum = 0.0;
            for (int i = 0; i <= LengthSize; i++)
            {
                //factor for trapezoidal rule
                double lengthFactor = 1.0;
                if (i == 0 || i == LengthSize) lengthFactor = 0.5;
                double l = i * DeltaLength - Length * 0.5;
                for (int j = 0; j <= RadiusSize; j++)
                {
                    //factor for trapezoidal rule
                    double radiusFactor = 1.0;
                    if (j == 0 || j == RadiusSize) radiusFactor = 0.5;
                    double r = j * DeltaRadius;
                    for (int k = 0; k <= ThetaSize; k++)
                    {
                        //factor for trapezoidal rule
                        double thetaFactor = 1.0;
                        if (k == 0 || k == ThetaSize) thetaFactor = 0.5;
                        double theta = k * DeltaTheta;

                        double x = CenterX + r * Math.Cos(theta);
                        double y = CenterY + r * Math.Sin(theta);
                        double z = CenterZ + l;

                        double dose = theta*theta;

                        sum += lengthFactor * radiusFactor * thetaFactor * dose * r;
                    }
                }
            }
            sum *= DeltaRadius * DeltaTheta * DeltaLength;
            //double volume = Math.PI * Radius * Radius * Length;
            //double volume = Math.PI * Radius * Radius * (Length * Length * Length/3.0/4.0);
            double volume = 0.5 * Radius * Radius * (2 * Math.PI * 2 * Math.PI * 2 * Math.PI / 3) * Length;
            return sum / volume;
        }

    }
}
