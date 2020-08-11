////////////////////////////////////////////////////////////////////////////////
// Helpers.cs
//
// Helper methods to manipulate courses etc.
//  
// Applies to: ESAPI v13, v13.5, v13.6.
//
// Copyright (c) 2015 Varian Medical Systems, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in 
//  all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
// THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;

using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Juntendo.MedPhys.Esapi
{
    public static class EsapiHelpers
    {

        public static Course GetCourse(Patient patient, string courseId)
        {
            var res = patient.Courses.Where(c => c.Id == courseId);
            if (res.Any())
            {
                return res.Single();
            }
            else
            {
                throw new InvalidOperationException($"No corresponding Course: {courseId}");
            }
        }

        public static PlanSetup GetPlanSetup(Course course, string planId)
        {
            var res = course.PlanSetups.Where(p => p.Id == planId);
            if (res.Any())
            {
                return res.Single();
            }
            {
                throw new InvalidOperationException($"No corresponding PlanSetup: {planId}");
            }
        }

        public static PlanSetup CheckAndGetPlanSetup(Course course, string planId)
        {
            var res = course.PlanSetups.Where(p => p.Id == planId);
            if (res.Any())
            {
                return res.Single();
            }
            {
                return null;
            }
        }
        
        public static double[,,] EsapiDoseToDose3dArray(Dose doseEsapi)
        {
            int xSize = doseEsapi.XSize;
            int ySize = doseEsapi.YSize;
            int zSize = doseEsapi.ZSize;

            double[,,] dose3dArray = new double[zSize, ySize, xSize];

            int[,] voxelValuesInPlane = new int[xSize, ySize];
            for (int i = 0; i < zSize; i++)
            {
                doseEsapi.GetVoxels(i, voxelValuesInPlane);

                for (int j = 0; j < ySize; j++)
                {
                    for (int k = 0; k < xSize ; k++)
                    {
                        int voxelValue = voxelValuesInPlane[k, j];
                        var doseValue = doseEsapi.VoxelToDoseValue(voxelValue);
                        dose3dArray[i, j, k] = doseValue.Dose;
                    }
                }
            }

            return dose3dArray;
        }

        /// <summary>
        /// ESAPI user coordinate system to the DICOM coordinate system.
        /// </summary>
        /// <param name="x">The x coordinate in the User coordinate system.</param>
        /// <param name="y">The y coordinate in the User coordinate system.</param>
        /// <param name="z">The z coordinate in the User coordinate system.</param>
        /// <param name="planSetup">ESAPI PlanSetup, which is necessary to convert from the User to DICOM coordinate system.</param>
        /// <returns>Coordinates in the DICOM coordinate system.</returns>
        public static double[] UserToDicomCoordinates(double x, double y, double z, PlanSetup planSetup)
        {
            var strucureSet = planSetup.StructureSet;
            var imageEsapi = strucureSet.Image;
            var vectorInUCS = new VVector(x, y, z);
            var vectorInDCS = imageEsapi.UserToDicom(vectorInUCS, planSetup);

            return new double[] { vectorInDCS.x, vectorInDCS.y, vectorInDCS.z };
        }

        /// <summary>
        /// ESAPI DICOM coordinate system to the user coordinate system.
        /// </summary>
        /// <param name="x">The x coordinate in the DICOM coordinate system.</param>
        /// <param name="y">The y coordinate in the DICOM coordinate system.</param>
        /// <param name="z">The z coordinate in the DICOM coordinate system.</param>
        /// <param name="planSetup">ESAPI PlanSetup, which is necessary to convert from the DICOM to User coordinate system.</param>
        /// <returns>Coordinates in the User coordinate system.</returns>
        public static double[] DicomToUserCoordinates(double x, double y, double z, PlanSetup planSetup)
        {
            var strucureSet = planSetup.StructureSet;
            var imageEsapi = strucureSet.Image;
            var vectorInDCS = new VVector(x, y, z);
            var vectorInUCS = imageEsapi.DicomToUser(vectorInDCS, planSetup);

            return new double[] { vectorInUCS.x, vectorInUCS.y, vectorInUCS.z };
        }
    }
}
