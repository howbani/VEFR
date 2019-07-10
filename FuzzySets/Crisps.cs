using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using VSIM_VEFR.Operations;
using VSIM_VEFR.Properties;

namespace VSIM_VEFR.FuzzySets
{
    public static class Crisps
    {
        /// <summary>
        /// The main goal is to capture the similarity of speed such that higher priority is assigned to the vehicle with the least speed difference. Let s_i, s_j and max⁡(s) be the speed of n_i, the speed of n_j, the maximum allowed speed, the speed difference between the  n_i and n_j simply normalized by Eq.(28).
        /// </summary>
        /// <param name="si">speed of sender</param>
        /// <param name="sj">speed of potential reciver</param>
        /// <param name="maxSp">max speed allowed</param>
        /// <returns></returns>
        public static double SpeedDifference(double si, double sj, double maxSp)
        {
            return Math.Sqrt(Math.Pow(si - sj, 2)) / maxSp;
        }

        


        /// <summary>
        ///  is in the front
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="d">destination</param>
        /// <returns></returns>
        public static double MovingDirection(Point i, Point j, Point d)
        {
            double axb = ((j.X - i.X) * (d.X - i.X)) + ((j.Y - i.Y) * (d.Y - i.Y));
            double disMul = Computations.Distance(i, d) * Computations.Distance(i, j);
            double angale = Math.Acos(axb / disMul);
            double norAngle = angale / Math.PI;
            if (norAngle <= 0.5)
                return norAngle;
            else
                return 1;
        }
        /// <summary>
        /// x=√((x ̌_j-x ̌_i )^2+(y ̌_j-y ̌_i )^2 )⁄δ
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="comrange"></param>
        /// <returns></returns>
        public static double TransmissionDistance(Point i, Point j, double comrange)
        {
            double dis = Computations.Distance(i, j);
            if (dis <= comrange)
                return dis / comrange;
            else
                return 1;
        }

        /// <summary>
        /// density
        /// </summary>
        /// <param name="_RoadSegmentLength"></param>
        /// <param name="_VechilesCountInTheSegment"></param>
        /// <param name="_myComunicationRange"></param>
        /// <param name="_numberoflanes"></param>
        /// <returns></returns>
        public static double Density(double _RoadSegmentLength, double _VechilesCountInTheSegment, double _myComunicationRange, double _numberoflanes)
        {
            
            double pr = 0;
            double blockes = _RoadSegmentLength / (_myComunicationRange); // dived the raod segments into blockes
            double density = (2*_numberoflanes * _VechilesCountInTheSegment) / _RoadSegmentLength;
            pr = Math.Pow(1 - Math.Exp(-density), blockes);
            return pr;
        }

        /// <summary>
        /// valid distance.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ValidDistance(Point i, Point j, Point d)
        {
            // average of angle and dot angle.
            double angle = AngleDotProdection(i, j, d);
            if (angle != 1)
            {
                double normalized = ((0.4 * angle) + (0.6 * Length(i, j))) / 2;
                return normalized;
            }
            else 
            return 1;
        }


        /// <summary>
        /// d should be the heading distance of v.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double AngleDotProdection(Point i, Point j, Point d)
        {
            double axb = ((j.X - i.X) * (d.X - i.X)) + ((j.Y - i.Y) * (d.Y - i.Y));
            double disMul = Computations.Distance(i, d) * Computations.Distance(i, j);
            double angale = Math.Acos(axb / disMul);
            double norAngle = angale / Math.PI;
            if (norAngle <= 0.5)
               return norAngle;
            else
                return  1; // behind.
        }

        /// <summary>
        /// Givvs heigher priorit to the shortest.
        /// Road Segment Length: This distribution allocates higher probability to the longer segment. Longer segments are more preferable to avoid frequent segment switching that leads to delay the packet.  For the source junction〖 v〗_i, we define the normalized segment length random variable L ̅_i=(L ̅_(i,1),L ̅_(i,3),…L ̅_(i,b_i ) )  by Eq. (21). Furthermore, we define the segment length probability distribution, denoted by L ̃_i=(L ̃_(i,1),L ̃_(i,2)…L ̃_(i,b_i ) ), by the mass function as formulated in Eq. (22).
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static  double Length(Point i, Point j)
        {
          return  1 - (1 / Math.Log10(Computations.Distance(i, j)));
        }





    }
}
