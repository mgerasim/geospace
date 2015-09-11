using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceMediana.Common;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class MarksController : Controller
    {
        public void Pre_Index(List<int> values, List<int> valuesSkip, List<int> mediana, List<int> ionka, ref double[,] marks)
        {
            int sum1 = 0, sum2 = 0, sum3 = 0;
            double del1 = 0.0, del2 = 0.0, del3 = 0.0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] < 1000 && mediana[i] < 1000)
                {
                    marks[0, i] = values[i] - mediana[i];
                    sum1 += values[i] - mediana[i];
                }
                else
                {
                    marks[0, i] = 1000;
                    del1++;
                }
                if (i < ionka.Count && ionka[i] < 1000 && values[i] < 1000)
                {
                    marks[1, i] = values[i] - ionka[i];
                    sum2 += values[i] - ionka[i];
                }
                else
                {
                    marks[1, i] = 1000;
                    del2++;
                }

                if (i < ionka.Count && ionka[i] < 1000 && mediana[i] < 1000)
                {
                    marks[2, i] = mediana[i] - ionka[i];
                    sum3 += mediana[i] - ionka[i];
                }
                else
                {
                    marks[2, i] = 1000;
                    del3++;
                }
            }

            if (24 - del1 != 0) marks[0, 24] = Math.Round(sum1 / (24 - del1), 1);
            else marks[0, 24] = 1000;

            if (24 - del2 != 0) marks[1, 24] = Math.Round(sum2 / (24 - del2), 1);
            else marks[1, 24] = 1000;

            if (24 - del3 != 0) marks[2, 24] = Math.Round(sum3 / (24 - del3), 2);
            else marks[2, 24] = 1000;
        }

        public ActionResult Index(int stationCode = 43501, string type = "f0F2", int year = -1, int month = -1)
        {
            @ViewBag.Title = "Оценки";

            if (type == "M3000F2" || type == "M3000")
            {
                ViewBag.Type = "M3000";
                ViewBag.textY = "Коэффициент F2 ⋅ 10";
                ViewBag.max = "80";
            }
            if (type == "f0F2" || type == "f0")
            {
                ViewBag.Type = "f0";
                ViewBag.textY = "Критическая частота МГц ⋅ 10";
                ViewBag.max = "100";
            }

            ViewBag.step = "10";

            ViewBag.NameMenu = "Оценка " + ViewBag.Type;
            Station stat = Station.GetByCode(stationCode);            

            DateTime nowDateTime;

            if (year < 0 && month < 0) nowDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            else nowDateTime = new DateTime(year, month, 1);
            DateTime nextDate = nowDateTime.AddMonths(1);
            DateTime prevDate = nowDateTime.AddMonths(-1);

            if (nowDateTime.Month == DateTime.Now.Month) ViewBag.Header = stat.Name + " - " + stat.Code.ToString() + ". Оценки на " + nowDateTime.ToShortDateString();
            else ViewBag.Header = stat.Name + " - " + stat.Code.ToString() + ". Оценки за " + nowDateTime.ToString("MMM yyyy");

            int rangeNumber = 1;

            if (1 <= nowDateTime.Day && nowDateTime.Day <= 5) rangeNumber = 0;
            if (6 <= nowDateTime.Day && nowDateTime.Day <= 10) rangeNumber = 1;
            if (11 <= nowDateTime.Day && nowDateTime.Day <= 15) rangeNumber = 2;
            if (16 <= nowDateTime.Day && nowDateTime.Day <= 20) rangeNumber = 3;
            if (21 <= nowDateTime.Day && nowDateTime.Day <= 25) rangeNumber = 4;
            if (26 <= nowDateTime.Day && nowDateTime.Day <= 31) rangeNumber = 5;

            ViewBag.PrevDate = prevDate;
            ViewBag.NextDate = nextDate;

            ViewBag.Date = nowDateTime;
            ViewBag.Station = stat;
            ViewBag.Stations = Station.GetAll();

            const int N = 25;
            const int M = 3;
            const int P = 3;

            double[] F = new double[N-1];
            double[] delF = new double[N-1];

            double[,] marks_05 = new double[M, N];
            double[,] marks_07 = new double[M, N];
            double[,] marks_10 = new double[M, N];
            double[,] marks_20 = new double[M, N];
            double[,] marks_27 = new double[M, N];
            double[,] marks_30 = new double[M, N];

            double[,] del_05 = new double[M, N];
            double[,] del_07 = new double[M, N];
            double[,] del_10 = new double[M, N];
            double[,] del_20 = new double[M, N];
            double[,] del_27 = new double[M, N];
            double[,] del_30 = new double[M, N];

            double[, ,] Marks_05 = new double[P, M, N];
            double[, ,] Marks_07 = new double[P, M, N];
            double[, ,] Marks_10 = new double[P, M, N];
            double[, ,] Marks_20 = new double[P, M, N];
            double[, ,] Marks_27 = new double[P, M, N];
            double[, ,] Marks_30 = new double[P, M, N];

            try
            {
                List<CodeIonka> modelIonka = null;

                List<int> ionka = null;
                List<int> mediana = null;

                if (type == "f0F2" || type == "f0")
                {
                    DateTime dt;
                    int dayInMonth = DateTime.DaysInMonth(nowDateTime.Year, nowDateTime.Month);

                    for (int day = 0; day < dayInMonth; day++)
                    {
                        dt = nowDateTime.AddDays(day);

                        ionka = CodeIonka.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Where(x => x.MI == 0).Select(x => x.f0F2).ToList();
                        if( ionka == null ) break;
                        for( int h = 0; h < ionka.Count; h++ )
                        {
                            if (ionka[h] < 1000) F[h] += ionka[h];
                            else delF[h]++;
                        }
                    }

                    for (int h = 0; h < ionka.Count; h++)
                    {
                        if (24 - delF[h] > 0) F[h] = F[h] / delF[h];
                        else F[h] = 1000;
                    }

                    for (int day = 0; day < dayInMonth; day++)
                    {
                        dt = nowDateTime.AddDays(day);

                        modelIonka = CodeIonka.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day);
                        ionka = CodeIonka.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Where(x => x.MI == 0).Select(x => x.f0F2).ToList();
                        mediana = Mediana.GetByDate2(Station.GetByCode(stationCode), dt.Year, dt.Month, rangeNumber).Select(x => x.f0F2).ToList();

                        for (int i = 0; i < 24; i++)
                        {
                            if (ionka.Count == i)
                            {
                                ionka.Add(0);
                                continue;
                            }
                            else
                            {
                                if (i < modelIonka.Count)
                                {
                                    if (modelIonka[i].HH != i) ionka.Insert(i, 0);
                                }
                                else ionka.Insert(i, 0);
                            }
                        }

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_05).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_05_skip).ToList(),
                        mediana, ionka, ref marks_05);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_07).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_07_skip).ToList(),
                            mediana, ionka, ref marks_07);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_10).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_10_skip).ToList(),
                            mediana, ionka, ref marks_10);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_20).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_20_skip).ToList(),
                            mediana, ionka, ref marks_20);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_27).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_27_skip).ToList(),
                            mediana, ionka, ref marks_27);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_30).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.F2_30_skip).ToList(),
                            mediana, ionka, ref marks_30);

                        for( int i = 0; i < M; i++ )
                        {
                            for( int j = 0; j < N; j++ )
                            {
                                if (marks_05[i, j] < 1000)
                                {
                                    Marks_05[0, i, j] += marks_05[i, j];
                                    Marks_05[1, i, j] += Math.Abs( marks_05[i, j] );
                                }
                                else del_05[i, j]++;

                                if (marks_07[i, j] < 1000)
                                {
                                    Marks_07[0, i, j] += marks_07[i, j];
                                    Marks_07[1, i, j] += Math.Abs(marks_07[i, j]);
                                }
                                else del_07[i, j]++;

                                if (marks_10[i, j] < 1000)
                                {
                                    Marks_10[0, i, j] += marks_10[i, j];
                                    Marks_10[1, i, j] += Math.Abs(marks_10[i, j]);
                                }
                                else del_10[i, j]++;

                                if (marks_20[i, j] < 1000)
                                {
                                    Marks_20[0, i, j] += marks_20[i, j];
                                    Marks_20[1, i, j] += Math.Abs(marks_20[i, j]);
                                }
                                else del_20[i, j]++;

                                if (marks_27[i, j] < 1000)
                                {
                                    Marks_27[0, i, j] += marks_27[i, j];
                                    Marks_27[1, i, j] += Math.Abs(marks_27[i, j]);
                                }
                                else del_27[i, j]++;

                                if (marks_30[i, j] < 1000)
                                {
                                    Marks_30[0, i, j] += marks_30[i, j];
                                    Marks_30[1, i, j] += Math.Abs(marks_30[i, j]);
                                }
                                else del_30[i, j]++;
                            }
                        }
                    }

                    for (int i = 0; i < M; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (dayInMonth - del_05[i, j] != 0)
                            {
                                Marks_05[0, i, j] = Math.Round(Marks_05[0, i, j] / (dayInMonth - del_05[i, j]), 1);
                                Marks_05[1, i, j] = Math.Round(Marks_05[1, i, j] / (dayInMonth - del_05[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_05[2, i, j] = Math.Round(Marks_05[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_05[0, i, j] = 1000;
                                Marks_05[1, i, j] = 1000;
                                Marks_05[2, i, j] = 1000;
                            }

                            if (dayInMonth - del_07[i, j] != 0)
                            {
                                Marks_07[0, i, j] = Math.Round(Marks_07[0, i, j] / (dayInMonth - del_07[i, j]), 1);
                                Marks_07[1, i, j] = Math.Round(Marks_07[1, i, j] / (dayInMonth - del_07[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_07[2, i, j] = Math.Round(Marks_07[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_07[0, i, j] = 1000;
                                Marks_07[1, i, j] = 1000;
                                Marks_07[2, i, j] = 1000;
                            }
                            
                            if (dayInMonth - del_10[i, j] != 0)
                            {
                                Marks_10[0, i, j] = Math.Round(Marks_10[0, i, j] / (dayInMonth - del_10[i, j]), 1);
                                Marks_10[1, i, j] = Math.Round(Marks_10[1, i, j] / (dayInMonth - del_10[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_10[2, i, j] = Math.Round(Marks_10[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_10[0, i, j] = 1000;
                                Marks_10[1, i, j] = 1000;
                                Marks_10[2, i, j] = 1000;
                            }
                            
                            if (dayInMonth - del_20[i, j] != 0)
                            {
                                Marks_20[0, i, j] = Math.Round(Marks_20[0, i, j] / (dayInMonth - del_20[i, j]), 1);
                                Marks_20[1, i, j] = Math.Round(Marks_20[1, i, j] / (dayInMonth - del_20[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_20[2, i, j] = Math.Round(Marks_20[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_20[0, i, j] = 1000;
                                Marks_20[1, i, j] = 1000;
                                Marks_20[2, i, j] = 1000;
                            }
                            
                            if (dayInMonth - del_27[i, j] != 0)
                            {
                                Marks_27[0, i, j] = Math.Round(Marks_27[0, i, j] / (dayInMonth - del_27[i, j]), 1);
                                Marks_27[1, i, j] = Math.Round(Marks_27[1, i, j] / (dayInMonth - del_27[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_27[2, i, j] = Math.Round(Marks_27[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_27[0, i, j] = 1000;
                                Marks_27[1, i, j] = 1000;
                                Marks_27[2, i, j] = 1000;
                            }
                            
                            if (dayInMonth - del_30[i, j] != 0)
                            {
                                Marks_30[0, i, j] = Math.Round(Marks_30[0, i, j] / (dayInMonth - del_30[i, j]), 1);
                                Marks_30[1, i, j] = Math.Round(Marks_30[1, i, j] / (dayInMonth - del_30[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_30[2, i, j] = Math.Round(Marks_30[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_30[0, i, j] = 1000;
                                Marks_30[1, i, j] = 1000;
                                Marks_30[2, i, j] = 1000;
                            }
                        }
                    }
                }

                if (type == "M3000F2" || type == "M3000")
                {
                    DateTime dt;
                    int dayInMonth = DateTime.DaysInMonth(nowDateTime.Year, nowDateTime.Month);

                    for (int day = 0; day < dayInMonth; day++)
                    {
                        dt = nowDateTime.AddDays(day);

                        ionka = CodeIonka.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Where(x => x.MI == 0).Select(x => x.M3000F2).ToList();
                        if (ionka == null) break;
                        for (int h = 0; h < ionka.Count; h++)
                        {
                            if (ionka[h] < 1000) F[h] += ionka[h];
                            else delF[h]++;
                        }
                    }

                    for (int h = 0; h < ionka.Count; h++)
                    {
                        if (24 - delF[h] > 0) F[h] = F[h] / delF[h];
                        else F[h] = 1000;
                    }

                    for (int day = 0; day < dayInMonth; day++)
                    {
                        dt = nowDateTime.AddDays(day);

                        modelIonka = CodeIonka.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day);
                        ionka = CodeIonka.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Where(x => x.MI == 0).Select(x => x.M3000F2).ToList();
                        mediana = Mediana.GetByDate2(Station.GetByCode(stationCode), dt.Year, dt.Month, rangeNumber).Select(x => x.M3000F2).ToList();

                        for (int i = 0; i < 24; i++)
                        {
                            if (ionka.Count == i)
                            {
                                ionka.Add(0);
                                continue;
                            }
                            else
                            {
                                if (i < modelIonka.Count)
                                {
                                    if (modelIonka[i].HH != i) ionka.Insert(i, 0);
                                }
                                else ionka.Insert(i, 0);
                            }
                        }

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_05).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_05_skip).ToList(),
                        mediana, ionka, ref marks_05);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_07).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_07_skip).ToList(),
                            mediana, ionka, ref marks_07);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_10).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_10_skip).ToList(),
                            mediana, ionka, ref marks_10);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_20).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_20_skip).ToList(),
                            mediana, ionka, ref marks_20);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_27).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_27_skip).ToList(),
                            mediana, ionka, ref marks_27);

                        Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_30).ToList(),
                            Average.GetByDate(Station.GetByCode(stationCode), dt.Year, dt.Month, dt.Day).Select(x => x.M3000_30_skip).ToList(),
                            mediana, ionka, ref marks_30);

                        for (int i = 0; i < M; i++)
                        {
                            for (int j = 0; j < N; j++)
                            {
                                if (marks_05[i, j] < 1000)
                                {
                                    Marks_05[0, i, j] += marks_05[i, j];
                                    Marks_05[1, i, j] += Math.Abs(marks_05[i, j]);
                                }
                                else del_05[i, j]++;

                                if (marks_07[i, j] < 1000)
                                {
                                    Marks_07[0, i, j] += marks_07[i, j];
                                    Marks_07[1, i, j] += Math.Abs(marks_07[i, j]);
                                }
                                else del_07[i, j]++;

                                if (marks_10[i, j] < 1000)
                                {
                                    Marks_10[0, i, j] += marks_10[i, j];
                                    Marks_10[1, i, j] += Math.Abs(marks_10[i, j]);
                                }
                                else del_10[i, j]++;

                                if (marks_20[i, j] < 1000)
                                {
                                    Marks_20[0, i, j] += marks_20[i, j];
                                    Marks_20[1, i, j] += Math.Abs(marks_20[i, j]);
                                }
                                else del_20[i, j]++;

                                if (marks_27[i, j] < 1000)
                                {
                                    Marks_27[0, i, j] += marks_27[i, j];
                                    Marks_27[1, i, j] += Math.Abs(marks_27[i, j]);
                                }
                                else del_27[i, j]++;

                                if (marks_30[i, j] < 1000)
                                {
                                    Marks_30[0, i, j] += marks_30[i, j];
                                    Marks_30[1, i, j] += Math.Abs(marks_30[i, j]);
                                }
                                else del_30[i, j]++;
                            }
                        }
                    }

                    for (int i = 0; i < M; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (dayInMonth - del_05[i, j] != 0)
                            {
                                Marks_05[0, i, j] = Math.Round(Marks_05[0, i, j] / (dayInMonth - del_05[i, j]), 1);
                                Marks_05[1, i, j] = Math.Round(Marks_05[1, i, j] / (dayInMonth - del_05[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_05[2, i, j] = Math.Round(Marks_05[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_05[0, i, j] = 1000;
                                Marks_05[1, i, j] = 1000;
                                Marks_05[2, i, j] = 1000;
                            }

                            if (dayInMonth - del_07[i, j] != 0)
                            {
                                Marks_07[0, i, j] = Math.Round(Marks_07[0, i, j] / (dayInMonth - del_07[i, j]), 1);
                                Marks_07[1, i, j] = Math.Round(Marks_07[1, i, j] / (dayInMonth - del_07[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_07[2, i, j] = Math.Round(Marks_07[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_07[0, i, j] = 1000;
                                Marks_07[1, i, j] = 1000;
                                Marks_07[2, i, j] = 1000;
                            }

                            if (dayInMonth - del_10[i, j] != 0)
                            {
                                Marks_10[0, i, j] = Math.Round(Marks_10[0, i, j] / (dayInMonth - del_10[i, j]), 1);
                                Marks_10[1, i, j] = Math.Round(Marks_10[1, i, j] / (dayInMonth - del_10[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_10[2, i, j] = Math.Round(Marks_10[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_10[0, i, j] = 1000;
                                Marks_10[1, i, j] = 1000;
                                Marks_10[2, i, j] = 1000;
                            }

                            if (dayInMonth - del_20[i, j] != 0)
                            {
                                Marks_20[0, i, j] = Math.Round(Marks_20[0, i, j] / (dayInMonth - del_20[i, j]), 1);
                                Marks_20[1, i, j] = Math.Round(Marks_20[1, i, j] / (dayInMonth - del_20[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_20[2, i, j] = Math.Round(Marks_20[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_20[0, i, j] = 1000;
                                Marks_20[1, i, j] = 1000;
                                Marks_20[2, i, j] = 1000;
                            }

                            if (dayInMonth - del_27[i, j] != 0)
                            {
                                Marks_27[0, i, j] = Math.Round(Marks_27[0, i, j] / (dayInMonth - del_27[i, j]), 1);
                                Marks_27[1, i, j] = Math.Round(Marks_27[1, i, j] / (dayInMonth - del_27[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_27[2, i, j] = Math.Round(Marks_27[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_27[0, i, j] = 1000;
                                Marks_27[1, i, j] = 1000;
                                Marks_27[2, i, j] = 1000;
                            }

                            if (dayInMonth - del_30[i, j] != 0)
                            {
                                Marks_30[0, i, j] = Math.Round(Marks_30[0, i, j] / (dayInMonth - del_30[i, j]), 1);
                                Marks_30[1, i, j] = Math.Round(Marks_30[1, i, j] / (dayInMonth - del_30[i, j]), 1);
                                if (i < 24 && F[i] != 1000) Marks_30[2, i, j] = Math.Round(Marks_30[1, i, j] / F[i], 2);
                            }
                            else
                            {
                                Marks_30[0, i, j] = 1000;
                                Marks_30[1, i, j] = 1000;
                                Marks_30[2, i, j] = 1000;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content("Ошибка построения");
            }

            ViewBag.marks_05 = Marks_05;
            ViewBag.marks_07 = Marks_07;
            ViewBag.marks_10 = Marks_10;
            ViewBag.marks_20 = Marks_20;
            ViewBag.marks_27 = Marks_27;
            ViewBag.marks_30 = Marks_30;

            return View();
        }
    }
}
