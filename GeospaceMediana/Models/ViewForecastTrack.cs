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

                List<CodeIonka> ionka = CodeIonka.GetByDate(item, now.Year, now.Month, now.Day);

                int h = 0; 
                for (int i = 0; i < ionka.Count; i++ )
                {
                    double val0 = 0, val1 = 0, val2 = 0, val3 = 0;

                    h = ionka[i].HH;
                    if (h > 23) continue;

                    if (ionka[i].f0F2 < 1000 && ionka[i].f0F2 > 0)
                    {
                        table1[0, h] = ionka[i].f0F2 / 10.0 + item.addition;
                        val0 = table1[0, h];
                    }
                    else table1[0, h] = ionka[i].f0F2;

                    if (ionka[i].f0F1 < 1000 && ionka[i].f0F1 > 0)
                    {
                        table1[1, h] = ionka[i].f0F1 / 10.0 + item.addition;
                        val1 = table1[1, h];
                    }
                    else table1[1, h] = ionka[i].f0F1;

                    if (ionka[i].f0E < 1000 && ionka[i].f0E > 0)
                    {
                        table1[2, h] = ionka[i].f0E / 10.0 + item.addition;
                        val2 = table1[2, h];
                    }
                    else table1[2, h] = ionka[i].f0E;

                    if (ionka[i].f0Es < 1000 && ionka[i].f0Es > 0)
                    {
                        table1[3, h] = ionka[i].f0Es / 10.0 + item.addition;
                        val3 = table1[3, h];
                    }
                    else table1[3, h] = ionka[i].f0Es;

                    table1[4, h] = Math.Max(val0, val1);
                    table1[4, h] = Math.Max(table1[4, h], val2);
                    table1[4, h] = Math.Max(table1[4, h], val3);

                    val0 = 0;
                    val1 = 0;
                    val2 = 0;
                    if (ionka[i].f0F2 > 0 && ionka[i].f0F2 < 1000
                        && ionka[i].M3000F2 > 0 && ionka[i].M3000F2 < 1000
                        && table1[0, h] > 0 && table1[0, h] < 1000
                        && ionka[i].f0F2 > 0 && ionka[i].f0F2 < 1000
                        && ionka[i].M3000F2 > 0 && ionka[i].M3000F2 < 1000)
                    {
                        table2[0, h] = Math.Round( (17.8 * (((ionka[i].f0F2 * ionka[i].M3000F2) / 100) - table1[0, h]) / 14.75) + table1[0, h], 1 );
                        val0 = table2[0, h];
                    }
                    else table2[0, h] = 1000;

                    if (ionka[i].f0F1 > 0 && ionka[i].f0F1 < 1000 && ionka[i].M3000F1 > 0 && ionka[i].M3000F1 < 1000 && ionka[i].f0F1 > 0 && ionka[i].f0F1 < 1000)
                    {
                        table2[1, h] = Math.Round( ionka[i].f0F1 * ionka[i].M3000F1 / 100.0, 1 );
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

                for (h = 0; h < 24; h++ )
                {
                    if (table1[0, h] <= 0 || table1[0, h] > 30) table1[0, h] = 1100;
                    if (table1[1, h] <= 0 || table1[1, h] > 30) table1[1, h] = 1100;
                    if (table1[2, h] <= 0 || table1[2, h] > 30) table1[2, h] = 1100;
                    if (table1[3, h] <= 0 || table1[3, h] > 30) table1[3, h] = 1100;
                    if (table1[4, h] <= 0 || table1[4, h] > 30) table1[4, h] = 1100;

                    if (table2[0, h] <= 0 || table2[0, h] > 30) table2[0, h] = 1100;
                    if (table2[1, h] <= 0 || table2[1, h] > 30) table2[1, h] = 1100;
                    if (table2[2, h] <= 0 || table2[2, h] > 30) table2[2, h] = 1100;
                    if (table2[3, h] <= 0 || table2[3, h] > 30) table2[3, h] = 1100;                    
                }

                string graf1 = "[", graf2 = "[", graf3 = "[";
                List<string> oneGraf = new List<string>();

                for (h = 0; h < 24; h++)
                {
                    //if (table1[4, h] >= 1000) interpolation(table1, h, 4);
                    //if (table2[3, h] >= 1000) interpolation(table2, h, 3);

                    if (table1[4, h] < 1000) graf1 += table1[4, h].ToString().Replace( ",", "." ) + ",";
                    else graf1 += "null,";

                    if (table2[3, h] < 1000) graf2 += table2[3, h].ToString().Replace(",", ".") + ",";
                    else graf2 += "null,";

                    if (h < ionka.Count)
                    {
                        if (ionka[h].fmin < 1000) graf3 += ionka[h].fmin.ToString().Replace(",", ".") + ",";
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
            }
        }
        public void interpolation( double[,] values, int hour, int index )
        {
            if( hour == 0 )
            {
                if (values[index, hour + 1] < 1000 && values[index, hour + 2] < 1000)
                    values[index, hour] = 2 * values[index, hour + 1] - values[index, hour + 2];
            }

            if (hour == 1)
            {
                if (values[index, hour - 1] < 1000 && values[index, hour + 1] < 1000)
                    values[index, hour] = Math.Round( values[index, hour - 1] + (values[index, hour + 1] - values[index, hour - 1]) / 2, 1);
                else
                {
                    if (values[index, hour + 1] < 1000 && values[index, hour + 2] < 1000)
                        values[index, hour] = 2 * values[index, hour + 1] - values[index, hour + 2];
                }
            }

            if( hour > 1 && hour < 22)
            {
                if (values[index, hour - 1] < 1000 && values[index, hour + 1] < 1000)
                    values[index, hour] = Math.Round( values[index, hour - 1] + (values[index, hour + 1] - values[index, hour - 1]) / 2, 1 );
                else
                {
                    if (values[index, hour + 1] < 1000 && values[index, hour + 2] < 1000)
                        values[index, hour] = 2 * values[index, hour + 1] - values[index, hour + 2];
                    else
                    {
                        if (values[index, hour - 1] < 1000 && values[index, hour - 2] < 1000)
                            values[index, hour] = 2 * values[index, hour - 1] - values[index, hour - 2];
                    }
                }
            }

            if ( hour == 22 )
            {
                if (values[index, hour - 1] < 1000 && values[index, hour + 1] < 1000)
                    values[index, hour] = Math.Round( values[index, hour - 1] + (values[index, hour + 1] - values[index, hour - 1]) / 2, 1 );
                else
                {
                    if (values[index, hour - 1] < 1000 && values[index, hour - 2] < 1000)
                        values[index, hour] = 2 * values[index, hour - 1] - values[index, hour - 2];
                }
            }

            if (hour == 23)
            {
                if (values[index, hour - 1] < 1000 && values[index, hour - 2] < 1000)
                    values[index, hour] = 2 * values[index, hour - 1] - values[index, hour - 2];
            }            
        }
    }
}