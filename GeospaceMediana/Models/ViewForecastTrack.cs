using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;

namespace GeospaceMediana.Models
{
    public class ViewForecastTrack
    {
        public List<string> header = new List<string>();
        public List<double[,]> Table1 = new List<double[,]>();
        public List<double[,]> Table2 = new List<double[,]>();
        public List<double[,]> Colors1 = new List<double[,]>();
        public List<double[,]> Colors2 = new List<double[,]>();
        public int quantity;
        public List<List<string>> grafValues = new List<List<string>>();

        public ViewForecastTrack( DateTime now )
        {
            List<Station> allStatition = Station.GetAll();
            quantity = allStatition.Count;

            foreach (Station item in allStatition)
            {
                double[,] table1 = new double[5, 24];
                double[,] table2 = new double[5, 24];

                double[,] colors1 = new double[5, 24];
                double[,] colors2 = new double[5, 24];

                List<CodeIonka> ionka = CodeIonka.GetByDate(item, now.Year, now.Month, now.Day);

                int h = 0; 
                for (int i = 0; i < ionka.Count; i++ )
                {
                    double val0 = 0, val1 = 0, val2 = 0, val3 = 0;

                    h = ionka[i].HH;
                    if (h > 23) continue;

                    if (ionka[i].f0F2 < 1000 && ionka[i].f0F2 > 0)
                    {
                        table1[0, h] = Math.Round( ionka[i].f0F2 / 10.0 + item.addition, 1 );
                        val0 = table1[0, h];
                    }
                    else table1[0, h] = ionka[i].f0F2;

                    if (ionka[i].f0F1 < 1000 && ionka[i].f0F1 > 0)
                    {
                        table1[1, h] = Math.Round(ionka[i].f0F1 / 10.0 + item.addition, 1);
                        val1 = table1[1, h];
                    }
                    else table1[1, h] = ionka[i].f0F1;

                    if (ionka[i].f0E < 1000 && ionka[i].f0E > 0)
                    {
                        table1[2, h] =  Math.Round( ionka[i].f0E / 10.0 + item.addition, 1);
                        val2 = table1[2, h];
                    }
                    else table1[2, h] = ionka[i].f0E;

                    if (ionka[i].f0Es < 1000 && ionka[i].f0Es > 0)
                    {
                        table1[3, h] =  Math.Round( ionka[i].f0Es / 10.0 + item.addition, 1);
                        val3 = table1[3, h];
                    }
                    else table1[3, h] = ionka[i].f0Es;

                    table1[4, h] = Math.Max(val0, val1);
                    table1[4, h] = Math.Max(table1[4, h], val2);
                    table1[4, h] = Math.Max(table1[4, h], val3);

                    val0 = 0;
                    val1 = 0;
                    val2 = 0;                                        
                }

                Check(table1, colors1);

                string graf1 = "[", graf2 = "[", graf3 = "[";
                List<string> oneGraf = new List<string>();

                Recovery_Data_Table1(item, table1, colors1, now);

                for (int i = 0; i < ionka.Count; i++)
                {
                    double val0 = 0, val1 = 0, val2 = 0;

                    h = ionka[i].HH;
                    if (h > 23) continue;

                    if (ionka[i].f0F2 > 0 && ionka[i].f0F2 < 1000
                        && ionka[i].M3000F2 > 0 && ionka[i].M3000F2 < 1000
                        && table1[0, h] > 0 && table1[0, h] < 1000
                        && ionka[i].f0F2 > 0 && ionka[i].f0F2 < 1000
                        && ionka[i].M3000F2 > 0 && ionka[i].M3000F2 < 1000)
                    {
                        table2[0, h] = Math.Round( ((17.8 * ( ((ionka[i].f0F2 * ionka[i].M3000F2) / 100.0) - table1[0, h]) ) / 14.75 ) + table1[0, h], 1);
                        val0 = table2[0, h];
                    }
                    else table2[0, h] = 1000;

                    if (ionka[i].f0F1 > 0 && ionka[i].f0F1 < 1000 && ionka[i].M3000F1 > 0 && ionka[i].M3000F1 < 1000 && ionka[i].f0F1 > 0 && ionka[i].f0F1 < 1000)
                    {
                        table2[1, h] = Math.Round(ionka[i].f0F1 * ionka[i].M3000F1 / 100.0, 1);
                        val1 = table2[1, h];
                    }
                    else table2[1, h] = 1000;

                    if (ionka[i].f0E < 1000 && ionka[i].f0E > 0)
                    {
                        table2[2, h] = Math.Round(ionka[i].f0E * 5.27 / 10.0, 1);
                        val2 = table2[2, h];
                    }
                    else table2[2, h] = 1000;

                    table2[3, h] = Math.Max(val0, val1);
                    table2[3, h] = Math.Max(table2[3, h], val2);
                }

                Check(table2, colors2);
                Recovery_Data_Table2(item, table2, table1, colors2, now);

                for (h = 0; h < 24; h++)
                {
                    if (table1[4, h] > 0 && table1[4, h] < 1000) graf1 += table1[4, h].ToString().Replace( ",", "." ) + ",";
                    else graf1 += "null,";

                    if (table2[3, h] > 0 && table2[3, h] < 1000) graf2 += table2[3, h].ToString().Replace(",", ".") + ",";
                    else graf2 += "null,";

                    if (h < ionka.Count)
                    {
                        if (ionka[h].fmin < 1000) graf3 += (ionka[h].fmin / 10.0).ToString().Replace(",", ".") + ",";
                        else graf3 += "null,";
                    }
                    else graf3 += "null,";
                }
                graf1 += "]"; graf2 += "]"; graf3 += "]";

                oneGraf.Add(graf1);
                oneGraf.Add(graf2);
                oneGraf.Add(graf3);

                grafValues.Add(oneGraf);

                header.Add(item.Code.ToString() + " - " + item.Name);

                Table1.Add(table1);
                Table2.Add(table2);

                Colors1.Add(colors1);
                Colors2.Add(colors2);
            }
        }

