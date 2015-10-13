using GeospaceEntity.Common;
using GeospaceEntity.Models;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/

        public ActionResult Index()
        {
            IRepository<Settings> repo = new SettingsRepository();
            Settings settings;
            List<Settings> theSettings = (List<Settings>)repo.GetAll();
            if (theSettings.Count == 0)
            {
                settings = new Settings();
                repo.Save(settings);
            }
            else
            {
                settings = theSettings[0];
            }
            return RedirectToAction("Details", new { ID = settings.ID });
        }

        //
        // GET: /Settings/Details/5

        public ActionResult Details(int id)
        {
            IRepository<Settings> repo = new SettingsRepository();
            return View(repo.GetById(id));
        }

        //
        // GET: /Settings/Edit/5

        public ActionResult Edit(int id)
        {
            IRepository<Settings> repo = new SettingsRepository();
            return View(repo.GetById(id));
        }

        //
        // POST: /Settings/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string MonthForecastTrack = collection.Get("MonthForecastTrack");
                string FiveDayForecastTrack = collection.Get("FiveDayForecastTrack");
                string CalcTrack = collection.Get("CalcTrack");
                string DATADIR = collection.Get("DATADIR");
                string SNMP_host = collection.Get("SNMP_host");
                int SNMP_port = Convert.ToInt32(collection.Get("SNMP_port"));
                string Email_ASPD_To = collection.Get("Email_ASPD_To");
                string Email_ASPD_From = collection.Get("Email_ASPD_From");
                Settings settings = new Settings()
                {
                    ID = id,
                    MonthForecastTrack = MonthForecastTrack,
                    FiveDayForecastTrack = FiveDayForecastTrack,
                    CalcTrack = CalcTrack,
                    DATADIR = DATADIR,
                    SNMP_host = SNMP_host,
                    SNMP_port = SNMP_port,
                    Email_ASPD_To = Email_ASPD_To,
                    Email_ASPD_From = Email_ASPD_From
                };

                IRepository<Settings> repo = new SettingsRepository();
                repo.Update(settings);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
