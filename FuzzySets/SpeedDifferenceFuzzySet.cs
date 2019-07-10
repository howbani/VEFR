using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSIM_VEFR.FuzzySets
{
    /// <summary>
    /// The least speed difference between the sender and the receiver enhances network performance as it keeps the inter-distance between the two vehicles within the transmission range which in turn allows more time to receive packets and reduces packet loss, especially when the size of the data packet is large. The vehicles with similar velocity stay longer together and closer to one another which reduces the beacon packets between them. The main goal is to capture the similarity of speed such that higher priority is assigned to the vehicle with the least speed difference. Let s_i, s_j and max⁡(s) be the speed of n_i, the speed of n_j, the maximum allowed speed, the speed difference between the  n_i and n_j simply normalized by Eq.(28).
    /// </summary>
    public class SpeedDifferenceFuzzySet
    {
        private double crisp;
        public SpeedDifferenceFuzzySet(double _crisp) { crisp = _crisp; }

        /// <summary>
        /// We defined the Small difference fuzzy set  by S ̃_S={ (x,μ_S^S (x))|x∈[0,1]} where μ_S^S (x) is the membership function which is interpolated by Boltzmann distribution as formulated in Eq.(29).
        /// </summary>
        public double Small
        {
            get
            {
                double x = crisp;
                double dx = 0.06, A2 = 0.11, x0 = 0.2;
                double small = A2 + ((1 - A2) / (1 + Math.Exp((x - x0) / dx)));
                return small;
            }
        }

        /// <summary>
        /// The Moderate speed difference is defined by the fuzzy set S ̃_M={ (x,μ_M^S (x))|x∈[0,1]} where μ_M^S (x) is the membership function interpolated by Gaussian process which is non-linear interpolation that used for fitting a curve through discrete data.as formulated in Eq.(30).
        /// </summary>
        public double Moderate
        {
            get
            {
                double x = crisp;
                double y0 = -0.10857, A = 0.6369, μ = 0.5, σ = 0.50166;
                double moderate = y0 + ((A / σ * Math.Sqrt(Math.PI / 2)) * (Math.Exp(-2 * (Math.Pow(((x - μ) / σ), 2)))));
                return moderate;
            }
        }

        /// <summary>
        /// The Large speed difference is defined by the fuzzy set S ̃_L={ (x,μ_L^S (x))|x∈[0,1]} where μ_L^S (x) is the membership function interpolated by logistic curve as formulated in Eq.(31).
        /// </summary>
        public double Large
        {
            get
            {
                double x = crisp,x0 = 0.816, p = 6.99, A2 = 1.2, A1 = 0.09219;
                double larg = A2 + ((A1 - A2) / (1 + Math.Pow(x / x0, p)));
                return larg;
            }
        }
    }
}
