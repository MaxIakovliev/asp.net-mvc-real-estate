using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRealty.Models;
using WebRealty.Common;

namespace WebRealty.Controllers
{
    public class AdController : Controller
    {
        RealtyDb _db = new RealtyDb();
        //
        // GET: /Ad/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewAd(string Id="")
        {
            int membershipId, iId;
            if (!string.IsNullOrEmpty(Id) &&
                int.TryParse(Id,out iId)
                && Auth.IsAuthenticated(Request, User, out membershipId))
            {

                var obj = (from s in _db.PropertyObjects
                      .Include("City")
                      .Include("CityDistrict")
                      .Include("PropertyType")
                      .Include("PropertyAction")
                      .Include("BuildingTypeName")
                      .Include("Currency")
                      .Include("PriceForTypeName")
                      .Include("UserOwner")
                      .Include("WCType")
                      .Include("CommercialPropertyType")
                      .Include("ServiceType")
                      .Include("Periods")
                           where s.Id == iId
                           select s).SingleOrDefault<RealtyDomainObjects.PropertyObject>();

                var propertyType = from s in _db.PropertyTypes
                                   select new { id = s.Id, name = s.PropertyTypeName };
               var sl = new SelectList(propertyType, "id", "name", obj.PropertyType.Id);
               ViewBag.PropertyTypes = sl;

                var PropertyTypeAction = from s in _db.PropertyActions
                                         select new { id = s.Id, name = s.PropertyActionName};
                sl = new SelectList(PropertyTypeAction, "id", "name", obj.PropertyAction.Id);
                ViewBag.propertyTypeAction = sl;

                var PropertyTypeCities = from s in _db.Cities
                                         select new { id = s.Id, name = s.CityName};
                sl = new SelectList(PropertyTypeCities, "id", "name", obj.City.Id);
                ViewBag.propertyTypeCities = sl;

                var PropertyTypeCityDistrict = from s in _db.CityDistricts
                                         select new { id = s.Id, name = s.District};
                sl = new SelectList(PropertyTypeCityDistrict, "id", "name", obj.CityDistrict.Id);
                ViewBag.propertyTypeCityDistrict = sl;

                ViewBag.AdId = iId;
                return View();

            }
            else
            {
                //var model = _db.PropertyTypes;
                var model = from s in _db.PropertyTypes
                            select new { id = s.Id, name = s.PropertyTypeName };
                var sl = new SelectList(model, "id", "name", 8);
                ViewBag.PropertyTypes = sl;
                return View();
            }
            
        }

        

    }
}
