using Microsoft.Win32;
using Prism.Mvvm;
using System;

namespace VolumeAverage
{
    /// <summary>
    /// The Cylinder class.
    /// Contains the cylinder geometrical information and the dose voxel.
    /// </summary>
    public class Cylinder : BindableBase
    {
        public string Id { get; set; }
        /// <value>Gets the center coordinate in the x direction.</value>
        public double XCenter { get; set; }
        /// <value>Gets the center coordinate in the y direction.</value>
        public double YCenter { get; set; }
        /// <value>Gets the center coordinate in the z direction.</value>
        public double ZCenter { get; set; }

        /// <value>Gets the radius of a cylinder.</value>
        public double Radius { get; private set; }
        /// <value>Gets the length of a cylinder.</value>
        public double Length { get; private set; }

        /// <value>Gets the size in the radius direction.</value>
        public int RadiusSize { get; set; }
        /// <value>Gets the size in the length direction.</value>
        public int LengthSize { get; set; }
        /// <value>Gets the size in the theta direction.</value>
        public int ThetaSize { get; set; }

        /// <value>Gets the grid width in the radius direction.</value>
        public double RadiusDelta { get; set; }
        /// <value>Gets the grid width in the length direction.</value>
        public double LengthDelta { get; set; }
        /// <value>Gets the grid width in the theta direction.</value>
        public double ThetaDelta { get; set; }

        private double averageDoseValue;
        public double AverageDoseValue
        {
            get { return averageDoseValue; }
            set { SetProperty(ref averageDoseValue, value); }
        }

        /// <value>Gets Voxel of dose.</value>
        public Voxel DoseVoxel { get; set; }

        /// <value>Gets the cylinder (chamber) volume.</value>
        public double Volume { get { return Math.PI * Radius * Radius * Length/1000; } }

        public Cylinder(string id, double radius, double length, Voxel doseVoxel = null, int radiusSize = 10, int lengthSize = 10, int thetaSize = 10)
        {
            Id = id;

            Radius = radius;
            Length = length;
            RadiusSize = radiusSize;
            LengthSize = lengthSize;
            ThetaSize = thetaSize;

            RadiusDelta = radius / radiusSize;
            LengthDelta = length / lengthSize;
            ThetaDelta = 2 * Math.PI / thetaSize;

            DoseVoxel = doseVoxel;
        }

        public Cylinder(double radius, double length, Voxel doseVoxel, int radiusSize, int lengthSize, int thetaSize)
        {
            Radius = radius;
            Length = length;
            RadiusSize = radiusSize;
            LengthSize = lengthSize;
            ThetaSize = thetaSize;

            RadiusDelta = radius / radiusSize;
            LengthDelta = length / lengthSize;
            ThetaDelta = 2 * Math.PI / thetaSize;

            DoseVoxel = doseVoxel;
        }

        public double VolumeAverageDose()
        {
            double sum = 0.0;
            for (int i = 0; i <= LengthSize; i++)
            {
                //factor for trapezoidal rule
                double lengthFactor = 1.0;
                if (i == 0 || i == LengthSize) lengthFactor = 0.5;
                double l = i * LengthDelta - Length * 0.5;
                for (int j = 0; j <= RadiusSize; j++)
                {
                    //factor for trapezoidal rule
                    double radiusFactor = 1.0;
                    if (j == 0 || j == RadiusSize) radiusFactor = 0.5;
                    double r = j * RadiusDelta;
                    for (int k = 0; k <= ThetaSize; k++)
                    {
                        //factor for trapezoidal rule
                        double thetaFactor = 1.0;
                        if (k == 0 || k == ThetaSize) thetaFactor = 0.5;
                        double theta = k * ThetaDelta;

                        double x = XCenter + r * Math.Cos(theta);
                        double y = YCenter + r * Math.Sin(theta);
                        double z = ZCenter + l;

                        double dose = DoseVoxel.GetVoxelValue(x, y, z);

                        sum += lengthFactor * radiusFactor * thetaFactor * dose * r;
                    }
                }
            }
            sum *= RadiusDelta * ThetaDelta * LengthDelta;
            double volume = Math.PI * Radius * Radius * Length;

            //double volume = Math.PI * Radius * Radius * (Length * Length * Length/3.0/4.0);
            //double volume = 0.5 * Radius * Radius * (2 * Math.PI * 2 * Math.PI * 2 * Math.PI / 3) * Length;

            AverageDoseValue = sum / volume;

            return AverageDoseValue;
        }

    }
}