        public void Check( double[,] values, double[,] colors )
        {
            for( int i = 0; i < 5; i++)
            {
                for( int j = 0; j < 24; j++)
                {
                    if (values[i, j] <= 0 || values[i, j] > 30)
                    {
                        values[i, j] = 1100;
                        colors[i, j] = 0;
                    }
                }
            }
        }

        public void Recovery_Data_Table1(Station stat, double[,] values, double[,] colors, DateTime now)
        {
            for (int i = 0; i < 24; i++)
            {
                if (values[0, i] >= 1000)
                    interpolation(values, i, colors);
            }
            Check(values, colors);

            bool flag = false;
            int sum = 0, maxSum = 0, del = 0, del10 = 0;
            double avg = 0.0, avg10 = 0.0;

            List<int> int_f0F2 = Average.GetByDate(stat, now.Year, now.Month, now.Day).Select(x => x.F2_10).ToList();

            for( int i = 0; i < 24; i++ )
            {
                if (values[0, i] >= 1000)
                {
                    flag = true;
                    del++;
                }
                else
                {
                    flag = false;
                    avg += values[0, i];
                }

                if (flag) sum++;
                else
                {
                    maxSum = Math.Max(maxSum, sum);
                    sum = 0;
                }


                if (int_f0F2[i] < 1000 && i < int_f0F2.Count) avg10 += int_f0F2[i];
                else del10++;
            }

            if (24 - del > 0) avg = avg / (24 - del);
            if (24 - del10 > 0) avg10 = avg10 / ((24 - del10)*10);

            for (int i = 0; i < 24; i++)
            {
                if (values[0, i] >= 1100)
                {
                    if (maxSum <= 6 && i < int_f0F2.Count)
                    {
                        if (values[0, i] >= 1000 && int_f0F2[i] < 1000)
                        {
                            values[0, i] = Math.Round((avg + ((int_f0F2[i] / 10.0) - avg10)) * 1, 1);
                            colors[0, i] = 1;
                        }
                    }
                    else
                    {
                        if (i < int_f0F2.Count)
                        {
                            if (int_f0F2[i] < 1000)
                            {
                                values[0, i] = int_f0F2[i] / 10.0 + stat.addition;
                                colors[0, i] = 1;
                            }
                        }
                    }                    
                }                
            }

            Check(values, colors);
            

            for (int i = 0; i < 24; i++)
            {
                double val0 = 0.0, val1 = 0.0, val2 = 0.0, val3 = 0.0;
                if (values[0, i] > 0 && values[0, i] < 1000) val0 = values[0, i];
                if (values[1, i] > 0 && values[1, i] < 1000) val1 = values[1, i];
                if (values[2, i] > 0 && values[2, i] < 1000) val2 = values[2, i];
                if (values[3, i] > 0 && values[3, i] < 1000) val3 = values[3, i];

                values[4, i] = Math.Max(val0, val1);
                values[4, i] = Math.Max(values[4, i], val2);
                values[4, i] = Math.Max(values[4, i], val3);  
            }
            Check(values, colors);
        }

