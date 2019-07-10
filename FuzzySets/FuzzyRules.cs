using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using VSIM_VEFR.Properties;

namespace VSIM_VEFR.FuzzySets
{

    /// <summary>
    /// To identify the relations among premises involved, a weight for each premise is obtained by employing Analytical Hierarchy Process (AHP) [2] which is an effective tool to deal with complex decision making by setting priorities for each attribute. The AHP is implemented by consecutive two steps: constructing pairwise comparison matrix, and priority rating. More details about these two steps are found in [1]. Based on AHP, the pairwise comparison matrix (m*m) is defined by Eq. (16), in which the entry〖 a〗_jk  represents the importance of the jth criteria relative to the kth. For example, the entry a_1,2=5 means that the importance of medium density is 5 times compare to the low density.  Note that a_jk.a_kj=1. We included 6 sub-criteria in A^* such that each column contains 6 rows representing Medium density(M), Low density(L), High density(H), Close distance(C), Medium distance(MV) and Far distance(F). We derived the normalized pairwise comparison matrix such that each entry in A^* is normalized by Eq.(17). At the end, the criteria weight vector W=[M, L, H, C, MV, F] is built by averaging entries on each row on the normalized pairwise comparison matrix as formulated in Eq. (17).
    /// </summary>
    public class Weights
    {
        public static double DensityMedium = 0.313;
        public static double DensityLow = 0.0547;
        public static double DensityHigh = 0.0925;

        public static double ValidDistanceClose = 0.3616;
        public static double ValidDistanceMedium = 0.130;
        public static double ValidDistanceFar = 0.04;


        public static double TransmissionDistanceNear = 0.217;
        public static double TransmissionDistanceIntermediate = 0.106;
        public static double TransmissionDistanceFar = 0.044;



        public static double SpeedDifferenceSmall = 0.216;
        public static double SpeedDifferenceModerate = 0.09;
        public static double SpeedDifferenceLarge = 0.04;

        public static double MovingDirectionFront = 0.5;// 0.233;
        public static double MovingDirectionBehind = 0.037;

    }

    // ITEM STETS
    public enum Density { Medium, Low, High }
    public enum ValidDistance { Close, Medium, Far }
    public enum TransmissionDistance { Near, Intermediate, Far }
    public enum MovingDirection { Front, Behind }
    public enum SpeedDifference { Small, Moderate, Large }

    /// <summary>
    /// TSK inference: recall that the Relay Vehicle Selection(RVS) as shown in Fig.2 is a problem of aggregating multi-criteria to form overall decision function.  RVS decision function encapsulates three criteria, Transmission Distance(TM), Speed Difference(SD), and Moving Direction(MD). To infer the final forwarding priority, we feed the rules shown in Table 3 to TSK inference system that yields a crisp output directly without defuzzification process. Like used the same type of rules as in RSS, explained above.  To identify the relations among premises (Transmission Distance, Speed Difference, and Moving Direction) involved, we determined a weigh for each premise by employing Analytical Hierarchy Process. The pairwise comparison matrix (m*m) is defined by Eq. (32). We included 8 sub-criteria in A^* such that each column contains 8 rows representing Near transmission distance(N), Intermediate transmission distance (I), Far transmission distance (F), Small speed difference (S), Moderate speed difference (M), Large speed difference (L), Front moving direction(FR) and Behind moving direction(B).
    /// </summary>
    public class RVSInput
    {
        public int ID { get; set; }
        public Point CurrentVehlocation { get; set; }  
        public Point CandidateVehlocation { get; set; }
        public Point DesVehlocation { get; set; }  
        public double CurrentVehSpeedInKMH { get; set; }
        public double CandidateVehSpeedInKMH { get; set; }
        public double MaxSpeed => Settings.Default.MaxSpeed;
        public double CommunicationRange => Settings.Default.CommunicationRange;
        public TransmissionDistance TransmissionDistance
        {
            get
            {
                if (TransmissionDistanceCrisp >= 0 && TransmissionDistanceCrisp <= 0.29)
                {
                    return TransmissionDistance.Near;
                }
                else if (TransmissionDistanceCrisp >= 0.29 && TransmissionDistanceCrisp <= 0.69)
                {
                    return TransmissionDistance.Intermediate;
                }
                else
                {
                    return TransmissionDistance.Far;
                }
            }
        }

