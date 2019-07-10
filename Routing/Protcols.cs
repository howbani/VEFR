using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSIM_VEFR.Properties;
using static VSIM_VEFR.RoadNet.Components.VehicleUi;

namespace VSIM_VEFR.Routing
{
    public enum RoutingProtocol { HERO, VEFR };
    public class EnumConverter
    {
        /// <summary>
        /// convert string to  Vehicleinfo enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vehicleinfo VehicleinfoEnum(string value)
        {
            if (value != "")
            {
                Vehicleinfo m = (Vehicleinfo)Enum.Parse(typeof(Vehicleinfo), value, true);
                return m;
            }
            else
            {
                return Vehicleinfo.VID;
            }
        }

        /// <summary>
        /// convert string to RoutingProtocol
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RoutingProtocol RoutingProtocolEnum(string value)
        {
            if (value != "")
            {
                RoutingProtocol m = (RoutingProtocol)Enum.Parse(typeof(RoutingProtocol), value, true);
                return m;
            }
            else
            {
                return RoutingProtocol.VEFR;
            }
        }
    }

    public class Lister
    {
        public static List<string> ListProtocolsNames 
        {
            get
            {
                List<string> re = new List<string>();
                foreach (string str in Enum.GetNames(typeof(RoutingProtocol)))
                {
                    re.Add(str);
                }
                return re;
            }
        }
    }
}
