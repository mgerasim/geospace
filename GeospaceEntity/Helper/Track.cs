using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models;

namespace GeospaceEntity.Helper
{
    public static class Track
    {
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public static double Delta_Sigma(Post a, Post b)
        {
            return Math.Atan2( Math.Sqrt( Math.Pow( Math.Cos( b.Latitude )*Math.Sin(b.Longitude-a.Longitude),2)
                + Math.Pow(Math.Cos(a.Latitude) * Math.Sin(b.Latitude) - Math.Sin(a.Latitude)*Math.Cos(b.Latitude)
                * Math.Cos(b.Longitude - a.Longitude), 2))
                , Math.Sin(a.Latitude) * Math.Sin(b.Latitude) + Math.Cos(a.Latitude)
                * Math.Cos(b.Latitude) * Math.Cos(b.Longitude - a.Longitude));
        }
        public static void Calc_Track( Post a, Post b, List<Post> listPost, ref double lenght, ref double angle )
        {
            double r = 6372.795;

            Post A = new Post();
            Post B = new Post();

            A.Longitude = DegreeToRadian(a.Longitude);
            A.Latitude = DegreeToRadian(a.Latitude);

            B.Longitude = DegreeToRadian(b.Longitude);
            B.Latitude = DegreeToRadian(b.Latitude);

            double deltaSigma = Delta_Sigma(A, B);
            lenght = deltaSigma * r;
            Console.WriteLine(deltaSigma.ToString());
        }
    }
}
