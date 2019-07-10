using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSIM_VEFR.FuzzySets
{
    /// <summary>
    /// The “Transmission distance” is defined as linguistic variable denoted by T with terms set T(T)={Near, intermediate, Far } as shown in Fig.5. Intermediate transmission distance from transmitter to receiver is more desirable as it ensures a reliable communication link by avoiding path-loss which is the attenuation of signal power that occurs as a radio wave propagates over a distance. Far transmission distance between the transmitter and receiver reduces the number of hops but it largely increases path-loss. Near transmission distance between the transmitter and receiver increases the number of hops and the communication overhead but it ensures a reliable communication link. Based on these facts, we defined three fuzzy sets of transmission distance as follows.
    /// </summary>
    public class TransmissiondistanceFuzzySet
    {
        double transCrisp;
        public TransmissiondistanceFuzzySet(double _transCrisp)
        {
            transCrisp = _transCrisp;
        }

        /// <summary>
        ///   T ̃_N={ (x,μ_N^T (x))|x∈[0,1]} defines Near fuzzy set where μ_N^T (x)  represents the Near membership function which is interpolated ,based our collected data, by modified Hill function with offset as formulated by Eq.(19). 
        /// </summary>
        public double Near
        {
            get
            {
                double x = transCrisp;
                double s = 1; double ε = 0; double k = 0.2; double n = Math.PI;
                double x_n = Math.Pow(x, n);
                double k_n = Math.Pow(k, n);
                double near = s + ((ε - s) * (x_n / (k_n + x_n)));
                return near;
            }
        }

        /// <summary>
        /// The intermediate fuzzy set is defined as T ̃_I={ (x,μ_I^T (x))|x∈[0,1]} in which μ_I^T (x) represents the membership function and interpolated by Voigt distribution which is a convolution of a Cauchy-Lorentz distribution and a Gaussian distribution as formulated in Eq.(20). 
        /// </summary>
        public double Intermediate
        {
            get
            {
                double x = transCrisp;
                double y0= 0.06895, xc= 0.5, w= 0.18579, A= 0.2047;


                // double intermediat = y0 + (A / (w * sqrt(pi / 2))) * exp(-2 * ((x - xc) / w) ^ 2)
                
                double intermediat = y0 + (A / (w * Math.Sqrt(Math.PI / 2))) * Math.Exp(-2 * Math.Pow((x - xc) / w, 2));
               
                


                return intermediat;
            }
        }

        /// <summary>
        /// The Far fuzzy set is defined as T ̃_F={ (x,μ_F^T (x))|x∈[0,1]} in which μ_F^T (x) represents the degree of membership and interpolated by Gumbel distribution as formulated in Eq.(21).
        /// </summary>
        public double Far
        {
            get
            {
                double x = transCrisp, a = 0.85, b = 0.16;
                double far = 1 - Math.Exp(-Math.Exp((x - a) / b));
                return far;
            }
        }

    }
}
