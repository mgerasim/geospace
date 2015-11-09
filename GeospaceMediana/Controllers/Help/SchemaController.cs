using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class SchemaController : Controller
    {
        //
        // GET: /Schema/

        public ActionResult Update()
        {
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                GeospaceEntity.Common.NHibernateHelper.UpdateSchema();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return Content("База данных успешно обновлена!");
        }

    }
}
