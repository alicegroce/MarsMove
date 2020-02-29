using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsMove
{
    class Formul
    {
        public static double Root(double x, double n)
        {
            return Math.Pow(x, 1.0 / n);
        }
        public static double SemimajorAxis(double mu, double T)
        {
            return Root(mu / Math.Pow(2 * Math.PI / T, 2), 3);
        }

        public static double MeanMotion(double T)
        {
            return 2 * Math.PI / T;
        }

        public static double MeanAnomaly(double E, double e)
        {
            return E - e * Math.Sin(E);
        }

        public static double EccentricAnomaly(double e, double theta)
        {
            return 2 * Math.Atan(Math.Sqrt((1 - e) / (1 + e)) * Math.Tan(theta / 2));
        }

        public static double PositionVectorLength(double a, double e, double E)
        {
            return a * (1 - e * Math.Cos(E));
        }

        public static double OrbitalVelocity(double mu, double r, double a)
        {
            return Math.Sqrt(mu * (2 / r - 1 / a));
        }
        public static double MeanAnomaly(double M0, double n, double t, double t0)
        {
            return M0 + n * (t - t0);
        }

        private static double NextE(double E, double M, double e)
        {
            return E - (E - e * Math.Sin(E) - M) / (1 + e * Math.Cos(E));
        }
        public static double AboutEccentricAnomaly(double e, double M)
        {
            int i = 0;
            double En = M + e * Math.Sin(M) * (1 + e * Math.Cos(M));
            for (i = 0; i < 5; i++)
            {
                En = NextE(En, M, e);
            }

            double En_plus_one = NextE(En, M, e);
            double diff = Math.Abs(En_plus_one - En);

            i = 0;
            while (true)
            {
                En = En_plus_one;
                En_plus_one = NextE(En, M, e);
                double new_diff = Math.Abs(En_plus_one - En);
                if (new_diff > diff || diff < 0.00000001 || i > 1000000)
                {
                    break;
                }
                diff = new_diff;
                i++;
            }

            return En;
        }

        public static double TrueAnomaly(double e, double E)
        {
            return 2 * Math.Atan(Math.Sqrt((1 + e) / (1 - e)) * Math.Tan(E / 2));
        }
    }
}
