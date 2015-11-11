using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System.IO;
using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using GeospaceEntity.Helper;

namespace GeospaceMediana.Models
{
    public class ViewMonthForecastTrack
    {
        public int month;
        public int W;
        public int id;
        public List<Consumer> consumers;
        public List<List<string>> listOutput = new List<List<string>>();
        public List<string> title = new List<string>();
        public List<List<List<string>>> table = new List<List<List<string>>>();
        public int quantity = 0;
        public List<int> calcValue = new List<int>();
 
        public ViewMonthForecastTrack(int id_, int month_, int W_)
        {
            if (month_ < 0) month_ = DateTime.Now.Month;
            if (W_ < 0) W_ = 10;

            month = month_;
            W = W_;
            id = id_;

            consumers = Consumer.GetAll();
        }

        public void Calc( int id )
        {
            Consumer consumer = GeospaceEntity.Models.Consumer.GetById(id);
            DateTime dt = new DateTime(DateTime.Now.Year, month, 1);
            List<string> log = new List<string>();
            log.Add("");

            IRepository<Settings> repo = new SettingsRepository();
            string exePath = repo.GetAll().Select(x => x.MonthForecastTrack).ToList()[0];
            string DATADIR = repo.GetAll().Select(x => x.DATADIR).ToList()[0];

            quantity = consumer.Tracks.Count;
            //GeospaceEntity.Helper.HelperTrack.Start(log, null, null, "\\MonthForecast\\MonthForecast.f90", exePath, false, true);
            foreach (Track track in consumer.Tracks)
            {
                log[0] += track.Name + "\n";
                List<string> output = new List<string>();
                output.Add("[");   //MUF
                output.Add("[");   //OPF
                output.Add("");    //параметры: D

                string param = track.PointA.Longitude.ToString().Replace(",", ".") + " "
                + track.PointA.Latitude.ToString().Replace(",", ".") + " "
                + track.PointB.Longitude.ToString().Replace(",", ".") + " "
                + track.PointB.Latitude.ToString().Replace(",", ".") + " "
                + W.ToString() + " "
                    //+ DateTime.Now.AddMonths(1).Month.ToString();
                + month.ToString() + " "
                + DATADIR;

                GeospaceEntity.Helper.HelperTrack.Start(log, output, param, "\\MonthForecast\\MonthForecast.f90", exePath, true);

                output[0] += "]";
                output[1] += "]";


                string[] str = output[2].Split(' ');
                title.Add("Трасса: " + track.Name + "\nКоординаты: " + track.PointA.Longitude.ToString() + " "
                    + track.PointA.Latitude.ToString() + " — "
                    + track.PointB.Longitude.ToString() + " "
                    + track.PointB.Latitude.ToString() + "\n"
                    + "Длина трассы: " + str[0] + " км\n"
                    + "W: " + W.ToString() + "\n"
                    + "Прогноз на: " + dt.ToString("MMMM yyyy") + "\n\n");                

                string[] muf = output[0].Split();
                string[] opf = output[1].Split();
                List<List<string>> tab = new List<List<string>>();
                for (int i = 0; i <= 12; i++ )
                {
                    List<string> line = new List<string>();
                    line.Add(i.ToString());

                    string el = muf[i].Replace( "[", "");
                    el = el.Replace( ",", "");
                    el = el.Replace( "]", "");
                    if (el == "null") el = "—";
                    line.Add(el);                    

                    el = opf[i].Replace("[", "");
                    el = el.Replace(",", "");
                    el = el.Replace("]", "");
                    if (el == "null") el = "—";
                    line.Add(el);

                    line.Add((i+12).ToString());

                    el = muf[i+12].Replace("[", "");
                    el = el.Replace(",", "");
                    el = el.Replace("]", "");
                    if (el == "null") el = "—";
                    line.Add(el);

                    el = opf[i+12].Replace("[", "");
                    el = el.Replace(",", "");
                    el = el.Replace("]", "");
                    if (el == "null") el = "—";
                    line.Add(el);

                    tab.Add(line);
                }
                table.Add(tab);
                listOutput.Add(output);
                log[0] += "\n";
            }

            StreamWriter sw = new StreamWriter("C:\\inetpub\\wwwroot\\mediana\\bin2\\log\\MonthForecast_log.txt");
            foreach (string s in log)
                sw.WriteLine(s);
            sw.Close();
        }
    }
}