        public void Recovery_Data_Table2(Station stat, double[,] table2, double[,] table1, double[,] colors, DateTime now)
        {
            bool flag = false;
            int sum = 0, maxSum = 0, del = 0, del10 = 0;
            double avg = 0.0, avg10 = 0.0;

            List<int> int_M3000 = Average.GetByDate(stat, now.Year, now.Month, now.Day).Select(x => x.M3000_10).ToList();
            List<int> int_M3000_Now = CodeIonka.GetByDate(stat, now.Year, now.Month, now.Day).Select(x => x.M3000F2).ToList();
            List<CodeIonka> ionkaProve = CodeIonka.GetByDate(stat, now.Year, now.Month, now.Day);

            for (int i = 0; i < 24; i++)
            {

                if (i < int_M3000_Now.Count)
                {
                    if (int_M3000_Now[i] >= 1000)
                    {
                        flag = true;
                        del++;
                    }
                    else
                    {
                        flag = false;
                        avg += int_M3000_Now[i];
                    }

                    if (flag) sum++;
                    else
                    {
                        maxSum = Math.Max(maxSum, sum);
                        sum = 0;
                    }
                }
                else del++;

                if (i < int_M3000.Count)
                {
                    if (int_M3000[i] < 1000) avg10 += int_M3000[i];
                    else del10++;
                }
                else del10++;
            }

            if (24 - del > 0) avg = avg / ((24 - del) * 10);
            if (24 - del10 > 0) avg10 = avg10 / ((24 - del10) * 10);

            int h = 0;
            for (int i = 0; i < ionkaProve.Count; i++)
            {
                h = ionkaProve[i].HH;
                if (h > 23) continue;

                if (table2[0, h] >= 1000 && h < int_M3000.Count && table1[0, h] < 1000)
                {
                    if (maxSum <= 6)
                    {
                        if (int_M3000[h] < 1000)
                        {
                            double w = avg + ((int_M3000[h] / 10.0) - avg10);
                            table2[0, h] = Math.Round(((17.8 * (((table1[0, h] - stat.addition) * (avg + ((int_M3000[h] / 10.0) - avg10))) - table1[0, h])) / 14.75) + table1[0, h], 1) * 1;
                            colors[0, h] = 1;
                        }
                    }
                    else
                    {
                        if (int_M3000[h] < 1000)
                        {
                            table2[0, h] = Math.Round(((17.8 * (((table1[0, h] - stat.addition) * (int_M3000[h] / 10.0)) - table1[0, h])) / 14.75) + table1[0, h], 1) * 1; ;
                            colors[0, h] = 1;
                        }
                    }
                    
                }             
            }

            Check(table2, colors); 
            for (int i = 0; i < 24; i++)
            {
                if (table2[0, i] >= 1000)
                    interpolation(table2, i, colors);
            }
            Check(table2, colors);

            for (int i = 0; i < 24; i++)
            {
                double val0 = 0.0, val1 = 0.0, val2 = 0.0, val3 = 0.0;
                if (table2[0, i] > 0 && table2[0, i] < 1000) val0 = table2[0, i];
                if (table2[1, i] > 0 && table2[1, i] < 1000) val1 = table2[1, i];
                if (table2[2, i] > 0 && table2[2, i] < 1000) val2 = table2[2, i];

                table2[3, i] = Math.Max(val0, val1);
                table2[3, i] = Math.Max(table2[3, i], val2);
                table2[3, i] = Math.Max(table2[3, i], val3);  
            }
            Check(table2, colors);
        }

        public void interpolation( double[,] values, int hour, double[,] colors )
        {
            if( hour == 0 )
            {
                if (values[0, hour + 1] < 1000 && values[0, hour + 2] < 1000)
                {
                    values[0, hour] = 2 * values[0, hour + 1] - values[0, hour + 2];
                    colors[0, hour] = 1;
                }
            }

            if( hour > 0 && hour < 23)
            {
                if (values[0, hour - 1] < 1000 && values[0, hour + 1] < 1000)
                {
                    values[0, hour] = Math.Round(values[0, hour - 1] + (values[0, hour + 1] - values[0, hour - 1]) / 2, 1);
                    colors[0, hour] = 1;
                }
            }

            if (hour == 23)
            {
                if (values[0, hour - 1] < 1000 && values[0, hour - 2] < 1000)
                {
                    values[0, hour] = 2 * values[0, hour - 1] - values[0, hour - 2];
                    colors[0, hour] = 1;
                }
            }            
        }
    }
}