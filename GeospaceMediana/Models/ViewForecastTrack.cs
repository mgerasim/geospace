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
                    if( h == ionka[i].HH )
                    {
                        if (ionka[i].f0F2 < 1000 && ionka[i].f0F2 > 0)
                        {
                            table1[0, i] = ionka[i].f0F2 / 10 + item.addition;
                            val0 = table1[0, i];
                        }
                        else table1[0, i] = ionka[i].f0F2;

                        if (ionka[i].f0F1 < 1000 && ionka[i].f0F1 > 0)
                        {
                            table1[1, i] = ionka[i].f0F1 / 10 + item.addition;
                            val1 = table1[1, i];
                        }
                        else table1[1, i] = ionka[i].f0F1;

                        if (ionka[i].f0E < 1000 && ionka[i].f0E > 0)
                        {
                            table1[2, i] = ionka[i].f0E / 10 + item.addition;
                            val2 = table1[2, i];
                        }
                        else table1[2, i] = ionka[i].f0E;

                        if (ionka[i].f0Es < 1000 && ionka[i].f0Es > 0)
                        {
                            table1[3, i] = ionka[i].f0Es / 10 + item.addition;
                            val3 = table1[3, i];
                        }
                        else table1[3, i] = ionka[i].f0Es;

                        table1[4, i] = Math.Max(val0, val1);
                        table1[4, i] = Math.Max(table1[4, i], val2);
                        table1[4, i] = Math.Max(table1[4, i], val3);

                        val0 = 0;
                        val1 = 0;
                        val2 = 0;
                        if (ionka[i].f0F2 < 1000 && ionka[i].M3000F2 < 1000 && table1[0, i] < 1000 && ionka[i].f0F2 > 0 && ionka[i].M3000F2 > 0 && table1[0, i] > 0)
                        {
                            table2[0, i] = Math.Round( (17.8 * (((ionka[i].f0F2 * ionka[i].M3000F2) / 100) - table1[0, i]) / 14.75) + table1[0, i], 1 );
                            val0 = table2[0, i];
                        }
                        else table2[0, i] = 1000;

                        if (ionka[i].f0F1 < 1000 && ionka[i].M3000F1 > 0 && ionka[i].f0F1 > 0 && ionka[i].M3000F1 > 0)
                        {
                            table2[1, i] = Math.Round( ionka[i].f0F1 * ionka[i].M3000F1 / 100.0, 1 );
                            val1 = table2[1, i];
                        }
                        else table2[1, i] = 1000;

                        if (ionka[i].f0E < 1000 && ionka[i].f0E > 0)
                        {
                            table2[2, i] = Math.Round(ionka[i].f0E * 5.27 / 10.0, 1);
                            val2 = table2[2, i];
                        }
                        else table2[2, i] = 1000;

                        table2[3, i] = Math.Max(val0, val1);
                        table2[3, i] = Math.Max(table2[3, i], val2);
                        
                        h++;
                    }
                }

                header.Add(item.Code.ToString() + " - " + item.Name);

                Table1.Add(table1);
                Table2.Add(table2);
            }
        }
    }
}