        public SpeedDifference SpeedDifference
        {
            get
            {
                if (SpeedDifferenceCrisp >= 0 && SpeedDifferenceCrisp <= 0.20)
                {
                    return SpeedDifference.Small;
                }
                else if (SpeedDifferenceCrisp >= 0.20 && SpeedDifferenceCrisp <= 0.8)
                {
                    return SpeedDifference.Moderate;
                }
                else
                {
                    return SpeedDifference.Large;
                }
            }
        }
        public MovingDirection MovingDirection
        {
            get
            {
                if (MovingDirectionCrisp >= 0 && MovingDirectionCrisp <= 0.50)
                {
                    return MovingDirection.Front;
                }
                else
                {
                    return MovingDirection.Behind;
                }
            }
        }
        public double TransmissionDistanceCrisp { get; set; }
        public double SpeedDifferenceCrisp { get; set; }
        public double MovingDirectionCrisp
        {
            get;set;
        }
        public double Priority
        {
            get
            {
                return RVSRuleBase.Aggregate(this);
            }
        }
    }


    /// <summary>
    /// input the two Crisp.
    /// </summary>
    public class RSSInput
    {
        public double DensityCrisp { get; set; }
        public Density Density
        {
            get
            {
                if (DensityCrisp >= 0 && DensityCrisp <= 0.25)
                {
                    return Density.Low;
                }
                else if (DensityCrisp >= 0.25 && DensityCrisp <= 0.65)
                {
                    return Density.Medium;
                }
                else
                {
                    return Density.High;
                }
            }
        }
        public double ValidDistanceCrisp { get; set; }
        public ValidDistance ValidDistance
        {
            get
            {
                if (ValidDistanceCrisp >= 0 && ValidDistanceCrisp <= 0.32)
                {
                    return ValidDistance.Close;
                }
                else if (ValidDistanceCrisp >= 0.32 && ValidDistanceCrisp <= 0.62)
                {
                    return ValidDistance.Medium;
                }
                else
                {
                    return ValidDistance.Far;
                }
            }
        }
    }

    /// <summary>
    /// TSK inference: recall that the Road Segment Selection (RSS) as shown in Fig.1 is a problem of aggregating mul-ti-criteria to form overall decision function.  RSS decision function encapsulates two criteria, valid distance and density. In order to formulate a fuzzy multi-objective aggregation for road segment, we designed RSS rules as listed in Table 2.
    /// </summary>
    public class RSSRuleBase
    {
        /// <summary>
        /// The inferred output of the TSK model with z≥1 rules each with k≥1 premises is obtained by Eq. (18). Note that F_i (x_1,…,x_k )=w_0^i+w_1^i x_1+w_2^i x_2+…+w_k^i x_k and  G_i (x_1,…,x_k ) is t-norm usually implemented as max/min fuzzy operators.   
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Aggregate(RSSInput input)
        {
            DensityFuzzySets denFuzzy = new DensityFuzzySets(input.DensityCrisp);
            ValidDistanceFuzzySets VlaidFuzzy = new ValidDistanceFuzzySets(input.ValidDistanceCrisp);

            if (input.Density == Density.Medium && input.ValidDistance == ValidDistance.Close) // 1
            {

                double re = (Weights.DensityMedium * denFuzzy.Medium) + (Weights.ValidDistanceClose * VlaidFuzzy.Close);
                return re;
            }
            else if (input.Density == Density.Medium && input.ValidDistance == ValidDistance.Medium) //2
            {

                double re = (Weights.DensityMedium * denFuzzy.Medium) + (Weights.DensityMedium * VlaidFuzzy.Medium);
                return re;
            }
            else if (input.Density == Density.Medium && input.ValidDistance == ValidDistance.Far) //3
            {

                double re = (Weights.DensityMedium * denFuzzy.Medium) + (Weights.ValidDistanceFar * VlaidFuzzy.Far);
                return re;
            }
            else if (input.Density == Density.Low && input.ValidDistance == ValidDistance.Close) //4
            {

                double re = (Weights.DensityLow * denFuzzy.Low) + (Weights.ValidDistanceClose * VlaidFuzzy.Close);
                return re;
            }
            else if (input.Density == Density.Low && input.ValidDistance == ValidDistance.Medium) //5
            {

                double re = (Weights.DensityLow * denFuzzy.Low) + (Weights.ValidDistanceMedium * VlaidFuzzy.Medium);
                return re;
            }
            else if (input.Density == Density.Low && input.ValidDistance == ValidDistance.Far) //6
            {

                double re = (Weights.DensityLow * denFuzzy.Low) + (Weights.ValidDistanceFar * VlaidFuzzy.Far);
                return re;
            }
            else if (input.Density == Density.High && input.ValidDistance == ValidDistance.Close)//7
            {

                double re = (Weights.DensityHigh * denFuzzy.High) + (Weights.ValidDistanceClose * VlaidFuzzy.Close);
                return re;
            }
            else if (input.Density == Density.High && input.ValidDistance == ValidDistance.Medium)//8
            {

                double re = (Weights.DensityHigh * denFuzzy.High) + (Weights.ValidDistanceMedium * VlaidFuzzy.Medium);
                return re;
            }
            else if (input.Density == Density.High && input.ValidDistance == ValidDistance.Far)//9
            {

                double re = (Weights.DensityHigh * denFuzzy.High) + (Weights.ValidDistanceFar * VlaidFuzzy.Far);
                return re;
            }
            return 0;
        }
    }


