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
        public int id;
        public List<Consumer> consumers;
        public List<List<string>> listOutput = new List<List<string>>();
        public List<string> title = new List<string>();
        public List<List<List<string>>> table = new List<List<List<string>>>();
        public int quantity = 0;
        public List<int> calcValue = new List<int>();
 
        public ViewFiveDayForecastTrack(int id_, int month_, int W_)
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
            List<string> log = new List<string>();
            List<Station> allStation = Station.GetAll();
            log.Add("");

            IRepository<Settings> repo = new SettingsRepository();
            string exePath = repo.GetAll().Select(x => x.FiveDayForecastTrack).ToList()[0];
            string DATADIR = repo.GetAll().Select(x => x.DATADIR).ToList()[0];     
            string path, statPath;
            int num;

            num = exePath.LastIndexOf("\\");
            path = exePath.Substring(0, num + 1);
            statPath = path + "stat_coord.txt";
            path += "share.txt";

            quantity = consumer.Tracks.Count;

            StreamWriter sw = new StreamWriter(statPath);
            sw.Write(allStation.Count.ToString() + "\n");
            foreach( Station stat in allStation )
            {
                sw.Write(stat.Code.ToString() + ", " + stat.Longitude.ToString().Replace(",", ".") + ", " + stat.Latitude.ToString().Replace(",", ".") + "\n");
            }
            sw.Close();

            //GeospaceEntity.Helper.HelperTrack.Start(log, null, null, "\\FiveDayForecast\\FiveDayForecast.f90", exePath, false, true);
            int k = 0;
            foreach (Track track in consumer.Tracks)
            {
                calcValue.Add(0);
                log[0] += track.Name + "\n";
                List<string> output = new List<string>();
                output.Add("[");   //MUF
                output.Add("[");   //OPF
                output.Add("");    //параметры: D
                output.Add("");    //код станции

                string param = "0 " + track.PointA.Longitude.ToString().Replace(",", ".") + " "
                + track.PointA.Latitude.ToString().Replace(",", ".") + " "
                + track.PointB.Longitude.ToString().Replace(",", ".") + " "
                + track.PointB.Latitude.ToString().Replace(",", ".") + " "
                + statPath;

                GeospaceEntity.Helper.HelperTrack.Start(log, output, param, "\\FiveDayForecast\\FiveDayForecast.f90", exePath, true);

                string[] str = output[3].Split(' ');
                title.Add("Трасса: " + track.Name + "\nКоординаты: " + track.PointA.Longitude.ToString() + " "
                        + track.PointA.Latitude.ToString() + " - "
                        + track.PointB.Longitude.ToString() + " "
                        + track.PointB.Latitude.ToString() + "\n");
                if (output[3].Length > 0)
                {
                    DateTime nowDateTime = DateTime.Now.AddDays(-1);
                    sw = new StreamWriter(path);
                    foreach (string item in str)
                    {
                        if (item.Length > 0)
                        {
                            int code = Convert.ToInt32(item);
                            List<int> avgF = Average.GetByDate(Station.GetByCode(code), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_10).ToList();
                            List<int> avgM = Average.GetByDate(Station.GetByCode(code), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_10).ToList();

                            bool flagF = true;
                            bool flagM = true;

                            foreach (int val in avgF)
                            {
                                if (val != 1000)
                                {
                                    flagF = false;
                                    break;
                                }
                            }

                            foreach (int val in avgM)
                            {
                                if (val != 1000)
                                {
                                    flagM = false;
                                    break;
                                }
                            }

                            if (avgF.Count == 0 || avgM.Count == 0 || flagF || flagM)
                            {
                                calcValue[k] = 1000;
                                for (int i = 1; i <= 48; i++)
                                {
                                    sw.Write("1000 ");
                                    if (i == 24 || i == 48) sw.Write("\n");
                                }
                            }
                            else
                            {
                                calcValue[k] = 0;
                                foreach (int a in avgF)
                                    sw.Write(a + " ");
                                sw.Write("\n");

                                foreach (int a in avgM)
                                    sw.Write(a + " ");
                                sw.Write("\n");
                            }
                        }
                    }
                    sw.Close();

                    param = "1 " + track.PointA.Longitude.ToString().Replace(",", ".") + " "
                    + track.PointA.Latitude.ToString().Replace(",", ".") + " "
                    + track.PointB.Longitude.ToString().Replace(",", ".") + " "
                    + track.PointB.Latitude.ToString().Replace(",", ".") + " "
                    + path + " "
                    + DATADIR + " "
                    + W.ToString() + " "
                    + month.ToString();

                    GeospaceEntity.Helper.HelperTrack.Start(log, output, param, "\\FiveDayForecast\\FiveDayForecast.f90", exePath, true);                    

                    str = output[2].Split(' ');
                    
                    title[k] += "Длина трассы: " + str[0] + " км\n"
                        + "W: " + W.ToString() + "\n"
                        + "Прогноз на: " + DateTime.Now.ToString("dd") + " — " + DateTime.Now.AddDays(4).ToString("dd MMM. yyyy г.");

                    string[] muf = output[0].Split();
                    string[] opf = output[1].Split();
                    List<List<string>> tab = new List<List<string>>();
                    for (int i = 0; i <= 12; i++)
                    {
                        List<string> line = new List<string>();
                        line.Add(i.ToString());

                        string el = muf[i].Replace("[", "");
                        el = el.Replace(",", "");
                        el = el.Replace("]", "");
                        if (el == "null") el = "—";
                        line.Add(el);

                        el = opf[i].Replace("[", "");
                        el = el.Replace(",", "");
                        el = el.Replace("]", "");
                        if (el == "null") el = "—";
                        line.Add(el);

                        line.Add((i + 12).ToString());

                        el = muf[i + 12].Replace("[", "");
                        el = el.Replace(",", "");
                        el = el.Replace("]", "");
                        if (el == "null") el = "—";
                        line.Add(el);

                        el = opf[i + 12].Replace("[", "");
                        el = el.Replace(",", "");
                        el = el.Replace("]", "");
                        if (el == "null") el = "—";
                        line.Add(el);

                        tab.Add(line);
                    }
                    table.Add(tab);
                }

                output[0] += "]";
                output[1] += "]";
                
                listOutput.Add(output);
                log[0] += "\n";
                k++;
            }

            sw = new StreamWriter("C:\\inetpub\\wwwroot\\mediana\\bin2\\log\\FiveDayForecast_log.txt");
            foreach (string s in log)
                sw.WriteLine(s);
            sw.Close();
        }
    }
}