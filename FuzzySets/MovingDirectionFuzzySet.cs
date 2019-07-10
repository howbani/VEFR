using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSIM_VEFR.FuzzySets
{
    /// <summary>
    /// Note that the road segment selection is achieved by RSS, explained in the previous subsection. Given a road segment r_(s,d)=(〖 v〗_s, 〖 v〗_d), where v_s denotes the start junction and〖 v〗_d denotes the end junction. Let n_i be the a vehicle which has packet, and is currently moving over  an estimated position (x ̌_i,y ̌_i) on r_(s,d). The current sender n_i  selects one of its neighbors to be the relay. The relay vehicle 〖n_j∈N〗_i  should be in front of n_i without considering its heading direction(same direction or opposite direction). That is to say, the heading direction of the relay vehicle n_j is not important, since the packet should be travelled from the start junction v_s to the end junction v_d. Let ϑ∈[0,1] be the normalized angle between n_i to 〖 v〗_d  and n_i the relay 〖n_j∈N〗_i as formulated by Eq.(27). When  0≤ϑ≤0.5, the relay node n_(j )is located in front of n_i. Otherwise, n_(j )is located behind n_i.
    /// </summary>
    public class MovingDirectionFuzzySet
    {
        private double crisp;
        public MovingDirectionFuzzySet(double _crisp)
        {
            crisp = _crisp;
        }
        /// <summary>
        /// Front and Behind fuzzy sets respectively. Both fuzzy sets are depicted in Fig.6. Front μ_F^M (ϑ) and Behind μ_B^M (ϑ) membership functions are interpolated ,based our collected data, by logistic curve as formulated in Eq.(28) and Eq.(29), respectively. Note that ϑ is obtained by Eq.(27).
        /// </summary>
        public double Front
        {
            get
            {
                double x = crisp;
                double x0 = 0.33, p = 3.8;
                double front = 1 / (1 + (Math.Pow(x / x0, p)));
                return front;
            }
        }

        /// <summary>
        /// Front and Behind fuzzy sets respectively. Both fuzzy sets are depicted in Fig.6. Front μ_F^M (ϑ) and Behind μ_B^M (ϑ) membership functions are interpolated ,based our collected data, by logistic curve as formulated in Eq.(28) and Eq.(29), respectively. Note that ϑ is obtained by Eq.(27).
        /// </summary>
        public double Behind
        {
            get
            {
                double x = crisp;
                double x0 = 0.66, p = 9.2, A1=0.01;
                double behind = 1 + ((A1 - 1) / (1 + (Math.Pow(x / x0, p))));
                return behind;
            }
        }

    }
}
