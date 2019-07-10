using System;
using VSIM_VEFR.Operations;
using VSIM_VEFR.Properties;
using VSIM_VEFR.RoadNet.Components;

namespace VSIM_VEFR.Vpackets
{
    class SourceDestinationSelector
    {
        MainWindow _mw;
        public SourceDestinationSelector(MainWindow mw)
        {
            _mw = mw;
        }
        /// <summary>
        /// select the source vehicle randomly.
        /// </summary>
        public void RandomlySourceVechicle(bool isDistance, double distance)
        {
            // select random vehicle:
            if (Settings.Default.IsIntialized)
            {
                if (_mw.MyVehicles.Count > 0)
                {
                    // select the source:

                    int max = _mw.MyVehicles.Count;
                    if (max >= 2)
                    {
                        // consider the distance
                        if (isDistance)
                        {
                            int rand = Convert.ToInt16(RandomeNumberGenerator.GetUniform(max - 1));
                            VehicleUi src = _mw.MyVehicles[rand];
                            VehicleUi des = GetDestinationWithinAdistance(src, distance);
                            if (des != null)
                            {
                                src.GeneratePacket(des);
                            }
                        }
                        else
                        {
                            int rand = Convert.ToInt16(RandomeNumberGenerator.GetUniform(max - 1));
                            _mw.MyVehicles[rand].RandomDestinationVehicle();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// selet the source within the dis.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dis"></param>
        /// <returns></returns>
        public VehicleUi GetDestinationWithinAdistance(VehicleUi src, double dis)
        {
            foreach (VehicleUi des in _mw.MyVehicles)
            {
                double accualdistance = Computations.Distance(src.InstanceLocation, des.InstanceLocation);
                double thesould = 2 * Math.Sqrt(dis);
                double uper_tollerance = dis + thesould;
                double lower_tollerance = dis - thesould;
                if (accualdistance >= lower_tollerance && accualdistance <= uper_tollerance)
                {
                    return des;
                }
            }

            return null;
        }
    }
}
