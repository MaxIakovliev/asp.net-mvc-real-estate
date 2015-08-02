using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRealty.Models;

namespace WebRealty.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        RealtyDb _db = new RealtyDb();
        public ActionResult Index()
        {
            var model = _db.Cities;
            return View(model);
        }

    }
}
