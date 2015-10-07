using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models;
using System.Diagnostics;
using System.IO;
using GeospaceEntity.Common;
using GeospaceEntity.Repositories;

namespace GeospaceEntity.Helper
{
    public static class HelperTrack
    {
        public static void Start(List<string> log, List<string> output, string param, string addSourcesPath, string exePath, bool run = false, bool build = false)
        {
            string sourcesPath = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources";

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.EnableRaisingEvents = true;
            //cmd.OutputDataReceived += new DataReceivedEventHandler(Output_Data_Received);           
            cmd.OutputDataReceived += (sender, e) => { Output_Data_Received(sender, e, log, output ); };
            cmd.ErrorDataReceived += (sender, e) => { Error_Data_Received(sender, e, log); };
            
            cmd.Start();
            cmd.BeginOutputReadLine();
            cmd.BeginErrorReadLine();

            if (build) Build_Sources(cmd, sourcesPath, addSourcesPath, exePath);
            if (run) Run(cmd, exePath, param);

            cmd.StandardInput.WriteLine(@"exit");
            
            cmd.WaitForExit();
            cmd.Close();
        }

        public static void Build_Sources(Process cmd, string sourcesPath, string addSourcesPath, string exePath)
        {            
            string commands2 = "ifort " + sourcesPath + addSourcesPath + " " + sourcesPath + "\\*.f90 /exe:" + exePath;   
            cmd.StandardInput.WriteLine(@"cd " + sourcesPath);
            cmd.StandardInput.WriteLine(@"""C:\\Program Files (x86)\\Intel\\Composer XE 2015\\bin\\compilervars.bat"" intel64");
            cmd.StandardInput.WriteLine(@commands2);
            //cmd.WaitForExit();
        }

        public static void Run(Process cmd, string exePath, string param)
        {
            cmd.StandardInput.WriteLine(@exePath + " " + param);            
            //string s = cmd.StandardOutput.ReadToEnd();
        }

        static void Output_Data_Received(object sender, DataReceivedEventArgs e, List<string> log, List<string> output)
        {
            if (sender == null || e.Data == null) return;

            if (e.Data.IndexOf("DEBUG") >= 0)
            {
                log[0] += e.Data.Replace("DEBUG ", "") + "\n";
            }

            if (e.Data.IndexOf("ERROR") >= 0)
            {
                log[0] += e.Data + "\n";
            }

            if (e.Data.IndexOf("OUTPUT") >= 0)
            {
                string str = e.Data.Replace("OUTPUT ", "");
                if (str.IndexOf(" MUF ") >= 0)
                    output[0] += str.Replace(" MUF ", "").Trim() + ", ";
                if (str.IndexOf(" OPF ") >= 0)
                    output[1] += str.Replace(" OPF ", "").Trim() + ", ";
                if (str.IndexOf(" D ") >= 0)
                    output[2] += str.Replace(" D ", "").Trim() + " ";
                if (str.IndexOf(" CODE ") >= 0)
                    output[3] += str.Replace(" CODE ", "").Trim() + " ";
            }
        }

        static void Error_Data_Received(object sender, DataReceivedEventArgs e, List<string> output)
        {
            if (sender == null || e.Data == null) return;

            output[0] += "Error:\n" + e.Data + "\n\n";
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
