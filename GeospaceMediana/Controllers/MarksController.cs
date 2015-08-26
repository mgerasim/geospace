﻿using GeospaceEntity.Models;
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
    public class AverageController : Controller
    {
        //
        // GET: /Average/

        public void Pre_Index( List<int> values, List<int> valuesSkip, List<int> mediana, List<int> ionka, ref string strValues, ref string strValuesSkip, ref int[,] marks)
        {
            int sum = 0, del = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] < 1000)
                {
                    strValues += (values[i].ToString()).Replace(",", ".") + ",";
                    sum += values[i];
                }
                else
                {
                    strValues += "0,";
                    del++;
                }
                strValuesSkip += valuesSkip[i].ToString() + ",";
                marks[0, i] = values[i] - mediana[i];
                if (i < ionka.Count && ionka[i] < 1000)
                {
                    marks[1, i] = values[i] - ionka[i];
                    marks[2, i] = mediana[i] - ionka[i];
                }
                else
                {
                    marks[1, i] = 1000;
                    marks[2, i] = 1000;
                }
            }
            if (24 - del != 0) marks[0, 24] = (int)Math.Round(1.0 * sum / (24 - del), 0);
            else marks[0, 24] = 1000;

        }

        public ActionResult Index(int stationCode = 43501, string type = "f0F2", int year=-1, int month=-1, int day=-1)
        {
            @ViewBag.Title = "Оценки";

            if (type == "M3000F2" || type == "M3000")
            {
                ViewBag.Type = "M3000";
                ViewBag.textY = "Коэффициент M3000 ⋅ 10";
                ViewBag.max = "60";
            }
            if (type == "f0F2" || type == "f0")
            {
                ViewBag.Type = "f0";
                ViewBag.textY = "Критическая частота МГц ⋅ 10";
                ViewBag.max = "100";
            }

            ViewBag.step = "10";

            ViewBag.NameMenu = "Оценки " + ViewBag.Type;

            DateTime nowDateTime;

            if (year < 0 && month < 0 && day < 0)
            {
                nowDateTime = DateTime.Now.AddDays(-1);
            }
            else nowDateTime = new DateTime(year, month, day);
            DateTime nextDate = nowDateTime.AddDays(1);
            DateTime prevDate = nowDateTime.AddDays(-1);

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
            ViewBag.Station = Station.GetByCode(stationCode);
            ViewBag.Stations = Station.GetAll();

            string value = "[";
            string medianaValues = "[";
            string value_05 = "[";
            string value_05_skip = "[";
            string value_07 = "[";
            string value_07_skip = "[";
            string value_10 = "[";
            string value_10_skip = "[";
            string value_20 = "[";
            string value_20_skip = "[";
            string value_27 = "[";
            string value_27_skip = "[";
            string value_30 = "[";
            string value_30_skip = "[";

            const int N = 25;
            const int M = 3;

            int[,] marks_05 = new int[M, N];
            int[,] marks_07 = new int[M, N];
            int[,] marks_10 = new int[M, N];
            int[,] marks_20 = new int[M, N];
            int[,] marks_27 = new int[M, N];
            int[,] marks_30 = new int[M, N];

            try
            {
                if (type == "f0F2" || type == "f0")
                {
                    List<int> ionka = CodeIonka.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Where(x => x.MI == 0).Select(x => x.f0F2).ToList();
                    List<int> mediana = Mediana.GetByDate2(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, rangeNumber).Select(x => x.f0F2).ToList();

                    for (int i = 0; i < ionka.Count; i++)
                    {
                        if (ionka[i] < 1000) value += ionka[i].ToString() + ",";
                        else value += "0,";
                    }

                    if (mediana != null)
                    {
                        for (int i = 0; i < mediana.Count; i++)
                        {
                            medianaValues += mediana[i].ToString() + ",";
                        }
                    }

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_05).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_05_skip).ToList(),
                        mediana, ionka, ref value_05, ref value_05_skip, ref marks_05);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_07).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_07_skip).ToList(),
                        mediana, ionka, ref value_07, ref value_07_skip, ref marks_07);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_10).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_10_skip).ToList(),
                        mediana, ionka, ref value_10, ref value_10_skip, ref marks_10);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_20).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_20_skip).ToList(),
                        mediana, ionka, ref value_20, ref value_20_skip, ref marks_20);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_27).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_27_skip).ToList(),
                        mediana, ionka, ref value_27, ref value_27_skip, ref marks_27);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_30).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.F2_30_skip).ToList(),
                        mediana, ionka, ref value_30, ref value_30_skip, ref marks_30);

                }

                if (type == "M3000F2" || type == "M3000")
                {
                    List<int> ionka = CodeIonka.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Where(x => x.MI == 0).Select(x => x.M3000F2).ToList();
                    List<int> mediana = Mediana.GetByDate2(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, rangeNumber).Select(x => x.M3000F2).ToList();

                    for (int i = 0; i < ionka.Count; i++)
                    {
                        if (ionka[i] < 1000) value += ionka[i].ToString() + ",";
                        else value += "0,";
                    }

                    if (mediana != null)
                    {
                        for (int i = 0; i < mediana.Count; i++)
                        {
                            medianaValues += mediana.ToString() + ",";
                        }
                    }

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_05).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_05_skip).ToList(),
                        mediana, ionka, ref value_05, ref value_05_skip, ref marks_05);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_07).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_07_skip).ToList(),
                        mediana, ionka, ref value_07, ref value_07_skip, ref marks_07);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_10).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_10_skip).ToList(),
                        mediana, ionka, ref value_10, ref value_10_skip, ref marks_10);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_20).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_20_skip).ToList(),
                        mediana, ionka, ref value_20, ref value_20_skip, ref marks_20);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_27).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_27_skip).ToList(),
                        mediana, ionka, ref value_27, ref value_27_skip, ref marks_27);

                    Pre_Index(Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_30).ToList(),
                        Average.GetByDate(Station.GetByCode(stationCode), nowDateTime.Year, nowDateTime.Month, nowDateTime.Day).Select(x => x.M3000_30_skip).ToList(),
                        mediana, ionka, ref value_30, ref value_30_skip, ref marks_30);
                }
            }
            catch( System.Exception ex)
            {
                return Content("Ошибка построения");
            }

            value += "]";
            value_05 += "]";
            value_05_skip += "]";
            value_07 += "]";
            value_07_skip += "]";
            value_10 += "]";
            value_10_skip += "]";
            value_20 += "]";
            value_20_skip += "]";
            value_27 += "]";
            value_27_skip += "]";
            value_30 += "]";
            value_30_skip += "]";
            medianaValues += "]";

            ViewBag.value = value;
            ViewBag.value_05 = value_05;
            ViewBag.value_05_skip = value_05_skip;
            ViewBag.value_07 = value_07;
            ViewBag.value_07_skip = value_07_skip;
            ViewBag.value_10 = value_10;
            ViewBag.value_10_skip = value_10_skip;
            ViewBag.value_20 = value_20;
            ViewBag.value_20_skip = value_20_skip;
            ViewBag.value_27 = value_27;
            ViewBag.value_27_skip = value_27_skip;
            ViewBag.value_30 = value_30;
            ViewBag.value_30_skip = value_30_skip;
            ViewBag.mediana = medianaValues;

            ViewBag.marks_05 = marks_05;
            ViewBag.marks_07 = marks_07;
            ViewBag.marks_10 = marks_10;
            ViewBag.marks_20 = marks_20;
            ViewBag.marks_27 = marks_27;
            ViewBag.marks_30 = marks_30;
            
            return View();
        }
    }
}
