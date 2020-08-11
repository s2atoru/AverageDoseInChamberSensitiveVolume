namespace AverageDoseInSensitiveVolume.Models
{
    /// <summary>
    /// Field reference point class.
    /// Contains the information of a field reference point.
    /// </summary>
    /// <remarks>
    /// The coordinates are defined in the user coordinate system.
    /// </remarks>
    public class FieldReferencePoint
    {
        ///<value>Gets the x coordinate of the reference point.</value>
        public double X { get; private set; }
        ///<value>Gets the y coordinate of the reference point.</value>
        public double Y { get; private set; }
        ///<value>Gets the z coordinate of the reference point.</value>
        public double Z { get; private set; }

        ///<value>Gets the x coordinate of the reference point in the DICOM coordinate system.</value>
        public double XDcs { get; private set; }
        ///<value>Gets the y coordinate of the reference point in the DICOM coordinate system.</value>
        public double YDcs { get; private set; }
        ///<value>Gets the z coordinate of the reference point in the DICOM coordinate system.</value>
        public double ZDcs { get; private set; }

        ///<value>Gets the ID of the reference point.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">An x coordinate of the reference point.</param>
        /// <param name="y">An y coordinate of the reference point.</param>
        /// <param name="z">An z coordinate of the reference point.</param>
        /// <param name="id">An ID of the reference point.</param>
        /// <remarks>The user and DICOM coordinate systems are assumed to be the same.</remarks>
        public FieldReferencePoint(double x, double y, double z, string id)
        {
            X = x;
            Y = y;
            Z = z;
            XDcs = x;
            YDcs = y;
            ZDcs = z;
            Id = id;
        }

        // <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">An x coordinate of the reference point.</param>
        /// <param name="y">An y coordinate of the reference point.</param>
        /// <param name="z">An z coordinate of the reference point.</param>
        /// <param name="xDcs">The x coordinate of the reference point in the DICOM coordinate system.</param>
        /// <param name="yDcs">The y coordinate of the reference point in the DICOM coordinate system.</param>
        /// <param name="zDcs">The z coordinate of the reference point in the DICOM coordinate system.</param>
        /// <param name="id">An ID of the reference point.</param>
        /// <remarks>The user and DICOM coordinate systems can be different.</remarks>
        public FieldReferencePoint(double x, double y, double z, double xDcs, double yDcs, double zDcs, string id)
        {
            X = x;
            Y = y;
            Z = z;
            XDcs = xDcs;
            YDcs = yDcs;
            ZDcs = zDcs;
            Id = id;
        }
    }
}
