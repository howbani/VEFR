using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSIM_VEFR.FuzzySets
{
    /// <summary>
    /// Segment connectivity is entirely correlated with vehicles density which is computed by combining multiple factors such as inter-distance of vehicles on the segment, transmission range of vehicle or roadside unit, number of lanes in the road segment, segment length and the number of vehicles. We Assume that each vehicle is equipped with OBU that holds a communication unit with a transmission range denoted by δ . Vehicle〖 n〗_i is able to communicate with a subsequent vehicle n_j and relays the packet if the inter-distance of 〖 n〗_i and 〖 n〗_j does not exceed δ. Here, we define the vehicles special density ρ ́_ij by Eq.(1) where γ denotes the number of lanes and E_ij^R denotes the expected number of vehicles in the road segment〖 r〗_(i,j), ι ̅ denotes the average length of the vehicle, and R_(i,j)⁡(k) denotes the probability that k vehicles reside within one lane in 〖 r〗_(i,j). 
    /// </summary>
    public class DensityFuzzySets
    {
        private double densityCrisp;
        /// <summary>
        /// A block is said to be connected if it occupied by least one vehicle. Therefore, the road segment is said to be connected if all blocks are occupied, computed by Eq. (5).
        /// </summary>
        /// <param name="densityInput"></param>
        public DensityFuzzySets(double densityInput)
        {
            densityCrisp = densityInput;
        }
        /// <summary>
        /// Low density membership function μ_L^D (x), Eq.(6), is interpolated by Boltzmann distribution
        /// </summary>
        public double Low
        {
            get
            {
                double x = densityCrisp;
                double x0 = 0.25;
                double dx = 0.05;
                double low = 1 / (1 + Math.Exp((x - x0) / dx));
                return low;
            }
        }

        /// <summary>
        /// Medium density membership function μ_M^D (x),Eq.(7), is interpolated by Gaussian process which is non-linear interpolation that used for fitting a curve through discrete data.
        /// </summary>
        public double Medium
        {
            get
            {
                double x = densityCrisp;
                double y0 = 0.0338; 
                double A = 0.38289;
                double μ = 0.5;
                double σ = 0.30099;
                double first = ((A / (σ * Math.Sqrt(Math.PI / 2))));
                double second = Math.Exp(-2 * Math.Pow(x - μ, 2) / Math.Pow(σ, 2));
                double medium = y0 + (first * second);
                return medium;
            }
        }

        /// <summary>
        /// The High density membership function μ_H^D, Eq.(8), is interpolated by the cumulative function of Weibull distribution.
        /// </summary>
        public double High
        {
            get
            {
                double x = densityCrisp;
                double a = 0.66, b = 5;
                double high = 1 - Math.Exp(-Math.Pow(x / a, b));
                return high;
            }
        }


    }
}
