using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models;
using System.Diagnostics;
using System.IO;

namespace GeospaceEntity.Helper
{
    public static class HelperTrack
    {
        public static void Start( List<string> output, string param )
        {
            string sourcesPath = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources";
            string exePath = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Build\\track.exe";

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.EnableRaisingEvents = true;
            //cmd.OutputDataReceived += new DataReceivedEventHandler(Output_Data_Received);           
            cmd.OutputDataReceived += (sender, e) => { Output_Data_Received(sender, e, output ); };
            
            cmd.Start();
            cmd.BeginOutputReadLine();

            Build_Sources(cmd, sourcesPath, exePath);
            Run(cmd, exePath, param);

            cmd.StandardInput.WriteLine(@"exit");
            cmd.WaitForExit();
            cmd.Close();
        }

        public static void Build_Sources(Process cmd, string sourcesPath, string exePath)
        {            
            string commands = "gfortran -static " + sourcesPath + "\\*.f95 -o " + exePath + "\n";            
            cmd.StandardInput.WriteLine(@commands);
            //cmd.WaitForExit();
        }

        public static void Run(Process cmd, string exePath, string param)
        {
            cmd.StandardInput.WriteLine(@exePath + " " + param);
            
            //string s = cmd.StandardOutput.ReadToEnd();
        }
 
        static void Output_Data_Received(object sender, DataReceivedEventArgs e, List<string> output )
        {
            if (sender == null || e.Data == null) return;

            if (e.Data.IndexOf("DEBUG") >= 0)
            {
                output[0] += e.Data.Replace("DEBUG ", "");
            }
            if (e.Data.IndexOf("ERROR") >= 0)
            {
                output[1] += e.Data.Replace("ERROR ", "");
            }
                //Console.WriteLine(e.Data);

            //if (e.Data.IndexOf("RETURN") >= 0 )
        }

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
