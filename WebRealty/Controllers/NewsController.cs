using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRealty.Models;

namespace WebRealty.Controllers
{
    public class NewsController : Controller
    {
        RealtyDb _db = new RealtyDb();
        //
        // GET: /News/

        public ActionResult Index()
        {
            var model = (from s in _db.News.AsQueryable()
                         select s).OrderByDescending(s=>s.Id).Take(20);
            return View(model);
        }

        public PartialViewResult LastNews()
        {
            var model = (from s in _db.News.AsQueryable()
                         select s).Take(3);

            return PartialView("LastNews", model);
        }

        public ActionResult Detail(string id)
        {
            int tmp = 0;
            if (!int.TryParse(id, out tmp)) return View();

            var model = (from s in _db.News.AsQueryable()
                         where s.Id == tmp
                         select s).OrderByDescending(s => s.Id).SingleOrDefault<RealtyDomainObjects.News>();
            return View(model);


        }

    }
}
