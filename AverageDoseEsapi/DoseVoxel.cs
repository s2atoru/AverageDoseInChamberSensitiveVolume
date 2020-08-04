using System.Security.Cryptography.X509Certificates;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Juntendo.MedPhys.Esapi { 
    public class DoseVoxel
    {
        /// <value>Voxel of dose.</value>
        public VolumeAverage.Voxel Voxel { get; private set; }

        /// <value>ESAPI PlanSetup.</value>
        public PlanSetup PlanSetup { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="planSetup">ESAPI PlanSetup.</param>
        public DoseVoxel(PlanSetup planSetup)
        {
            PlanSetup = planSetup;
            Dose doseEsapi = planSetup.Dose;
            var doseValues = Helpers.EsapiDoseToDose3dArray(doseEsapi);
            Voxel = new VolumeAverage.Voxel(doseEsapi.XSize, doseEsapi.YSize, doseEsapi.ZSize,
                doseEsapi.Origin.x, doseEsapi.Origin.y, doseEsapi.Origin.z, doseEsapi.XRes, doseEsapi.YRes, doseEsapi.ZRes, doseValues);
        }

        /// <summary>
        /// ESAPI user coordinate system to the DICOM coordinate system.
        /// </summary>
        /// <param name="x">The x coordinate in the User coordinate system.</param>
        /// <param name="y">The y coordinate in the User coordinate system.</param>
        /// <param name="z">The z coordinate in the User coordinate system.</param>
        /// <returns>Coordinates in the DICOM coordinate system.</returns>
        public double[] UserToDicomCoordinates(double x, double y, double z)
        {
            var strucureSet = PlanSetup.StructureSet;
            var imageEsapi = strucureSet.Image;
            var vectorInUCS = new VVector(x, y, z);
            var vectorInDCS = imageEsapi.UserToDicom(vectorInUCS, PlanSetup);

            return new double[] { vectorInDCS.x, vectorInDCS.y, vectorInDCS.z };
        }
    }
}