    /// <summary>
    /// TSK inference: recall that the Relay Vehicle Selection(RVS) as shown in Fig.2 is a problem of aggregating multi-criteria to form overall decision function.  RVS decision function encapsulates three criteria, Transmission Distance(TM), Speed Difference(SD), and Moving Direction(MD). To infer the final forwarding priority, we feed the rules shown in Table 3 to TSK inference system that yields a crisp output directly without defuzzification process. Like used the same type of rules as in RSS, explained above.  To identify the relations among premises (Transmission Distance, Speed Difference, and Moving Direction) involved, we determined a weigh for each premise by employing Analytical Hierarchy Process. The pairwise comparison matrix (m*m) is defined by Eq. (32). We included 8 sub-criteria in A^* such that each column contains 8 rows representing Near transmission distance(N), Intermediate transmission distance (I), Far transmission distance (F), Small speed difference (S), Moderate speed difference (M), Large speed difference (L), Front moving direction(FR) and Behind moving direction(B).
    /// </summary>
    public static class RVSRuleBase
    {
        public static double Aggregate(RVSInput i)
        {
            TransmissiondistanceFuzzySet tdf = new TransmissiondistanceFuzzySet(i.TransmissionDistanceCrisp);
            SpeedDifferenceFuzzySet sdf = new SpeedDifferenceFuzzySet(i.SpeedDifferenceCrisp);
            MovingDirectionFuzzySet md = new MovingDirectionFuzzySet(i.MovingDirectionCrisp);

            //1
            if (i.TransmissionDistance == TransmissionDistance.Near && i.SpeedDifference == SpeedDifference.Small && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceNear * tdf.Near) + (Weights.SpeedDifferenceSmall * sdf.Small) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceNear + Weights.SpeedDifferenceSmall + Weights.MovingDirectionFront;
                return re;
            }
            //2
            else if (i.TransmissionDistance == TransmissionDistance.Near && i.SpeedDifference == SpeedDifference.Small && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceNear * tdf.Near) + (Weights.SpeedDifferenceSmall * sdf.Small) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceNear + Weights.SpeedDifferenceSmall + Weights.MovingDirectionBehind;
                return re;
            }
            //3
            else if (i.TransmissionDistance == TransmissionDistance.Near && i.SpeedDifference == SpeedDifference.Moderate && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceNear * tdf.Near) + (Weights.SpeedDifferenceModerate * sdf.Moderate) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceNear + Weights.SpeedDifferenceModerate + Weights.MovingDirectionFront;
                return re;

            }
            //4
            else if (i.TransmissionDistance == TransmissionDistance.Near && i.SpeedDifference == SpeedDifference.Moderate && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceNear * tdf.Near) + (Weights.SpeedDifferenceModerate * sdf.Moderate) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceNear + Weights.SpeedDifferenceModerate + Weights.MovingDirectionBehind;
                return re;

            }
            //5
            else if (i.TransmissionDistance == TransmissionDistance.Near && i.SpeedDifference == SpeedDifference.Large && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceNear * tdf.Near) + (Weights.SpeedDifferenceLarge * sdf.Large) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceNear + Weights.SpeedDifferenceLarge + Weights.MovingDirectionFront;
                return re;
            }
            //6
            else if (i.TransmissionDistance == TransmissionDistance.Near && i.SpeedDifference == SpeedDifference.Large && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceNear * tdf.Near) + (Weights.SpeedDifferenceLarge * sdf.Large) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceNear + Weights.SpeedDifferenceLarge + Weights.MovingDirectionBehind;
                return re;
            }
            //7
            else if (i.TransmissionDistance == TransmissionDistance.Intermediate && i.SpeedDifference == SpeedDifference.Small && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceIntermediate * tdf.Intermediate) + (Weights.SpeedDifferenceSmall * sdf.Small) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceIntermediate + Weights.SpeedDifferenceSmall + Weights.MovingDirectionFront;
                return re;
            }
            //8
            else if (i.TransmissionDistance == TransmissionDistance.Intermediate && i.SpeedDifference == SpeedDifference.Small && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceIntermediate * tdf.Intermediate) + (Weights.SpeedDifferenceSmall * sdf.Small) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceIntermediate + Weights.SpeedDifferenceSmall + Weights.MovingDirectionBehind;
                return re;
            }
            //9
            else if (i.TransmissionDistance == TransmissionDistance.Intermediate && i.SpeedDifference == SpeedDifference.Moderate && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceIntermediate * tdf.Intermediate) + (Weights.SpeedDifferenceModerate * sdf.Moderate) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceIntermediate + Weights.SpeedDifferenceModerate + Weights.MovingDirectionFront;
                return re;
            }
            //10
            else if (i.TransmissionDistance == TransmissionDistance.Intermediate && i.SpeedDifference == SpeedDifference.Moderate && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceIntermediate * tdf.Intermediate) + (Weights.SpeedDifferenceModerate * sdf.Moderate) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceIntermediate + Weights.SpeedDifferenceModerate + Weights.MovingDirectionBehind;
                return re;
            }
            //11
            else if (i.TransmissionDistance == TransmissionDistance.Intermediate && i.SpeedDifference == SpeedDifference.Large && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceIntermediate * tdf.Intermediate) + (Weights.SpeedDifferenceLarge * sdf.Large) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceIntermediate + Weights.SpeedDifferenceLarge + Weights.MovingDirectionFront;
                return re;
            }
            //12
            else if (i.TransmissionDistance == TransmissionDistance.Intermediate && i.SpeedDifference == SpeedDifference.Large && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceIntermediate * tdf.Intermediate) + (Weights.SpeedDifferenceLarge * sdf.Large) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceIntermediate + Weights.SpeedDifferenceLarge + Weights.MovingDirectionBehind;
                return re;
            }
            //13
            else if (i.TransmissionDistance == TransmissionDistance.Far && i.SpeedDifference == SpeedDifference.Small && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceFar * tdf.Far) + (Weights.SpeedDifferenceSmall * sdf.Small) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceFar + Weights.SpeedDifferenceSmall + Weights.MovingDirectionFront;
                return re;
            }
            //14
            else if (i.TransmissionDistance == TransmissionDistance.Far && i.SpeedDifference == SpeedDifference.Small && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceFar * tdf.Far) + (Weights.SpeedDifferenceSmall * sdf.Small) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceFar + Weights.SpeedDifferenceSmall + Weights.MovingDirectionBehind;
                return re;
            }
            //15
            else if (i.TransmissionDistance == TransmissionDistance.Far && i.SpeedDifference == SpeedDifference.Moderate && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceFar * tdf.Far) + (Weights.SpeedDifferenceModerate * sdf.Moderate) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceFar + Weights.SpeedDifferenceModerate + Weights.MovingDirectionFront;
                return re;
            }
            //16
            else if (i.TransmissionDistance == TransmissionDistance.Far && i.SpeedDifference == SpeedDifference.Moderate && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceFar * tdf.Far) + (Weights.SpeedDifferenceModerate * sdf.Moderate) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceFar + Weights.SpeedDifferenceModerate + Weights.MovingDirectionBehind;
                return re;
            }
            //17
            else if (i.TransmissionDistance == TransmissionDistance.Far && i.SpeedDifference == SpeedDifference.Large && i.MovingDirection == MovingDirection.Front)
            {
                double re = (Weights.TransmissionDistanceFar * tdf.Far) + (Weights.SpeedDifferenceLarge * sdf.Large) + (Weights.MovingDirectionFront * md.Front);
                double sum = Weights.TransmissionDistanceFar + Weights.SpeedDifferenceLarge + Weights.MovingDirectionFront;
                return re;

            }
            //18
            else if (i.TransmissionDistance == TransmissionDistance.Far && i.SpeedDifference == SpeedDifference.Large && i.MovingDirection == MovingDirection.Behind)
            {
                double re = (Weights.TransmissionDistanceFar * tdf.Far) + (Weights.SpeedDifferenceLarge * sdf.Large) + (Weights.MovingDirectionBehind * md.Behind);
                double sum = Weights.TransmissionDistanceFar + Weights.SpeedDifferenceLarge + Weights.MovingDirectionBehind;
                return re;

            }




            return 0;
        }
    }
}
