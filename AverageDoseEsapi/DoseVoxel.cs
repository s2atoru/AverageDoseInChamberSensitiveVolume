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
            var doseValues = EsapiHelpers.EsapiDoseToDose3dArray(doseEsapi);
            Voxel = new VolumeAverage.Voxel(doseEsapi.XSize, doseEsapi.YSize, doseEsapi.ZSize,
                doseEsapi.Origin.x, doseEsapi.Origin.y, doseEsapi.Origin.z, doseEsapi.XRes, doseEsapi.YRes, doseEsapi.ZRes, doseValues);
        }
    }
}
