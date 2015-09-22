using GeospaceEntity.Models;
using GeospaceMediana.Models;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class DisturbanceController : Controller
    {
        //
        // GET: /Disturbance/

        public ActionResult Index(int YYYY = -1, int MM = -1)
        {
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.Month;
            }

            ViewDisturbanceList theViewData = new ViewDisturbanceList(YYYY, MM);            
            List<ViewDisturbance> theDisturbanceList = new List<ViewDisturbance>();
            ViewBag.YYYY = YYYY;
            ViewBag.MM = MM;


            Station stationSalekhard = Station.GetByCode(37701);
            theViewData.theStationList.Add(stationSalekhard);
            foreach (var item in Disturbance.GetByMonth(stationSalekhard, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }

            Station stationMagadan = Station.GetByCode(45601);
            theViewData.theStationList.Add(stationMagadan);
            foreach (var item in Disturbance.GetByMonth(stationMagadan, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }


            Station stationKhabarovsk = Station.GetByCode(43501);
            theViewData.theStationList.Add(stationKhabarovsk);
            foreach (var item in Disturbance.GetByMonth(stationKhabarovsk, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }          

            Station stationParatunka = Station.GetByCode(46501);
            theViewData.theStationList.Add(stationParatunka);
            foreach (var item in Disturbance.GetByMonth(stationParatunka, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }
            theViewData.theDisturbanceList = theDisturbanceList;
            return View(theViewData);
        }
        public ActionResult Submit(int station, int year, int month, int day, int hour,int duration)
        {
            try
            {
                Station stationCode = Station.GetByCode(station);
                for (int i = hour; i < hour + duration; ++i)
                {
                    GeospaceEntity.Models.Disturbance disturbanceSave = GeospaceEntity.Models.Disturbance.GetByTime(stationCode, year, month, day, i, 0);

                    if (disturbanceSave == null)
                    {
                        disturbanceSave = new GeospaceEntity.Models.Disturbance();

                        disturbanceSave.Station = stationCode;
                        disturbanceSave.YYYY = year;
                        disturbanceSave.MM = month;
                        disturbanceSave.DD = day;
                        disturbanceSave.HH = i;
                        disturbanceSave.MI = 0;

                        disturbanceSave.Save();
                    }
                    else
                    {
                        disturbanceSave.HH = i;
                        disturbanceSave.MI = 0;
                        disturbanceSave.Update();
                    }
                }
               

                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

        public ActionResult Delete(int station, int year, int month, int day, int hour, int duration)
        {
            try
            {
                Station stationCode = Station.GetByCode(station);
                for (int i = hour; i < hour + duration; ++i)
                {
                    GeospaceEntity.Models.Disturbance disturbanceDelete = GeospaceEntity.Models.Disturbance.GetByTime(stationCode, year, month, day, i, 0);

                    if (disturbanceDelete != null)
                    {
                        disturbanceDelete.Delete();
                    }
                }


                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

        public ActionResult GetHoursForHtmlModalBody(int StationCode, int YYYY, int MM, int DD)
        {            
            ViewBag.StationCode = StationCode;
            ViewBag.YYYY = YYYY;
            ViewBag.MM = MM;
            ViewBag.DD = DD;
            Station theStation = Station.GetByCode(StationCode);
            if (theStation != null) {
                List<Disturbance> theList = Disturbance.GetByDay(theStation, YYYY, MM, DD);
                return View(theList);
            }
            return View(new List<Disturbance>());
        }
        public ActionResult Add(int StationCode, int YYYY, int MM, int DD, int HH)
        {
            try
            {
                Station stationCode = Station.GetByCode(StationCode);
                GeospaceEntity.Models.Disturbance disturbanceSave = GeospaceEntity.Models.Disturbance.GetByTime(stationCode, YYYY, MM, DD, HH, 0);
                
                if (disturbanceSave == null)
                {
                    disturbanceSave = new GeospaceEntity.Models.Disturbance();
                    disturbanceSave.Station = stationCode;
                    disturbanceSave.YYYY = YYYY;
                    disturbanceSave.MM = MM;
                    disturbanceSave.DD = DD;
                    disturbanceSave.HH = HH;
                    disturbanceSave.MI = 0;
                    disturbanceSave.Save();

                }
                else
                {

                    disturbanceSave.Station = stationCode;
                    disturbanceSave.YYYY = YYYY;
                    disturbanceSave.MM = MM;
                    disturbanceSave.DD = DD;
                    disturbanceSave.HH = HH;
                    disturbanceSave.MI = 0;
                    disturbanceSave.Update();
                
                }





                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

        public ActionResult Remove(int StationCode, int YYYY, int MM, int DD, int HH)
        {
            try
            {
                Station theStation = Station.GetByCode(StationCode);

                GeospaceEntity.Models.Disturbance disturbanceDelete = GeospaceEntity.Models.Disturbance.GetByTime(theStation, YYYY, MM, DD, HH, 0);

                if (disturbanceDelete != null)
                {
                    disturbanceDelete.Delete();
                }
                


                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

        public ActionResult Display(int StationCode, int YYYY, int MM, int DD)
        {
            try
            {
                Station theStation = Station.GetByCode(StationCode);


                ViewDisturbanceList theViewData = new ViewDisturbanceList(YYYY, MM);
                List<ViewDisturbance> theDisturbanceList = new List<ViewDisturbance>();
                ViewBag.YYYY = YYYY;
                ViewBag.MM = MM;


                theViewData.theStationList.Add(theStation);
                foreach (var item in Disturbance.GetByDay(theStation, YYYY, MM, DD))
                {
                    ViewDisturbance theDisturbance = new ViewDisturbance(item);
                    theDisturbanceList.Add(theDisturbance);
                }

                theViewData.theDisturbanceList = theDisturbanceList;

                return Content(theViewData.DisplaySafe(StationCode, YYYY, MM, DD));
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }


        public ActionResult ExportToWord(int YYYY = -1, int MM = -1)
        {
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.Month;
            }


            Microsoft.Office.Interop.Word.Application wApp = null;
            try
            {

                object oMissing = System.Reflection.Missing.Value;

                wApp = new Microsoft.Office.Interop.Word.Application();
                if (wApp == null)
                {
                    ViewBag.Error += "wApp is null\r\n";
                }
                wApp.Visible = false;
                var wDocument = wApp.Documents.Add(ref oMissing, 
                    ref oMissing,
                    ref oMissing, ref oMissing);
                if (wDocument == null)
                {
                    ViewBag.Error += "wDocument is null\r\n";
                }
                ViewBag.Error += "2\r\n";
                //foreach (Microsoft.Office.Interop.Word.Section section in wDocument.Sections)
                //{
                //    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                //    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                //    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                //    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                //    headerRange.Font.Size = 14;
                //    headerRange.Text = "Таблица нарушения радиосвязи";
                //}

                ViewBag.Error += "3\r\n";
                wDocument.Content.SetRange(0, 0);

                var paragraphTable = wDocument.Paragraphs.Add();
                ViewBag.Error += "4\r\n";
                paragraphTable.Range.InsertParagraphAfter();
                ViewBag.Error += "5\r\n";
                ViewDisturbanceList theViewData = new ViewDisturbanceList(YYYY, MM);
                List<ViewDisturbance> theDisturbanceList = new List<ViewDisturbance>();
                ViewBag.YYYY = YYYY;
                ViewBag.MM = MM;


                Station stationSalekhard = Station.GetByCode(37701);
                theViewData.theStationList.Add(stationSalekhard);
                foreach (var item in Disturbance.GetByMonth(stationSalekhard, YYYY, MM))
                {
                    ViewDisturbance theDisturbance = new ViewDisturbance(item);
                    theDisturbanceList.Add(theDisturbance);
                }

                Station stationMagadan = Station.GetByCode(45601);
                theViewData.theStationList.Add(stationMagadan);
                foreach (var item in Disturbance.GetByMonth(stationMagadan, YYYY, MM))
                {
                    ViewDisturbance theDisturbance = new ViewDisturbance(item);
                    theDisturbanceList.Add(theDisturbance);
                }


                Station stationKhabarovsk = Station.GetByCode(43501);
                theViewData.theStationList.Add(stationKhabarovsk);
                foreach (var item in Disturbance.GetByMonth(stationKhabarovsk, YYYY, MM))
                {
                    ViewDisturbance theDisturbance = new ViewDisturbance(item);
                    theDisturbanceList.Add(theDisturbance);
                }

                Station stationParatunka = Station.GetByCode(46501);
                theViewData.theStationList.Add(stationParatunka);
                foreach (var item in Disturbance.GetByMonth(stationParatunka, YYYY, MM))
                {
                    ViewDisturbance theDisturbance = new ViewDisturbance(item);
                    theDisturbanceList.Add(theDisturbance);
                }
                theViewData.theDisturbanceList = theDisturbanceList;

                ViewBag.Error += "7\r\n";
                Table firstTable = wDocument.Tables.Add(paragraphTable.Range, DateTime.DaysInMonth(YYYY, MM)+1, theViewData.theStationList.Count() + 1 /*for Day Columnt*/);
                firstTable.Borders.Enable = 1;
                foreach (Row row in firstTable.Rows)
                {
                    if (row.IsFirst)
                    {
                        foreach(Cell cell in row.Cells)
                        {
                            if (cell.ColumnIndex == 1)
                            {
                                cell.Range.Text = "День";
                            }
                            else
                            {
                                cell.Range.Text = theViewData.theStationList[cell.ColumnIndex - 2].Name;
                            }

                        }
                    }
                    else
                    {

                        foreach (Cell cell in row.Cells)
                        {
                            if (cell.ColumnIndex == 1)
                            {
                                cell.Range.Text = (cell.RowIndex - 1).ToString("D2") ;
                            }
                            else
                            {
                                cell.Range.Text = theViewData.DisplaySafe(theViewData.theStationList[cell.ColumnIndex - 2].Code,
                                    YYYY,
                                    MM,
                                    cell.RowIndex - 1);
                            }

                        }
                    }
                    /*
                    foreach (Cell cell in row.Cells)
                    {

                        //Header row
                        if (cell.RowIndex == 1)
                        {
                            cell.Range.Text = "Column " + cell.ColumnIndex.ToString();
                            cell.Range.Font.Bold = 1;
                            //other format properties goes here
                            cell.Range.Font.Name = "verdana";
                            cell.Range.Font.Size = 10;
                            //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                            
                            cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                            //Center alignment for the Header cells
                            cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                        }
                        //Data row
                        else
                        {
                            cell.Range.Text = (cell.RowIndex - 2 + cell.ColumnIndex).ToString();
                        }
                    }
                     * */
                }


                string nameDoc = HttpContext.Server.MapPath("~/App_Data/");
                string fileName = "Disturbance.doc";
                string fileNameTemp = string.Format(@"{0}.doc", Guid.NewGuid());
                nameDoc += fileNameTemp;

                wApp.ActiveDocument.SaveAs2(nameDoc);
                wApp.ActiveDocument.Close(true);

                byte[] fileBytes = System.IO.File.ReadAllBytes(nameDoc);
                System.IO.File.Delete(nameDoc);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message + "\n" + ex.StackTrace + "DDD";
                if (ex.InnerException != null)
                {
                    ViewBag.Error += ex.InnerException.Message + "\r\n" + ex.StackTrace;
                }
                return View();
            }

            finally
            {
                if (wApp != null)
                {
                    wApp.Quit();
                    Marshal.FinalReleaseComObject(wApp);
                }
            }
        }


    }

}
