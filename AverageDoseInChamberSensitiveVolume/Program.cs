using System;
using System.IO;
using System.Linq;
using AverageDoseInSensitiveVolume;
using AverageDoseInSensitiveVolume.ViewModels;
using Juntendo.MedPhys.Esapi;
using VolumeAverage;

namespace AverageDoseInChamberSensitiveVolume
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    double radius = 4;
        //    double length = 5;

        //    int radiusSize = 100;
        //    int lengthSize = 100;
        //    int thetaSize = 100;

        //    var cylinder = new VolumeAverage.Cylinder("Test", radius, length, radiusSize, lengthSize, thetaSize);

        //    var volumeAverage = cylinder.VolumeAverageDose();
        //}

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                using (var app = VMS.TPS.Common.Model.API.Application.CreateApplication("SysAdmin", "SysAdmin"))
                {
                    Execute(app);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }

        static void Execute(VMS.TPS.Common.Model.API.Application app)
        {

            var folderPath = @"\\10.208.223.10\Eclipse";
            folderPath = Path.Combine(folderPath, "ResearchProjects", "AverageDose");

            // For Non-clinical Eclipse
            var computerName = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
            var homePath = System.Environment.GetEnvironmentVariable("HOMEPATH");
            var homeDrive = System.Environment.GetEnvironmentVariable("HOMEDRIVE");
            if (computerName == "ECQ275" || computerName == "ECM516NC" || computerName == "XPS13")
            {
                folderPath = homeDrive + Path.Combine(homePath, @"Desktop\AverageDose");
            }

            var patientId = "Physics";
            var courseId = "DLG_TEST_06X";
            var planId = "06X";

            var currentPatient = app.OpenPatientById(patientId);
            var currentCourse = EsapiHelpers.GetCourse(currentPatient, courseId);
            var currentPlanSetup = EsapiHelpers.GetPlanSetup(currentCourse, planId);

            var mainWindowViewModel = new MainWindowViewModel();

            mainWindowViewModel.FolderPath = folderPath;
            mainWindowViewModel.FileName = "AverageDose.csv";

            if (currentPlanSetup.Beams.Count() > 0)
            {
                var query = currentPlanSetup.Beams.First().FieldReferencePoints.Where(p => !double.IsNaN(p.RefPointLocation.x));
                foreach (var p in query)
                {
                    var xDcs = p.RefPointLocation.x;
                    var yDcs = p.RefPointLocation.y;
                    var zDcs = p.RefPointLocation.z;

                    var pUcs = EsapiHelpers.DicomToUserCoordinates(xDcs, yDcs, zDcs, currentPlanSetup);

                    var x = pUcs[0];
                    var y = pUcs[1];
                    var z = pUcs[2];


                    mainWindowViewModel.FieldReferencePoints.Add(new AverageDoseInSensitiveVolume.Models.FieldReferencePoint(x, y, z, xDcs, yDcs, zDcs, p.ReferencePoint.Id));
                }
            }

            var doseVoxel = new DoseVoxel(currentPlanSetup);

            var voxel = doseVoxel.Voxel;

            // Add PinPoint 3D
            mainWindowViewModel.Cylinders.Add(new Cylinder("PinPoint 3D", 1.45, 2.9, voxel));
            mainWindowViewModel.Cylinders.Add(new Cylinder("Semiflex 3D", 2.4, 4.8, voxel));
            mainWindowViewModel.Cylinders.Add(new Cylinder("Semiflex", 2.75, 6.5, voxel));
            mainWindowViewModel.Cylinders.Add(new Cylinder("Farmer", 3.05, 23, voxel));

            var mainWindow = new MainWindow(mainWindowViewModel);

            mainWindow.ShowDialog();
        }
    }
}
