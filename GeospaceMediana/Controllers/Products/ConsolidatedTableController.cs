using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeospaceEntity.Models;
using Word = Microsoft.Office.Interop.Word; //Word  
using System.Reflection;
using System.Runtime.InteropServices;                    //Word 

namespace GeospaceMediana.Controllers
{
    public class ConsolidatedTableController : Controller
    {
        //
        // GET: /ConsolidatedTable/

        public virtual ActionResult Index( int YYYY = -1, int MM = -1, int api = 0)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
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
            ViewBag.IsLocal = Utils.Util.IsLocal();
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

        public ActionResult ExportToWord(int YYYY = -1, int MM = -1)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.Month;
            }

            Word._Application application = null;
            Word._Document document = null;
            try
            {
                Object missingObj = System.Reflection.Missing.Value;
                ViewBag.Error += "2";
                Object trueObj = true;
                Object falseObj = false;
                //создаем обьект приложения word
                application = new Word.Application();
                // создаем путь к файлу
                Object templatePathObj = HttpContext.Server.MapPath("~/App_Data/table2.dot");
                // если вылетим не этом этапе, приложение останется открытым
                ViewBag.Error += "0";
                try
                {
                    document = application.Documents.Add(ref  templatePathObj, ref missingObj, ref missingObj, ref missingObj);
                }
                catch (Exception error)
                {
                    ViewBag.Error += "111";
                    document.Close(ref falseObj, ref  missingObj, ref missingObj);
                    application.Quit(ref missingObj, ref  missingObj, ref missingObj);
                    document = null;
                    application = null;
                    
                    throw error;
                }
                application.Visible = false;
                Word.Table _table = document.Tables[1];
                // Получение данных о месяце
                IList<GeospaceEntity.Models.ConsolidatedTable> table_db = GeospaceEntity.Models.ConsolidatedTable.GetByDateMM(YYYY, MM);
                DateTime startMonth = new DateTime(YYYY, MM, 1);
                _table.Cell(1, 1).Range.Text = startMonth.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
                int day = 0;
                ViewBag.Error += "3";
                int correntDay = DateTime.DaysInMonth(YYYY, MM);
                for (int i = 0; i < correntDay; i++)
                {
                    if (i < correntDay-1) _table.Rows.Add(ref missingObj);

                    _table.Cell(5 + i, 1).Range.Text = (i + 1).ToString();
                    //day++;
                    if (table_db.Count > day)
                    {
                        if (table_db[day].DD == i + 1)
                        {
                            _table.Cell(5 + i, 2).Range.Text = table_db[day].Th2_W;
                            _table.Cell(5 + i, 3).Range.Text = table_db[day].Th3_Sp;
                            _table.Cell(5 + i, 4).Range.Text = table_db[day].Th4_F;
                            _table.Cell(5 + i, 5).Range.Text = table_db[day].Th5_90M;
                            _table.Cell(5 + i, 6).Range.Text = table_db[day].Th6_CountEvent;
                            _table.Cell(5 + i, 7).Range.Text = table_db[day].Th7_Balls;
                            _table.Cell(5 + i, 8).Range.Text = table_db[day].Th8_Coordinates;
                            _table.Cell(5 + i, 9).Range.Text = table_db[day].Th9_Time;
                            _table.Cell(5 + i, 10).Range.Text = table_db[day].Th10_RadioBursts;
                            _table.Cell(5 + i, 11).Range.Text = table_db[day].Th11_;
                            _table.Cell(5 + i, 12).Range.Text = table_db[day].Th12_AP;
                            _table.Cell(5 + i, 13).Range.Text = table_db[day].Th13_Amag;
                            _table.Cell(5 + i, 14).Range.Text = table_db[day].Th14_Apar;
                            _table.Cell(5 + i, 15).Range.Text = table_db[day].Th15_Akha;
                            _table.Cell(5 + i, 16).Range.Text = table_db[day].Th16_K;
                            _table.Cell(5 + i, 17).Range.Text = table_db[day].Th17_iSal;
                            _table.Cell(5 + i, 18).Range.Text = table_db[day].Th18_iMag;
                            _table.Cell(5 + i, 19).Range.Text = table_db[day].Th19_iKha;
                            _table.Cell(5 + i, 20).Range.Text = table_db[day].Th20_iPar;
                            day++;
                        }
                    }
                }
                string nameDoc = HttpContext.Server.MapPath("~/App_Data/");
                string fileName = "Сводная_таблица_" + startMonth.ToString("MMMM_yyyy", System.Globalization.CultureInfo.CurrentCulture)  + ".doc";
                string fileNameTemp = string.Format(@"{0}.doc", Guid.NewGuid());
                nameDoc += fileNameTemp;
                ViewBag.Error += "4";
                application.ActiveDocument.SaveAs2(nameDoc);
                application.ActiveDocument.Close(true);

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
                if (application != null)
                {
                    application.Quit();
                    Marshal.FinalReleaseComObject(application);
                }
            }
        }

    }
}
