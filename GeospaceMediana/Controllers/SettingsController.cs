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
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string GeospaceTrackExe = collection.Get("GeospaceTrackExe");
                Settings settings = new Settings()
                {
                    ID = id,
                    GeospaceTrackExe = GeospaceTrackExe
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
