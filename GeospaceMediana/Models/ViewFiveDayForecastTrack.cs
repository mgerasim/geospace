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
    public class ViewFiveDayForecastTrack
    {
        public int month;
        public int W;
        public List<Consumer> consumers;
        public List<List<string>> listOutput = new List<List<string>>();
        public List<string> title = new List<string>();
        public int quantity = 0;
 
        public ViewFiveDayForecastTrack(int month_, int W_)
        {
            if (month_ < 0) month_ = DateTime.Now.Month;
            if (W_ < 0) W_ = 10;

            month = month_;
            W = W_;

            consumers = Consumer.GetAll();
        }

        public void Calc( int id )
        {
            Consumer consumer = GeospaceEntity.Models.Consumer.GetById(id);
            DateTime dt = new DateTime(DateTime.Now.Year, month, 1);
            List<string> log = new List<string>();
            List<Station> allStation = Station.GetAll();
            log.Add("");

            IRepository<Settings> repo = new SettingsRepository();
            string exePath = repo.GetAll().Select(x => x.FiveDayForecastTrack).ToList()[0];
            string path;
            int num;

            num = exePath.LastIndexOf("\\");
            path = exePath.Substring(0, num + 1);
            path += "share.txt";

            quantity = consumer.Tracks.Count;

            StreamWriter sw = new StreamWriter(path );
            sw.Write(allStation.Count.ToString() + "\n");
            foreach( Station stat in allStation )
            {
                sw.Write(stat.ID.ToString() + ", " + stat.Longitude.ToString().Replace(",", ".") + ", " + stat.Latitude.ToString().Replace(",", ".") + "\n");
            }
            sw.Close();

            foreach (Track track in consumer.Tracks)
            {
                log[0] += track.Name + "\n";
                List<string> output = new List<string>();
                output.Add("[");   //MUF
                output.Add("[");   //OPF
                output.Add("");    //параметры: D
                output.Add("");    //ID станции

                string param = "0 " + track.PointA.Longitude.ToString().Replace(",", ".") + " "
                + track.PointA.Latitude.ToString().Replace(",", ".") + " "
                + track.PointB.Longitude.ToString().Replace(",", ".") + " "
                + track.PointB.Latitude.ToString().Replace(",", ".") + " "
                + path;

                GeospaceEntity.Helper.HelperTrack.Start(log, output, param, "\\FiveDayForecast\\FiveDayForecast.f90", exePath, true, true);

                param = "1 " + track.PointA.Longitude.ToString().Replace(",", ".") + " "
                + track.PointA.Latitude.ToString().Replace(",", ".") + " "
                + track.PointB.Longitude.ToString().Replace(",", ".") + " "
                + track.PointB.Latitude.ToString().Replace(",", ".") + " "
                + W.ToString() + " "
                    //+ DateTime.Now.AddMonths(1).Month.ToString();
                + month.ToString();

                output[0] += "]";
                output[1] += "]";


                string[] str = output[2].Split(' ');
                title.Add(track.Name + "\nКоординаты: " + track.PointA.Longitude.ToString() + " "
                    + track.PointA.Latitude.ToString() + " - "
                    + track.PointB.Longitude.ToString() + " "
                    + track.PointB.Latitude.ToString() + "\n"
                    + "Длина трассы: " + str[0] + " км\n"
                    + "Прогноз на: " + dt.ToString("MMM yyyy"));

                listOutput.Add(output);
                log[0] += "\n";
            }

            sw = new StreamWriter("C:\\inetpub\\wwwroot\\mediana\\bin2\\FiveDayForecast_log.txt");
            foreach (string s in log)
                sw.WriteLine(s);
            sw.Close();
        }
    }
}