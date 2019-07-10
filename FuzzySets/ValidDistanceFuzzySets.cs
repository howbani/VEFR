using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSIM_VEFR.FuzzySets
{
    /// <summary>
    /// The intuition behind the concept of Valid Distance is the degree of closeness of source junction (i.e., junction toward which the source vehicle moves) to the heading junction (of destination vehicle). The Valid Distance Φ ̃_(i,j) , as formularized in Eq. (9), is obtained by combining two factors, the direction angle θ ̃_(i,j) and the length of the road segment L ̃_(i,j).
    /// </summary>
    public class ValidDistanceFuzzySets
    {
        private double  validDistanceCrisp;
        public ValidDistanceFuzzySets(double _distance)
        {
            validDistanceCrisp = _distance;
        }

        /// <summary>
        /// H ̃_C={ (x,μ_C^H (x))|x∈[0,1]} defines Close fuzzy set where μ_C^H (x)  is the Close membership function which is interpolated by Boltzmann distribution as formulated by Eq.(13).
        /// μ_C^H (x)={(1/(1+e^((x-x_0 )/dx) ) @x_0=0.25; dx=0.1;@x=Φ ̃_(i,j)  ;∀v_j∈V_i )┤   (13)
        /// </summary>
        public double Close
        {
            get
            {
                double x = validDistanceCrisp;
                double x0 =0.25, dx = 0.1;
                double close = 1 / (1 + Math.Exp((x - x0) / dx)); 
                return close;
            }
        }

        /// <summary>
        /// the Medium fuzzy set is defined by H ̃_M={ (x,μ_M^H (x))|x∈[0,1]} where μ_M^H (x)  denotes the Medium membership function which is interpolated by Cauchy–Lorentz distribution as formulated by Eq.(14). 
        /// μ_M^H (x)={(y+(2.A.w)/(w^2+4π.(x-x_0 )^2 )                       @〖y=-0.037;x〗_0=0.49; w=0.239;@A=0.354; x=Φ ̃_(i,j)  ;∀v_j∈V_i )┤   (14)
        /// </summary>
        public double Medium
        {
            get
            {
                double x = validDistanceCrisp;
                double y0 = -0.03785, xc = 0.49537, w = 0.23932, A = 0.3545;
                double pi = Math.PI;
                double y = y0 + (2 * A / pi) * (w / (4 * Math.Pow((x - xc), 2) + Math.Pow(w, 2)));
                return y;
            }
        }

        /// <summary>
        /// H ̃_F={ (x,μ_F^H (x))|x∈[0,1]} defines Far fuzzy set where μ_F^H (x) is Far membership function which is interpolated by cumulative function of Weibull distribution as formulated by Eq.(15).
        /// μ_F^H (x)={(φ.(1-e^(〖-(x/a)〗^b ) )@x>0;a=0.81;b=5.11;φ=0.9;@〖x=ξ ̃〗_(i,j)  ;∀v_j∈V_i )┤
        /// </summary>
        public double Far
        {
            get
            {
                double x = validDistanceCrisp;
                double a = 0.81, b = 5.11, φ = 0.9;
                double far = φ * (1 - Math.Exp(-Math.Pow(x / a, b)));
                return far;
            }
        }




    }
}
