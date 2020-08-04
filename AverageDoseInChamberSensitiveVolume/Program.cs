using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverageDoseInChamberSensitiveVolume
{
    class Program
    {
        static void Main(string[] args)
        {
            double radius = 4;
            double length = 5;

            int radiusSize = 100;
            int lengthSize = 100;
            int thetaSize = 100;

            var cylinder = new VolumeAverage.Cylinder(radius, length, radiusSize, lengthSize, thetaSize);

            var volumeAverage = cylinder.VolumeAverageDose();
        }
    }
}
