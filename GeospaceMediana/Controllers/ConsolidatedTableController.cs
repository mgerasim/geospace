using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeospaceEntity.Models;

namespace GeospaceMediana.Controllers
{
    public class ConsolidatedTableController : Controller
    {
        //
        // GET: /ConsolidatedTable/

        public ActionResult Index( int YYYY = -1, int MM = -1)
        {
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.Month;
            }
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
       
        public ActionResult DeleteEvent(int id)
        {
            try
            {
                if (id != -1)
                {
                    EnergeticEvent delEvent = GeospaceEntity.Models.EnergeticEvent.GetById(id);
                    delEvent.Delete();
                }
                return Content("delete");
            }
            catch (Exception)
            {
                // return Content(e.ToString());
                return Content("Ошибка применения удалении! Проверьте корректность вводимых данных.");
            }
        }

    }
}
