using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VolumeAverage
{
    /// <summary>
    /// Helper class.
    /// </summary>
    class Helpers
    {
        /// <summary>
        /// Interpolates the voxel values to obtain a value at (x, y, z)
        /// </summary>
        /// <param name="x">An x coordinate.</param>
        /// <param name="y">A y coordinate.</param>
        /// <param name="z">A z coordinate.</param>
        /// <param name="xSize">The size in the x direction.</param>
        /// <param name="ySize">The size in the y direction.</param>
        /// <param name="zSize">The size in the z direction.</param>
        /// <param name="x0">The initial value in the x direction.</param>
        /// <param name="y0">The initial value in the y direction.</param>
        /// <param name="z0">The initial value in the z direction.</param>
        /// <param name="xDelta">The grid width in the x direction.</param>
        /// <param name="yDelta">The grid width in the y direction.</param>
        /// <param name="zDelta">The grid width in the z direction.</param>
        /// <param name="voxel">The voxel values.</param>
        /// <remarks>Th voxel values are stored as voxel[iz, iy, ix].</remarks>
        /// <returns>The interpolated voxel value at (x, y, z).</returns>
        public static double VoxelInterpolation(double x, double y, double z, int xSize, int ySize, int zSize,
            double x0, double y0, double z0, double xDelta, double yDelta, double zDelta,
            double[,,] voxel)
        {
            int ix = (int)((x - x0) / xDelta);
            int iy = (int)((y - y0) / yDelta);
            int iz = (int)((z - z0) / zDelta);

            // for extrapolation
            if (ix >= xSize - 1) ix = xSize - 2;
            if (iy >= ySize - 1) iy = ySize - 2;
            if (iz >= zSize - 1) iz = zSize - 2;

            // for extrapolation
            if (ix < 0) ix = 0;
            if (iy < 0) iy = 0;
            if (iz < 0) iz = 0;

            return TrilinearInterpolation3D(x, y, z,
                x0 + ix * xDelta, x0 + (ix + 1) * xDelta,
                y0 + iy * yDelta, y0 + (iy + 1) * yDelta,
                z0 + iz * zDelta, z0 + (iz + 1) * zDelta,
                voxel[iz, iy, ix], voxel[iz + 1, iy, ix], voxel[iz, iy + 1, ix], voxel[iz + 1, iy + 1, ix],
                voxel[iz, iy, ix + 1], voxel[iz + 1, iy, ix + 1], voxel[iz, iy + 1, ix + 1], voxel[iz + 1, iy + 1, ix + 1]); ;
        }


        /// <summary>
        /// 3D trilinear interpolation
        /// </summary>
        /// <param name="x"> the value of the first coordinate to be evaluated </param>
        /// <param name="y"> the value of the second coordinate to be evaluated </param>
        /// <param name="z"> the value of the third coordinate to be evaluated </param>
        /// <param name="x1"> the start value of the first coordinate where the function value is given </param>
        /// <param name="x2"> the end value of the first coordinate where the function value is given </param>
        /// <param name="y1"> the start value of the second coordinate where the function value is given </param>
        /// <param name="y2"> the end value of the second coordinate where the function value is given </param>
        /// <param name="z1"> the start value of the third coordinate where the function value is given </param>
        /// <param name="z2"> the start value of the third coordinate where the function value is given </param>
        /// <param name="f111"> function value at (x1, y1, z1) </param>
        /// <param name="f112"> function value at (x1, y1, z2) </param>
        /// <param name="f121"> function value at (x1, y2, z1) </param>
        /// <param name="f122"> function value at (x1, y2, z2) </param>
        /// <param name="f211"> function value at (x2, y1, z1) </param>
        /// <param name="f212"> function value at (x2, y1, z2) </param>
        /// <param name="f221"> function value at (x2, y2, z1) </param>
        /// <param name="f222"> function value at (x2, y2, z2) </param>
        /// <returns> interpolated value at (x, y) </returns>
        public static double TrilinearInterpolation3D(double x, double y, double z,
            double x1, double x2,
            double y1, double y2,
            double z1, double z2,
            double f111, double f112, double f121, double f122,
            double f211, double f212, double f221, double f222)
        {
            double denominator = 1.0 / ((x2 - x1) * (y2 - y1) * (z2 - z1));
            double numerator
                = f111 * (x2 - x) * (y2 - y) * (z2 - z)
                + f112 * (x2 - x) * (y2 - y) * (z - z1)
                + f121 * (x2 - x) * (y - y1) * (z2 - z)
                + f122 * (x2 - x) * (y - y1) * (z - z1)
                + f211 * (x - x1) * (y2 - y) * (z2 - z)
                + f212 * (x - x1) * (y2 - y) * (z - z1)
                + f221 * (x - x1) * (y - y1) * (z2 - z)
                + f222 * (x - x1) * (y - y1) * (z - z1);
            return numerator * denominator;
        }
    }
}
