namespace VolumeAverage
{
    /// <summary>
    /// Voxel class.
    /// Contains the voxel values and its coordinate information.
    /// </summary>
    public class Voxel
    {
        /// <value>Gets the voxel values. double[XSize, YSize, ZSize].</value>
        /// <remarks>The values are stored as Values[iz, iy, ix].</remarks>
        public double[,,] Values { get; private set; }
        ///<value>Gets the voxel size in the x direction.</value>
        public int XSize { get; private set; }
        ///<value>Gets the voxel size in the y direction.</value>
        public int YSize { get; private set; }
        ///<value>Gets the voxel size in the z direction.</value>
        public int ZSize { get; private set; }

        ///<value>Gets the initial value in the x direction.</value>
        public double X0 { get; private set; }
        ///<value>Gets the initial value in the y direction.</value>
        public double Y0 { get; private set; }
        ///<value>Gets the initial value in the z direction.</value>
        public double Z0 { get; private set; }

        ///<value>Gets the grid width in the x direction.</value>
        public double XDelta { get; private set; }
        ///<value>Gets the grid width in the y direction.</value>
        public double YDelta { get; private set; }
        ///<value>Gets the grid width in the z direction.</value>
        public double ZDelta { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="xSize">The voxel size in the x direction.</param>
        /// <param name="ySize">The voxel size in the y direction.</param>
        /// <param name="zSize">The voxel size in the z direction.</param>
        /// <param name="x0">The initial value in the x direction.</param>
        /// <param name="y0">The initial value in the y direction.</param>
        /// <param name="z0">The initial value in the z direction.</param>
        /// <param name="xDelta">The grid width in the x direction.</param>
        /// <param name="yDelta">The grid width in the y direction.</param>
        /// <param name="zDelta">The grid width in the z direction.</param>
        /// <param name="values">The voxel values. double[xSize, ySize, zSize]. </param>
        /// <remarks>
        /// The voxel values are stored as values[iz, iy, ix];
        /// </remarks>
        public Voxel(int xSize, int ySize, int zSize, double x0, double y0, double z0, double xDelta, double yDelta, double zDelta, double[,,] values)
        {
            XSize = xSize;
            YSize = ySize;
            ZSize = zSize;

            X0 = x0;
            Y0 = y0;
            Z0 = z0;

            XDelta = xDelta;
            YDelta = yDelta;
            ZDelta = zDelta;

            Values = values;
        }

        /// <summary>
        /// Gets the voxel value
        /// </summary>
        /// <param name="x">An x coordinate</param>
        /// <param name="y">A y coordinate</param>
        /// <param name="z">A z coordinate</param>
        /// <returns></returns>
        public double GetVoxelValue(double x, double y, double z)
        {
            return Helpers.VoxelInterpolation(x, y, z, XSize, YSize, ZSize, X0, Y0, Z0, XDelta, YDelta, ZDelta, Values);
        }
    }
}
