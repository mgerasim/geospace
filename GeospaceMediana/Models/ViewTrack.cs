using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System.IO;
using GeospaceEntity.Common;
using GeospaceEntity.Repositories;

namespace GeospaceMediana.Models
{
    public class ViewTrack
    {
        public List<Track> tracks;
        public List<double[,]> pointsO = new List<double[,]>();
        public List<double[,]> pointsP = new List<double[,]>();

        public ViewTrack(int calc = 0)
        {
            tracks = Track.GetAll();

            if( calc == 1)
            {
                IRepository<Settings> repo = new SettingsRepository();
                List<string> log = new List<string>();
                log.Add("");
                string exePath = repo.GetAll().Select(x => x.CalcTrack).ToList()[0];
                GeospaceEntity.Helper.HelperTrack.Start(log, null, null, "\\CalcTrack\\CalcTrack.f90", exePath, false, true);
                for (int i = 0; i < tracks.Count; i++ )
                {
                    Calc_Track(tracks[i], log, exePath);
                }
                StreamWriter sw = new StreamWriter("C:\\inetpub\\wwwroot\\mediana\\bin2\\log\\Calc_Track_log.txt");
                foreach (string s in log)
                    sw.WriteLine(s);
                sw.Close();
            }

            for (int i = 0; i < tracks.Count; i++)
            {
                double[,] o = tracks[i].Get_Points_O();
                double[,] p = tracks[i].Get_Points_P();

                pointsO.Add(o);                
                pointsP.Add(p);
            }
            
        }


        public void Calc_Track(Track track, List<string> log, string exePath)
        {
            List<string> output = new List<string>();
            output.Add("[");   //MUF
            output.Add("[");   //OPF
            output.Add("");    //параметры: D
            output.Add("");    //код станции
            output.Add("");    //долгота/широта точек отражения
            output.Add("");    //кол-во точек отражения, поглощения
            output.Add("");    //долгота/широта точек поглощения

            string param = track.PointA.Longitude.ToString().Replace(",", ".") + " "
            + track.PointA.Latitude.ToString().Replace(",", ".") + " "
            + track.PointB.Longitude.ToString().Replace(",", ".") + " "
            + track.PointB.Latitude.ToString().Replace(",", ".") + " ";

            GeospaceEntity.Helper.HelperTrack.Start(log, output, param, "\\CalcTrack\\CalcTrack.f90", exePath, true);

            if (output[2].Length > 0) track.lengthTrack = Convert.ToInt32(output[2]);
            if(output[4].Length > 0)
            {
                string[] str = output[4].Split();
                if (track.KTO > 0)
                {
                    track.lon1 = Convert.ToDouble(str[0].Replace(".", ","));
                    track.lat1 = Convert.ToDouble(str[1].Replace(".", ","));
                }
                if (track.KTO > 1)
                {
                    track.lon2 = Convert.ToDouble(str[2].Replace(".", ","));
                    track.lat2 = Convert.ToDouble(str[3].Replace(".", ","));
                }
                if (track.KTO > 2)
                {
                    track.lon3 = Convert.ToDouble(str[4].Replace(".", ","));
                    track.lat3 = Convert.ToDouble(str[5].Replace(".", ","));
                }
                if (track.KTO > 3)
                {
                    track.lon4 = Convert.ToDouble(str[6].Replace(".", ","));
                    track.lat4 = Convert.ToDouble(str[7].Replace(".", ","));
                }
                if (track.KTO > 4)
                {
                    track.lon5 = Convert.ToDouble(str[8].Replace(".", ","));
                    track.lat5 = Convert.ToDouble(str[9].Replace(".", ","));
                }
                if (track.KTO > 5)
                {
                    track.lon6 = Convert.ToDouble(str[10].Replace(".", ","));
                    track.lat6 = Convert.ToDouble(str[11].Replace(".", ","));
                }
            }

            if (output[5].Length > 0)
            {
                string[] str = output[5].Split();
                if (str[0].Length > 0) track.KTO = Convert.ToInt32(str[0]);
                if (str[1].Length > 0) track.KTP = Convert.ToInt32(str[1]);                
            }

            if (output[6].Length > 0)
            {
                string[] str = output[6].Split();
                if (track.KTP > 0)
                {
                    track.lonP1 = Convert.ToDouble(str[0].Replace(".", ","));
                    track.latP1 = Convert.ToDouble(str[1].Replace(".", ","));
                }
                if (track.KTP > 1)
                {
                    track.lonP2 = Convert.ToDouble(str[2].Replace(".", ","));
                    track.latP2 = Convert.ToDouble(str[3].Replace(".", ","));
                }
                if (track.KTP > 2)
                {
                    track.lonP3 = Convert.ToDouble(str[4].Replace(".", ","));
                    track.latP3 = Convert.ToDouble(str[5].Replace(".", ","));
                }
                if (track.KTP > 3)
                {
                    track.lonP4 = Convert.ToDouble(str[6].Replace(".", ","));
                    track.latP4 = Convert.ToDouble(str[7].Replace(".", ","));
                }
                if (track.KTP > 4)
                {
                    track.lonP5 = Convert.ToDouble(str[8].Replace(".", ","));
                    track.latP5 = Convert.ToDouble(str[9].Replace(".", ","));
                }
                if (track.KTP > 5)
                {
                    track.lonP6 = Convert.ToDouble(str[10].Replace(".", ","));
                    track.latP6 = Convert.ToDouble(str[11].Replace(".", ","));
                }

                if (track.KTP > 6)
                {
                    track.lonP7 = Convert.ToDouble(str[12].Replace(".", ","));
                    track.latP7 = Convert.ToDouble(str[13].Replace(".", ","));
                }
                if (track.KTP > 7)
                {
                    track.lonP8 = Convert.ToDouble(str[14].Replace(".", ","));
                    track.latP8 = Convert.ToDouble(str[15].Replace(".", ","));
                }
                if (track.KTP > 8)
                {
                    track.lonP9 = Convert.ToDouble(str[16].Replace(".", ","));
                    track.latP9 = Convert.ToDouble(str[17].Replace(".", ","));
                }
                if (track.KTP > 9)
                {
                    track.lonP10 = Convert.ToDouble(str[18].Replace(".", ","));
                    track.latP10 = Convert.ToDouble(str[19].Replace(".", ","));
                }
                if (track.KTP > 10)
                {
                    track.lonP11 = Convert.ToDouble(str[20].Replace(".", ","));
                    track.latP11= Convert.ToDouble(str[21].Replace(".", ","));
                }
                if (track.KTP >11)
                {
                    track.lonP12 = Convert.ToDouble(str[22].Replace(".", ","));
                    track.latP12= Convert.ToDouble(str[23].Replace(".", ","));
                }
            }
            track.Update();
        }
    }
}