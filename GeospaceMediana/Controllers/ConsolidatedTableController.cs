using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeospaceEntity.Models;
using Microsoft.Office.Interop.Word;

namespace GeospaceMediana.Controllers
{
    public class ConsolidatedTableController : Controller
    {
        //
        // GET: /ConsolidatedTable/

        public virtual ActionResult Index( int YYYY = -1, int MM = -1, int api = 0)
        {
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.Month;
            }
            ViewBag.Api = api;
            DateTime startMonth = new DateTime(YYYY, MM, 1);
            ViewBag.DateString = startMonth.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
            ViewBag.Year = YYYY;
            ViewBag.Month = MM;
            ViewBag.Date = startMonth;
            IList<ConsolidatedTable> tableView = ConsolidatedTable.GetByDateMM(YYYY, MM);
            return View(tableView);
        }
        public ActionResult Submit( int YYYY = -1, int MM = -1, int DD = -1, string type = "", string newvalue = "" )
        {
            try
            {
                ConsolidatedTable codeTable = ConsolidatedTable.GetByDateUTC(YYYY, MM, DD);

                if (codeTable == null) // Если запись отсутствует
                {
                    codeTable = new ConsolidatedTable();
                    codeTable.YYYY = YYYY;
                    codeTable.MM = MM;
                    codeTable.DD = DD;
                    codeTable.SetValueByType(type, newvalue);
                    codeTable.Save();
                }
                else
                {
                    codeTable.SetValueByType(type, newvalue);
                    codeTable.Update();
                }

                return Content("");
            }
            catch (Exception)
            {
                // return Content(e.ToString());
                return Content("Ошибка применения изменения! Проверьте корректность вводимых данных.");
            }
        }
       
        //public ActionResult ExportToWord(int YYYY = -1, int MM = -1)
        //{
        //    if (YYYY < 0)
        //    {
        //        YYYY = DateTime.Now.Year;
        //    }
        //    if (MM < 0)
        //    {
        //        MM = DateTime.Now.Month;
        //    }

        //    //Microsoft.Office.Interop.Word.Application wApp = null;
        //    //try
        //    //{

        //    //    IList<ConsolidatedTable> tableView = ConsolidatedTable.GetByDateMM(YYYY, MM);

        //    //    object oMissing = System.Reflection.Missing.Value;

        //    //    wApp = new Microsoft.Office.Interop.Word.Application();
        //    //    if (wApp == null)
        //    //    {
        //    //        ViewBag.Error += "wApp is null\r\n";
        //    //    }
        //    //    wApp.Visible = false;
        //    //    var wDocument = wApp.Documents.Add(ref oMissing,
        //    //        ref oMissing,
        //    //        ref oMissing, ref oMissing);
        //    //    if (wDocument == null)
        //    //    {
        //    //        ViewBag.Error += "wDocument is null\r\n";
        //    //    }
        //    //    ViewBag.Error += "2\r\n";
        //    //    foreach (Microsoft.Office.Interop.Word.Section section in wDocument.Sections)
        //    //    {
        //    //        Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
        //    //        headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
        //    //        headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //    //        headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
        //    //        headerRange.Font.Size = 14;
        //    //        headerRange.Text = "Сводная таблица";
        //    //    }

        //    //    ViewBag.Error += "3\r\n";
        //    //    wDocument.Content.SetRange(0, 0);

        //    //    var paragraphTable = wDocument.Paragraphs.Add();
        //    //    ViewBag.Error += "4\r\n";
        //    //    paragraphTable.Range.InsertParagraphAfter();
        //    //    ViewBag.Error += "5\r\n";

        //    //    ViewBag.Error += "7\r\n";
        //    //    Table firstTable = wDocument.Tables.Add(  (paragraphTable.Range, DateTime.DaysInMonth(YYYY, MM) + 1, theViewData.theStationList.Count() + 1 /*for Day Columnt*/);
        //    //    firstTable.Borders.Enable = 1;
        //    //    firstTable.Columns[1].PreferredWidth = 40f;

        //    //    foreach (Row row in firstTable.Rows)
        //    //    {
        //    //        if (row.IsFirst)
        //    //        {
        //    //            foreach (Cell cell in row.Cells)
        //    //            {
        //    //                if (cell.ColumnIndex == 1)
        //    //                {
        //    //                    cell.Range.Text = "День";
        //    //                }
        //    //                else
        //    //                {
        //    //                    cell.Range.Text = theViewData.theStationList[cell.ColumnIndex - 2].Name;
        //    //                }

        //    //            }
        //    //        }
        //    //        else
        //    //        {

        //    //            foreach (Cell cell in row.Cells)
        //    //            {
        //    //                if (cell.ColumnIndex == 1)
        //    //                {
        //    //                    cell.Range.Text = (cell.RowIndex - 1).ToString("D2");
        //    //                }
        //    //                else
        //    //                {
        //    //                    cell.Range.Text = theViewData.DisplaySafe(theViewData.theStationList[cell.ColumnIndex - 2].Code,
        //    //                        YYYY,
        //    //                        MM,
        //    //                        cell.RowIndex - 1);
        //    //                }

        //    //            }
        //    //        }

        //    //    }


        //    //    string nameDoc = HttpContext.Server.MapPath("~/App_Data/");
        //    //    string fileName = theViewData.Title + ".doc";
        //    //    string fileNameTemp = string.Format(@"{0}.doc", Guid.NewGuid());
        //    //    nameDoc += fileNameTemp;

        //    //    wApp.ActiveDocument.SaveAs2(nameDoc);
        //    //    wApp.ActiveDocument.Close(true);

        //    //    byte[] fileBytes = System.IO.File.ReadAllBytes(nameDoc);
        //    //    System.IO.File.Delete(nameDoc);
        //    //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    ViewBag.Error += ex.Message + "\n" + ex.StackTrace + "DDD";
        //    //    if (ex.InnerException != null)
        //    //    {
        //    //        ViewBag.Error += ex.InnerException.Message + "\r\n" + ex.StackTrace;
        //    //    }
        //    //    return View();
        //    //}

        //    //finally
        //    //{
        //    //    if (wApp != null)
        //    //    {
        //    //        wApp.Quit();
        //    //        Marshal.FinalReleaseComObject(wApp);
        //    //    }
        //    //}
        //}

    }
}
