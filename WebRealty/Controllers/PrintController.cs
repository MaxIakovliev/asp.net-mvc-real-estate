using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.Controllers
{
    public class PrintController : Controller
    {
        //
        // GET: /Print/
        RealtyDb _db = new RealtyDb();

        public ActionResult Index(string id)
        {
            int tmp = -1;
            if (!int.TryParse(id, out tmp))
            {
                return View("Home");
            }
            var po = (from s in _db.PropertyObjects.Include("City")
                                          .Include("CityDistrict")
                                          .Include("BuildingTypeName")
                                          .Include("PropertyAction")
                                          .Include("PropertyType")
                                          .Include("UserOwner")
                                          .Include("WCType")
                                          .Include("Currency")
                                          .Include("PriceForTypeName")
                                          .Include("Periods")
                                          .Include("UserOwner")
                                          .Include("ServiceType")

                      where s.Id == tmp
                      select s).SingleOrDefault<PropertyObject>();

            return View(po);
        }

    }
}
