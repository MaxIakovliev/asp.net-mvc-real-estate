using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRealty.Models;
using RealtyDomainObjects;
using System.IO;
using WebRealty.Common;
using Microsoft.Security.Application;

namespace WebRealty.Controllers
{
    public class PropertyManipulationController : Controller
    {
        //
        // GET: /PropertyManipulation/

        string errMsg = "Ошибка, работа приложения нарушена. попробуйте начать с главной страницы. Извините за не удобства";
        RealtyDb _db = new RealtyDb();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateDateOfPublication(string id)
        {
            id = Server.HtmlEncode(id);
            id = Encoder.HtmlEncode(id);
            
            int membershipId, iId;
            if (!int.TryParse(id, out iId) ||
                !Auth.IsAuthenticated(Request, User, out membershipId))
                return RedirectToAction("index", "home");

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
                       select s).SingleOrDefault<PropertyObject>();

            obj.CreatedDate = DateTime.Now;
            _db.PropertyObjects.Attach(obj);
            _db.Entry(obj).State = System.Data.EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("index", "PrivateRoom");
        }


        public ActionResult ChangeStateAd(string id, string state)
        {
            id = Server.HtmlEncode(id);
            state = Server.HtmlEncode(state);

            id = Encoder.HtmlEncode(id);
            state = Encoder.HtmlEncode(state);

            int membershipId, iId, iState;
            if (!int.TryParse(state, out iState) ||
                !int.TryParse(id, out iId) ||
                !Auth.IsAuthenticated(Request, User, out membershipId)
                || (iState < 0 && iState > 2))
                return RedirectToAction("index", "home");

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
                       select s).SingleOrDefault<PropertyObject>();

            obj.IsDeleted = iState;
            _db.PropertyObjects.Attach(obj);
            _db.Entry(obj).State = System.Data.EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("index", "PrivateRoom");
        }

        public ActionResult ShowForUpdatePropertyObject(string id)
        {
            id = Server.HtmlEncode(id);
            id = Encoder.HtmlEncode(id);

            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Content(string.Empty);

            int iID = -1;
            if (!int.TryParse(id, out iID))
                return RedirectToAction("index", "home");

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

                       where s.Id == iID
                       select s).SingleOrDefault<PropertyObject>();

            if (obj == null)//not found
                return RedirectToAction("index", "home");

            SearchPageParams spr = new SearchPageParams()
            {
                iPropertyType = obj.PropertyType.Id,//iPropertyType,
                iPropertyTypeAction = obj.PropertyAction.Id,//iPropertyTypeAction,
                iPropertyTypeCities = obj.City.Id,//iPropertyTypeCities,
                iPropertyTypeCityDistrict = obj.CityDistrict.Id//iPropertyTypeCityDistrict
            };
            ViewBag.SearchPageParams = spr;

            SelectList sl = null;

            var currency = from s in _db.CurrencyTypes
                           //               select s;
                           select new { id = s.Id, name = s.CurrencyTypeName };

            sl = new SelectList(currency, "id", "name", obj.Currency.Id);

            ViewBag.currency = sl;

            var priceForType = from s in _db.PriceForTypes
                               select new { id = s.Id, name = s.PriveForTypeName };


            switch (obj.PropertyType.Id)
            {
                case 1:
                    var buildingType = from s in _db.BuildingTypes.AsQueryable()
                                       select new { id = s.Id, name = s.BuildingTypeName };
                    if (obj.BuildingTypeName != null)
                        sl = new SelectList(buildingType, "id", "name", obj.BuildingTypeName.Id.ToString());
                    else
                        sl = new SelectList(buildingType, "id", "name");
                    ViewBag.buildingType = sl;

                    var wcType = from s in _db.WCTypes.AsQueryable()
                                 select new { id = s.Id, name = s.WCTypeName };
                    if (obj.WCType != null)
                        sl = new SelectList(wcType, "id", "name", obj.WCType.Id);
                    else
                        sl = new SelectList(wcType, "id", "name");
                    ViewBag.wcType = sl;

                    if (obj.PriceForTypeName != null)
                        sl = new SelectList(priceForType, "id", "name", obj.PriceForTypeName.Id.ToString());
                    else
                        sl = new SelectList(priceForType, "id", "name");
                    ViewBag.priceForType = sl;

                    return PartialView("AddSellFlat", obj);

                case 2:

                    //sl = new SelectList(currency, "id", "name");

                    //ViewBag.currency = sl;

                    var chooseTypeAction = (from s in _db.PropertyActions
                                            where s.Id == obj.PropertyAction.Id//iPropertyTypeAction
                                            select s.PropertyActionName).Single<string>();
                    //выбрать на основе типа дейстивия /аренда/продажа, кикие еэлементы показывать а какие нет
                    if (chooseTypeAction.ToLower().Contains("аренда"))
                    {
                        var periods = from s in _db.Periods.AsQueryable()
                                      select new { id = s.Id, name = s.PeriodName };
                        if (obj.Periods != null)
                            sl = new SelectList(periods, "id", "name", obj.Periods.Id);
                        else
                            sl = new SelectList(periods, "id", "name");
                        ViewBag.periods = sl;
                    }
                    else
                    {
                        if (obj.PriceForTypeName != null)
                            sl = new SelectList(priceForType, "Id", "name", obj.PriceForTypeName.Id);
                        else
                            sl = new SelectList(priceForType, "Id", "name");
                        ViewBag.priceForType = sl;
                    }
                    return PartialView("AddCountryHouse", obj);

                case 3:
                    //sl = new SelectList(currency, "id", "name");
                    //ViewBag.currency = sl;

                    var landCommunication = (from s in _db.LandCommunications
                                             select s).ToList<LandCommunication>();
                    ViewBag.landCommunication = landCommunication;

                    var landFunction = (from s in _db.LandFunctions
                                        select s).ToList<LandFunction>();
                    ViewBag.landFunction = landFunction;

                    return PartialView("AddLand", obj);

                case 4:
                    //sl = new SelectList(currency, "id", "name");
                    //ViewBag.currency = sl;

                    var commercialPropertyType = from s in _db.CommercialPropertyTypes
                                                 select new { id = s.Id, name = s.CommercialPropertyTypeName };
                    if (obj.CommercialPropertyType != null)
                        sl = new SelectList(commercialPropertyType, "id", "name", obj.CommercialPropertyType.Id.ToString());
                    else
                        sl = new SelectList(commercialPropertyType, "id", "name");

                    ViewBag.commercialPropertyType = sl;

                    if (obj.PriceForTypeName != null)
                        sl = new SelectList(priceForType, "id", "name", obj.PriceForTypeName.Id.ToString());
                    else
                        sl = new SelectList(priceForType, "id", "name");
                    ViewBag.priceForType = sl;

                    return PartialView("AddCommercialProperty", obj);

                case 5:
                    //sl = new SelectList(currency, "id", "name");
                    //ViewBag.currency = sl;
                    if (obj.PriceForTypeName != null)
                        sl = new SelectList(priceForType, "id", "name", obj.PriceForTypeName.Id.ToString());
                    else
                        sl = new SelectList(priceForType, "id", "name");
                    ViewBag.priceForType = sl;

                    return PartialView("AddParkingGarage", obj);

                case 6:
                    //sl = new SelectList(currency, "id", "name");
                    //ViewBag.currency = sl;
                    if (obj.PriceForTypeName != null)
                        sl = new SelectList(priceForType, "id", "name", obj.PriceForTypeName.Id.ToString());
                    else
                        sl = new SelectList(priceForType, "id", "name");
                    ViewBag.priceForType = sl;

                    return PartialView("AddParkingGarage", obj);

                case 7:
                    //sl = new SelectList(currency, "id", "name");
                    //ViewBag.currency = sl;

                    if (obj.PriceForTypeName != null)
                        sl = new SelectList(priceForType, "id", "name", obj.PriceForTypeName.Id.ToString());
                    else
                        sl = new SelectList(priceForType, "id", "name");
                    ViewBag.priceForType = sl;

                    var serviceTypes = from s in _db.ServiceTypes
                                       select new { id = s.Id, name = s.ServiceTypeName };
                    if (obj.ServiceType != null)
                        sl = new SelectList(serviceTypes, "id", "name", obj.ServiceType.Id.ToString());
                    else
                        sl = new SelectList(serviceTypes, "id", "name");
                    ViewBag.serviceTypes = sl;
                    return PartialView("AddService", obj);
                default:
                    return RedirectToAction("index", "home");
            }



        }

        [OutputCache(Duration=60,VaryByParam="*")]
        public PartialViewResult AddSellFlat(string PropertyType, string PropertyTypeAction, string PropertyTypeCities, string PropertyTypeCityDistrict, string id = "")
        {
            
            PropertyType = Server.HtmlEncode(PropertyType);
            PropertyTypeAction = Server.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Server.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Server.HtmlEncode(PropertyTypeCityDistrict);
            id = Server.HtmlEncode(id);
            

            PropertyType = Encoder.HtmlEncode(PropertyType);
            PropertyTypeAction = Encoder.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Encoder.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Encoder.HtmlEncode(PropertyTypeCityDistrict);
            id = Encoder.HtmlEncode(id);
             

            int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict, membershipId;

            if (int.TryParse(PropertyType, out iPropertyType) &&
                int.TryParse(PropertyTypeAction, out iPropertyTypeAction) &&
                int.TryParse(PropertyTypeCities, out iPropertyTypeCities) &&
                int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict))
            {

                SearchPageParams spr = new SearchPageParams()
                {
                    iPropertyType = iPropertyType,
                    iPropertyTypeAction = iPropertyTypeAction,
                    iPropertyTypeCities = iPropertyTypeCities,
                    iPropertyTypeCityDistrict = iPropertyTypeCityDistrict
                };
                ViewBag.SearchPageParams = spr;


                PropertyObject obj = null;
                if (Auth.IsAuthenticated(Request, User, out membershipId))
                {
                    var userDetails = Auth.GetUserDetails(membershipId, _db);
                    if (!string.IsNullOrEmpty(id))
                    {
                        obj = GetPropertyObject(id);
                        if (obj != null)
                        {
                            if (string.IsNullOrEmpty(obj.Phone1)) obj.Phone1 = userDetails.Phone1;
                            if (string.IsNullOrEmpty(obj.Phone2)) obj.Phone2 = userDetails.Phone2;
                            if (string.IsNullOrEmpty(obj.Phone3)) obj.Phone3 = userDetails.Phone3;
                            if (string.IsNullOrEmpty(obj.SourceUrl)) obj.SourceUrl = userDetails.SourceUrl;
                            if (string.IsNullOrEmpty(obj.ContactName)) obj.ContactName = userDetails.ContactName;
                        }
                    }
                }


                SelectList sl = null;
                var buildingType = from s in _db.BuildingTypes.AsQueryable()
                                   select new { id = s.Id, name = s.BuildingTypeName };
                if (obj != null && obj.BuildingTypeName != null)
                    sl = new SelectList(buildingType, "id", "name", obj.BuildingTypeName.Id.ToString());
                else
                    sl = new SelectList(buildingType, "id", "name");
                ViewBag.buildingType = sl;


                var wcType = from s in _db.WCTypes.AsQueryable()
                             select new { id = s.Id, name = s.WCTypeName };

                if (obj != null && obj.WCType != null)
                    sl = new SelectList(wcType, "id", "name", obj.WCType.Id.ToString());
                else
                    sl = new SelectList(wcType, "id", "name");
                ViewBag.wcType = sl;


                var currency = from s in _db.CurrencyTypes.AsQueryable()
                               select new { id = s.Id, name = s.CurrencyTypeName };
                if (obj != null && obj.Currency != null)
                    sl = new SelectList(currency, "id", "name", obj.Currency.Id.ToString());
                else
                    sl = new SelectList(currency, "id", "name");
                ViewBag.currency = sl;

                var chooseTypeAction = (from s in _db.PropertyActions
                                        where s.Id == iPropertyTypeAction
                                        select s.PropertyActionName).Single<string>();
                //выбрать на основе типа дейстивия /аренда/продажа, кикие еэлементы показывать а какие нет
                if (chooseTypeAction.ToLower().Contains("аренда"))
                {
                    var periods = from s in _db.Periods.AsQueryable()
                                  select new { id = s.Id, name = s.PeriodName };
                    if (obj != null && obj.Periods != null)
                        sl = new SelectList(periods, "id", "name", obj.Periods.Id.ToString());
                    else
                        sl = new SelectList(periods, "id", "name");
                    ViewBag.periods = sl;
                }
                else
                {
                    var priceForType = from s in _db.PriceForTypes.AsQueryable()
                                       select new { id = s.Id, name = s.PriveForTypeName };
                    if (obj != null && obj.PriceForTypeName != null)
                        sl = new SelectList(priceForType, "id", "name", obj.PriceForTypeName.Id.ToString());
                    else
                        sl = new SelectList(priceForType, "id", "name");
                    ViewBag.priceForType = sl;
                }



                if (Auth.IsAuthenticated(Request, User, out membershipId))
                {
                    var userDetails = Auth.GetUserDetails(membershipId, _db);
                    if (!string.IsNullOrEmpty(id) && obj != null)
                        return PartialView("AddSellFlat", obj);
                    return PartialView("AddSellFlat", userDetails);
                }

                return PartialView("AddSellFlat");
            }
            else
            {
                Content(errMsg);
            }
            return null;

        }

        [OutputCache(Duration = 60, VaryByParam = "*")]
        public PartialViewResult AddCountryHouse(string PropertyType, string PropertyTypeAction, string PropertyTypeCities, string PropertyTypeCityDistrict, string id = "")
        {
            
            PropertyType = Server.HtmlEncode(PropertyType);
            PropertyTypeAction = Server.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Server.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Server.HtmlEncode(PropertyTypeCityDistrict);
            id = Server.HtmlEncode(id);


            PropertyType = Encoder.HtmlEncode(PropertyType);
            PropertyTypeAction = Encoder.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Encoder.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Encoder.HtmlEncode(PropertyTypeCityDistrict);
            id = Encoder.HtmlEncode(id);


            int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict;

            if (int.TryParse(PropertyType, out iPropertyType) &&
                int.TryParse(PropertyTypeAction, out iPropertyTypeAction) &&
                int.TryParse(PropertyTypeCities, out iPropertyTypeCities) &&
                int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict))
            {
                SearchPageParams spr = new SearchPageParams()
                {
                    iPropertyType = iPropertyType,
                    iPropertyTypeAction = iPropertyTypeAction,
                    iPropertyTypeCities = iPropertyTypeCities,
                    iPropertyTypeCityDistrict = iPropertyTypeCityDistrict
                };
                ViewBag.SearchPageParams = spr;

                var currency = from s in _db.CurrencyTypes.AsQueryable()
                               select new { id = s.Id, name = s.CurrencyTypeName };
                var sl = new SelectList(currency, "id", "name");
                ViewBag.currency = sl;

                var chooseTypeAction = (from s in _db.PropertyActions
                                        where s.Id == iPropertyTypeAction
                                        select s.PropertyActionName).Single<string>();
                //выбрать на основе типа дейстивия /аренда/продажа, кикие еэлементы показывать а какие нет
                if (chooseTypeAction.ToLower() == "аренда")
                {
                    var periods = from s in _db.Periods.AsQueryable()
                                  select new { id = s.Id, name = s.PeriodName };
                    sl = new SelectList(periods, "id", "name");
                    ViewBag.periods = sl;
                }
                else
                {
                    var priceForType = from s in _db.PriceForTypes.AsQueryable()
                                       select new { id = s.Id, name = s.PriveForTypeName };
                    sl = new SelectList(priceForType, "id", "name");
                    ViewBag.priceForType = sl;
                }

                int membershipId;
                if (Auth.IsAuthenticated(Request, User, out membershipId))
                {
                    var userDetails = Auth.GetUserDetails(membershipId, _db);
                    if (!string.IsNullOrEmpty(id))
                    {
                        PropertyObject obj = null;
                        obj = GetPropertyObject(id);
                        if (obj != null)
                        {
                            if (!string.IsNullOrEmpty(obj.Phone1)) obj.Phone1 = userDetails.Phone1;
                            if (!string.IsNullOrEmpty(obj.Phone2)) obj.Phone1 = userDetails.Phone2;
                            if (!string.IsNullOrEmpty(obj.Phone3)) obj.Phone1 = userDetails.Phone3;
                            if (!string.IsNullOrEmpty(obj.SourceUrl)) obj.Phone1 = userDetails.SourceUrl;
                            if (!string.IsNullOrEmpty(obj.ContactName)) obj.Phone1 = userDetails.ContactName;
                            return PartialView("AddCountryHouse", obj);
                        }
                    }
                    return PartialView("AddCountryHouse", userDetails);
                }
                return PartialView("AddCountryHouse");
            }
            else
            {
                Content(errMsg);
            }
            return null;
        }

        [OutputCache(Duration = 60, VaryByParam = "*")]
        public PartialViewResult AddLand(string PropertyType, string PropertyTypeAction, string PropertyTypeCities, string PropertyTypeCityDistrict, string id = "")
        {
            PropertyType = Server.HtmlEncode(PropertyType);
            PropertyTypeAction = Server.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Server.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Server.HtmlEncode(PropertyTypeCityDistrict);
            id = Server.HtmlEncode(id);


            PropertyType = Encoder.HtmlEncode(PropertyType);
            PropertyTypeAction = Encoder.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Encoder.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Encoder.HtmlEncode(PropertyTypeCityDistrict);
            id = Encoder.HtmlEncode(id);

            int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict;

            if (int.TryParse(PropertyType, out iPropertyType) &&
                int.TryParse(PropertyTypeAction, out iPropertyTypeAction) &&
                int.TryParse(PropertyTypeCities, out iPropertyTypeCities) &&
                int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict))
            {
                SearchPageParams spr = new SearchPageParams()
                {
                    iPropertyType = iPropertyType,
                    iPropertyTypeAction = iPropertyTypeAction,
                    iPropertyTypeCities = iPropertyTypeCities,
                    iPropertyTypeCityDistrict = iPropertyTypeCityDistrict
                };
                ViewBag.SearchPageParams = spr;

                var currency = from s in _db.CurrencyTypes.AsQueryable()
                               select new { id = s.Id, name = s.CurrencyTypeName };
                var sl = new SelectList(currency, "id", "name");
                ViewBag.currency = sl;

                var landCommunication = (from s in _db.LandCommunications
                                         select s).ToList<LandCommunication>();
                ViewBag.landCommunication = landCommunication;

                var landFunction = (from s in _db.LandFunctions
                                    select s).ToList<LandFunction>();

                ViewBag.landFunction = landFunction;

                int membershipId;
                if (Auth.IsAuthenticated(Request, User, out membershipId))
                {
                    var userDetails = Auth.GetUserDetails(membershipId, _db);
                    if (!string.IsNullOrEmpty(id))
                    {
                        PropertyObject obj = null;
                        obj = GetPropertyObject(id);
                        if (obj != null)
                        {
                            if (!string.IsNullOrEmpty(obj.Phone1)) obj.Phone1 = userDetails.Phone1;
                            if (!string.IsNullOrEmpty(obj.Phone2)) obj.Phone1 = userDetails.Phone2;
                            if (!string.IsNullOrEmpty(obj.Phone3)) obj.Phone1 = userDetails.Phone3;
                            if (!string.IsNullOrEmpty(obj.SourceUrl)) obj.Phone1 = userDetails.SourceUrl;
                            if (!string.IsNullOrEmpty(obj.ContactName)) obj.Phone1 = userDetails.ContactName;
                            return PartialView("AddLand", obj);
                        }
                    }
                    return PartialView("AddLand", userDetails);
                }
                return PartialView("AddLand");
            }
            else
            {
                Content(errMsg);
            }
            return null;
        }

        [OutputCache(Duration = 60, VaryByParam = "*")]
        public PartialViewResult AddCommercialProperty(string PropertyType, string PropertyTypeAction, string PropertyTypeCities, string PropertyTypeCityDistrict, string id = "")
        {

            PropertyType = Server.HtmlEncode(PropertyType);
            PropertyTypeAction = Server.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Server.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Server.HtmlEncode(PropertyTypeCityDistrict);
            id = Server.HtmlEncode(id);

            PropertyType = Encoder.HtmlEncode(PropertyType);
            PropertyTypeAction = Encoder.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Encoder.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Encoder.HtmlEncode(PropertyTypeCityDistrict);
            id = Encoder.HtmlEncode(id);

            //Encoder
            int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict;

            if (int.TryParse(PropertyType, out iPropertyType) &&
                int.TryParse(PropertyTypeAction, out iPropertyTypeAction) &&
                int.TryParse(PropertyTypeCities, out iPropertyTypeCities) &&
                int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict))
            {
                SearchPageParams spr = new SearchPageParams()
                {
                    iPropertyType = iPropertyType,
                    iPropertyTypeAction = iPropertyTypeAction,
                    iPropertyTypeCities = iPropertyTypeCities,
                    iPropertyTypeCityDistrict = iPropertyTypeCityDistrict
                };
                ViewBag.SearchPageParams = spr;

                var currency = from s in _db.CurrencyTypes.AsQueryable()
                               select new { id = s.Id, name = s.CurrencyTypeName };
                var sl = new SelectList(currency, "id", "name");
                ViewBag.currency = sl;

                var commercialPropertyType = from s in _db.CommercialPropertyTypes
                                             select new { id = s.Id, name = s.CommercialPropertyTypeName };
                sl = new SelectList(commercialPropertyType, "id", "name");
                ViewBag.commercialPropertyType = sl;


                var priceForType = from s in _db.PriceForTypes.AsQueryable()
                                   select new { id = s.Id, name = s.PriveForTypeName };
                sl = new SelectList(priceForType, "id", "name");
                ViewBag.priceForType = sl;

                int membershipId;
                if (Auth.IsAuthenticated(Request, User, out membershipId))
                {
                    var userDetails = Auth.GetUserDetails(membershipId, _db);
                    if (!string.IsNullOrEmpty(id))
                    {
                        PropertyObject obj = null;
                        obj = GetPropertyObject(id);
                        if (obj != null)
                        {
                            if (!string.IsNullOrEmpty(obj.Phone1)) obj.Phone1 = userDetails.Phone1;
                            if (!string.IsNullOrEmpty(obj.Phone2)) obj.Phone1 = userDetails.Phone2;
                            if (!string.IsNullOrEmpty(obj.Phone3)) obj.Phone1 = userDetails.Phone3;
                            if (!string.IsNullOrEmpty(obj.SourceUrl)) obj.Phone1 = userDetails.SourceUrl;
                            if (!string.IsNullOrEmpty(obj.ContactName)) obj.Phone1 = userDetails.ContactName;
                            return PartialView("AddCommercialProperty", obj);
                        }
                    }
                    return PartialView("AddCommercialProperty", userDetails);
                }
                return PartialView("AddCommercialProperty");
            }
            else
            {
                Content(errMsg);
            }
            return null;
        }

        [OutputCache(Duration = 60, VaryByParam = "*")]
        public PartialViewResult AddParkingGarage(string PropertyType, string PropertyTypeAction, string PropertyTypeCities, string PropertyTypeCityDistrict, string id = "")
        {

            PropertyType = Server.HtmlEncode(PropertyType);
            PropertyTypeAction = Server.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Server.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Server.HtmlEncode(PropertyTypeCityDistrict);
            id = Server.HtmlEncode(id);

            PropertyType = Encoder.HtmlEncode(PropertyType);
            PropertyTypeAction = Encoder.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Encoder.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Encoder.HtmlEncode(PropertyTypeCityDistrict);
            id = Encoder.HtmlEncode(id);

            //Encoder

            int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict;

            if (int.TryParse(PropertyType, out iPropertyType) &&
                int.TryParse(PropertyTypeAction, out iPropertyTypeAction) &&
                int.TryParse(PropertyTypeCities, out iPropertyTypeCities) &&
                int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict))
            {
                SearchPageParams spr = new SearchPageParams()
                {
                    iPropertyType = iPropertyType,
                    iPropertyTypeAction = iPropertyTypeAction,
                    iPropertyTypeCities = iPropertyTypeCities,
                    iPropertyTypeCityDistrict = iPropertyTypeCityDistrict
                };
                ViewBag.SearchPageParams = spr;

                var currency = from s in _db.CurrencyTypes.AsQueryable()
                               select new { id = s.Id, name = s.CurrencyTypeName };
                var sl = new SelectList(currency, "id", "name");
                ViewBag.currency = sl;


                var priceForType = from s in _db.PriceForTypes.AsQueryable()
                                   select new { id = s.Id, name = s.PriveForTypeName };
                sl = new SelectList(priceForType, "id", "name");
                ViewBag.priceForType = sl;

                int membershipId;
                if (Auth.IsAuthenticated(Request, User, out membershipId))
                {
                    var userDetails = Auth.GetUserDetails(membershipId, _db);
                    if (!string.IsNullOrEmpty(id))
                    {
                        PropertyObject obj = null;
                        obj = GetPropertyObject(id);
                        if (obj != null)
                        {
                            if (!string.IsNullOrEmpty(obj.Phone1)) obj.Phone1 = userDetails.Phone1;
                            if (!string.IsNullOrEmpty(obj.Phone2)) obj.Phone1 = userDetails.Phone2;
                            if (!string.IsNullOrEmpty(obj.Phone3)) obj.Phone1 = userDetails.Phone3;
                            if (!string.IsNullOrEmpty(obj.SourceUrl)) obj.Phone1 = userDetails.SourceUrl;
                            if (!string.IsNullOrEmpty(obj.ContactName)) obj.Phone1 = userDetails.ContactName;
                            return PartialView("AddParkingGarage", obj);
                        }
                    }
                    return PartialView("AddParkingGarage", userDetails);
                }
                return PartialView("AddParkingGarage");
            }
            else
            {
                Content(errMsg);
            }
            return null;
        }

        [OutputCache(Duration = 60, VaryByParam = "*")]
        public PartialViewResult AddAbroadProperty(string PropertyType, string PropertyTypeAction, string id = "")
        {

            PropertyType = Server.HtmlEncode(PropertyType);
            PropertyTypeAction = Server.HtmlEncode(PropertyTypeAction);
            id = Server.HtmlEncode(id);

            PropertyType = Encoder.HtmlEncode(PropertyType);
            PropertyTypeAction = Encoder.HtmlEncode(PropertyTypeAction);
            id = Encoder.HtmlEncode(id);

            //Encoder

            int iPropertyType, iPropertyTypeAction;

            if (int.TryParse(PropertyType, out iPropertyType) &&
                int.TryParse(PropertyTypeAction, out iPropertyTypeAction))
            {
                SearchPageParams spr = new SearchPageParams()
                {
                    iPropertyType = iPropertyType,
                    iPropertyTypeAction = iPropertyTypeAction,
                    iPropertyTypeCities = -1,
                    iPropertyTypeCityDistrict = -1
                };
                ViewBag.SearchPageParams = spr;

                var currency = from s in _db.CurrencyTypes.AsQueryable()
                               select new { id = s.Id, name = s.CurrencyTypeName };
                var sl = new SelectList(currency, "id", "name");
                ViewBag.currency = sl;


                var priceForType = from s in _db.PriceForTypes.AsQueryable()
                                   select new { id = s.Id, name = s.PriveForTypeName };
                sl = new SelectList(priceForType, "id", "name");
                ViewBag.priceForType = sl;

                int membershipId;
                if (Auth.IsAuthenticated(Request, User, out membershipId))
                {
                    var userDetails = Auth.GetUserDetails(membershipId, _db);
                    if (!string.IsNullOrEmpty(id))
                    {
                        PropertyObject obj = null;
                        obj = GetPropertyObject(id);
                        if (obj != null)
                        {
                            if (!string.IsNullOrEmpty(obj.Phone1)) obj.Phone1 = userDetails.Phone1;
                            if (!string.IsNullOrEmpty(obj.Phone2)) obj.Phone1 = userDetails.Phone2;
                            if (!string.IsNullOrEmpty(obj.Phone3)) obj.Phone1 = userDetails.Phone3;
                            if (!string.IsNullOrEmpty(obj.SourceUrl)) obj.Phone1 = userDetails.SourceUrl;
                            if (!string.IsNullOrEmpty(obj.ContactName)) obj.Phone1 = userDetails.ContactName;
                            return PartialView("AddParkingGarage", obj);
                        }
                    }
                    return PartialView("AddParkingGarage", userDetails);
                }
                return PartialView("AddParkingGarage");
            }
            else
            {
                Content(errMsg);
            }
            return null;
        }

        [OutputCache(Duration = 60, VaryByParam = "*")]
        public PartialViewResult AddService(string PropertyType, string id = "")
        {

            PropertyType = Server.HtmlEncode(PropertyType);
            id = Server.HtmlEncode(id);

            PropertyType = Encoder.HtmlEncode(PropertyType);
            id = Encoder.HtmlEncode(id);

            int iPropertyType;

            if (int.TryParse(PropertyType, out iPropertyType))
            {
                SearchPageParams spr = new SearchPageParams()
                {
                    iPropertyType = iPropertyType,
                    iPropertyTypeAction = -1,
                    iPropertyTypeCities = -1,
                    iPropertyTypeCityDistrict = -1
                };
                ViewBag.SearchPageParams = spr;

                var currency = from s in _db.CurrencyTypes.AsQueryable()
                               select new { id = s.Id, name = s.CurrencyTypeName };
                var sl = new SelectList(currency, "id", "name");
                ViewBag.currency = sl;

                var priceForType = from s in _db.PriceForTypes.AsQueryable()
                                   select new { id = s.Id, name = s.PriveForTypeName };
                sl = new SelectList(priceForType, "id", "name");
                ViewBag.priceForType = sl;

                var serviceTypes = from s in _db.ServiceTypes
                                   select new { id = s.Id, name = s.ServiceTypeName };
                sl = new SelectList(serviceTypes, "id", "name");
                ViewBag.serviceTypes = sl;

                int membershipId;
                if (Auth.IsAuthenticated(Request, User, out membershipId))
                {
                    var userDetails = Auth.GetUserDetails(membershipId, _db);
                    if (!string.IsNullOrEmpty(id))
                    {
                        PropertyObject obj = null;
                        obj = GetPropertyObject(id);
                        if (obj != null)
                        {
                            if (!string.IsNullOrEmpty(obj.Phone1)) obj.Phone1 = userDetails.Phone1;
                            if (!string.IsNullOrEmpty(obj.Phone2)) obj.Phone1 = userDetails.Phone2;
                            if (!string.IsNullOrEmpty(obj.Phone3)) obj.Phone1 = userDetails.Phone3;
                            if (!string.IsNullOrEmpty(obj.SourceUrl)) obj.Phone1 = userDetails.SourceUrl;
                            if (!string.IsNullOrEmpty(obj.ContactName)) obj.Phone1 = userDetails.ContactName;
                            return PartialView("AddService", obj);
                        }
                    }
                    return PartialView("AddService", userDetails);
                }
                return PartialView("AddService");
            }
            else
            {
                Content(errMsg);
            }
            return null;
        }

        /*
        public ActionResult SaveSellFlat(RealtyDomainObjects.PropertyObject po)
        {
            string buildingType = Request.Form["buildingType"] as string;
            string wcType = Request.Form["wcType"] as string;
            string currency = Request.Form["currency"] as string;
            string priceForType = Request.Form["priceForType"] as string;
            string PropertyType = Request.Form["PropertyType"] as string;
            string PropertyTypeAction = Request.Form["PropertyTypeAction"] as string;
            string PropertyTypeCities = Request.Form["PropertyTypeCities"] as string;
            string PropertyTypeCityDistrict = Request.Form["PropertyTypeCityDistrict"] as string;

            buildingType = Server.HtmlEncode(buildingType);
            wcType = Server.HtmlEncode(wcType);
            currency = Server.HtmlEncode(currency);
            priceForType = Server.HtmlEncode(priceForType);
            PropertyType = Server.HtmlEncode(PropertyType);
            PropertyTypeAction = Server.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Server.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Server.HtmlEncode(PropertyTypeCityDistrict);

            buildingType = Encoder.HtmlEncode(buildingType);
            wcType = Encoder.HtmlEncode(wcType);
            currency = Encoder.HtmlEncode(currency);
            priceForType = Encoder.HtmlEncode(priceForType);
            PropertyType = Encoder.HtmlEncode(PropertyType);
            PropertyTypeAction = Encoder.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Encoder.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Encoder.HtmlEncode(PropertyTypeCityDistrict);


            //Encoder

            int iBuildingType, iwcType, iCurrency, iPriceForType, iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict;

            if (int.TryParse(buildingType, out iBuildingType)
                && int.TryParse(wcType, out iwcType)
                && int.TryParse(currency, out iCurrency)
                && int.TryParse(priceForType, out iPriceForType)
                && int.TryParse(PropertyType, out iPropertyType)
                && int.TryParse(PropertyTypeAction, out iPropertyTypeAction)
                && int.TryParse(PropertyTypeCities, out iPropertyTypeCities)
                && int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict))
            {
                try
                {


                    RealtyDomainObjects.User user = AddNewUserIfNotExist(po);


                    var buildingTypeObj = (from s in _db.BuildingTypes
                                           where s.Id == iBuildingType
                                           select s).Single<BuildingType>();

                    var wcTypeObj = (from s in _db.WCTypes
                                     where s.Id == iwcType
                                     select s).Single<WCType>();

                    var currenyObj = (from s in _db.CurrencyTypes
                                      where s.Id == iCurrency
                                      select s).Single<CurrencyType>();

                    var priceforTypeObj = (from s in _db.PriceForTypes
                                           where s.Id == iPriceForType
                                           select s).Single<PriceForType>();

                    var propertyTypeObj = (from s in _db.PropertyTypes
                                           where s.Id == iPriceForType
                                           select s).Single<PropertyType>();

                    var PropertyTypeActionObj = (from s in _db.PropertyActions
                                                 where s.Id == iPriceForType
                                                 select s).Single<PropertyAction>();

                    var PropertyTypeCitiesObj = (from s in _db.Cities
                                                 where s.Id == iPriceForType
                                                 select s).Single<City>();

                    var PropertyTypeCityDistrictObj = (from s in _db.CityDistricts
                                                       where s.Id == iPriceForType
                                                       select s).Single<CityDisctict>();

                    po.BuildingTypeName = buildingTypeObj;
                    po.WCType = wcTypeObj;
                    po.Currency = currenyObj;
                    po.PriceForTypeName = priceforTypeObj;
                    po.PropertyType = propertyTypeObj;
                    po.PropertyAction = PropertyTypeActionObj;
                    po.City = PropertyTypeCitiesObj;
                    po.CityDistrict = PropertyTypeCityDistrictObj;
                    po.UserOwner = user;
                    po.CreatedDate = DateTime.Now;


                    _db.PropertyObjects.Add(po);
                    _db.Entry(po.BuildingTypeName).State = System.Data.EntityState.Unchanged;
                    _db.Entry(po.WCType).State = System.Data.EntityState.Unchanged;
                    _db.Entry(po.Currency).State = System.Data.EntityState.Unchanged;
                    _db.Entry(po.PriceForTypeName).State = System.Data.EntityState.Unchanged;
                    _db.Entry(po.PropertyType).State = System.Data.EntityState.Unchanged;
                    _db.Entry(po.PropertyAction).State = System.Data.EntityState.Unchanged;
                    _db.Entry(po.City).State = System.Data.EntityState.Unchanged;
                    _db.Entry(po.CityDistrict).State = System.Data.EntityState.Unchanged;
                    if (user != null)
                        _db.Entry(po.UserOwner).State = System.Data.EntityState.Unchanged;
                    _db.SaveChanges();
                    PropertyStatsHelper.CreateProperyStat(po, _db);




                }
                catch (Exception ex)//добавить систему логирования
                {
                    return Content(errMsg);
                }
            }
            else
            {
                Content(errMsg);
            }

            return RedirectToAction("GetImageUploadPreview", new { objId = po.Id });

            //return Content(string.Empty);
        }

         */ 
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SavePropertyObject(RealtyDomainObjects.PropertyObject po)
        {
            string currency = Request.Form["Currency.Id"] as string;
            string priceForType = Request.Form["PriceForTypeName.Id"] as string;
            string PropertyType = Request.Form["PropertyType"] as string;
            string PropertyTypeAction = Request.Form["PropertyTypeAction"] as string;
            string PropertyTypeCities = Request.Form["PropertyTypeCities"] as string;
            string PropertyTypeCityDistrict = Request.Form["PropertyTypeCityDistrict"] as string;
            string PropertyTypePeriods = Request.Form["Periods.Id"] as string;
            string wcType = Request.Form["WCType.Id"] as string;
            string buildingType = Request.Form["BuildingTypeName.Id"] as string;

            currency = Server.HtmlEncode(currency);
            priceForType = Server.HtmlEncode(priceForType);
            PropertyType = Server.HtmlEncode(PropertyType);
            PropertyTypeAction = Server.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Server.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Server.HtmlEncode(PropertyTypeCityDistrict);
            PropertyTypePeriods = Server.HtmlEncode(PropertyTypePeriods);
            wcType = Server.HtmlEncode(wcType);
            buildingType = Server.HtmlEncode(buildingType);

            currency = Encoder.HtmlEncode(currency);
            priceForType = Encoder.HtmlEncode(priceForType);
            PropertyType = Encoder.HtmlEncode(PropertyType);
            PropertyTypeAction = Encoder.HtmlEncode(PropertyTypeAction);
            PropertyTypeCities = Encoder.HtmlEncode(PropertyTypeCities);
            PropertyTypeCityDistrict = Encoder.HtmlEncode(PropertyTypeCityDistrict);
            PropertyTypePeriods = Encoder.HtmlEncode(PropertyTypePeriods);
            wcType = Encoder.HtmlEncode(wcType);
            buildingType = Encoder.HtmlEncode(buildingType);

            

            int iCurrency, iPriceForType, iPropertyType, iPropertyTypeAction,
                iPropertyTypeCities, iPropertyTypeCityDistrict, iPeriods, iwcType, ibuildingType;

            if (int.TryParse(currency, out iCurrency)
                && int.TryParse(PropertyType, out iPropertyType)
                && int.TryParse(PropertyTypeAction, out iPropertyTypeAction)
                && int.TryParse(PropertyTypeCities, out iPropertyTypeCities)
                && int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict))
            {
                try
                {
                    RealtyDomainObjects.User user = AddNewUserIfNotExist(po);

                    var currencyObj = (from s in _db.CurrencyTypes
                                       where s.Id == iCurrency
                                       select s).Single<CurrencyType>();

                    WCType wcTypeObj = null;

                    if (int.TryParse(wcType, out iwcType))
                    {
                        wcTypeObj = (from s in _db.WCTypes
                                     where s.Id == iwcType
                                     select s).Single<WCType>();
                    }
                    PriceForType priceforTypeObj = null;
                    if (int.TryParse(priceForType, out iPriceForType))
                    {
                        priceforTypeObj = (from s in _db.PriceForTypes
                                           where s.Id == iPriceForType
                                           select s).Single<PriceForType>();
                    }
                    Periods periods = null;
                    if (int.TryParse(PropertyTypePeriods, out iPeriods))
                    {
                        periods = (from s in _db.Periods
                                   where s.Id == iPeriods
                                   select s).Single<Periods>();
                    }

                    BuildingType buildingTypeObj = null;
                    if (int.TryParse(buildingType, out ibuildingType))
                    {
                        buildingTypeObj = (from s in _db.BuildingTypes
                                           where s.Id == ibuildingType
                                           select s).SingleOrDefault<BuildingType>();
                    }

                    var propertyTypeObj = (from s in _db.PropertyTypes
                                           where s.Id == iPropertyType
                                           select s).SingleOrDefault<PropertyType>();

                    var PropertyTypeActionObj = (from s in _db.PropertyActions
                                                 where s.Id == iPropertyTypeAction
                                                 select s).SingleOrDefault<PropertyAction>();

                    var PropertyTypeCitiesObj = (from s in _db.Cities
                                                 where s.Id == iPropertyTypeCities
                                                 select s).SingleOrDefault<City>();

                    var PropertyTypeCityDistrictObj = (from s in _db.CityDistricts
                                                       where s.Id == iPropertyTypeCityDistrict
                                                       select s).SingleOrDefault<CityDisctict>();

                    CommercialPropertyType commercialPropertyType = null;
                    if (po.CommercialPropertyType != null)
                    {
                        commercialPropertyType = (from s in _db.CommercialPropertyTypes
                                                  where s.Id == po.CommercialPropertyType.Id
                                                  select s).Single<CommercialPropertyType>();
                    }
                    ServiceType serviceType = null;
                    if (po.ServiceType != null)
                    {
                        serviceType = (from s in _db.ServiceTypes
                                       where s.Id == po.ServiceType.Id
                                       select s).Single<ServiceType>();
                    }

                    if (po.Id > 0)
                    {

                        po.IsActive = true;
                        po.Currency = currencyObj;
                        po.PriceForTypeName = priceforTypeObj;
                        po.PropertyType = propertyTypeObj;
                        po.PropertyAction = PropertyTypeActionObj;
                        po.City = PropertyTypeCitiesObj;
                        po.CityDistrict = PropertyTypeCityDistrictObj;
                        po.Periods = periods;
                        po.UserOwner = user;
                        po.CreatedDate = DateTime.Now;
                        po.WCType = wcTypeObj;
                        po.BuildingTypeName = buildingTypeObj;
                        po.ServiceType = serviceType;


                        var existingObj = (from s in _db.PropertyObjects
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
                                           where s.Id == po.Id
                                           select s).SingleOrDefault<RealtyDomainObjects.PropertyObject>();






                        existingObj.Currency = po.Currency;
                        existingObj.PriceForTypeName = po.PriceForTypeName;
                        existingObj.PropertyType = po.PropertyType;
                        existingObj.PropertyAction = po.PropertyAction;
                        existingObj.City = po.City;
                        existingObj.CityDistrict = po.CityDistrict;
                        existingObj.Periods = po.Periods;
                        existingObj.UserOwner = po.UserOwner;
                        existingObj.CreatedDate = po.CreatedDate;
                        existingObj.WCType = po.WCType;
                        existingObj.BuildingTypeName = po.BuildingTypeName;
                        existingObj.CommercialPropertyType = commercialPropertyType;
                        existingObj.ServiceType = serviceType;

                        existingObj.Title = po.Title;
                        existingObj.PropertyDescription = po.PropertyDescription;
                        existingObj.RoomCount = po.RoomCount;
                        existingObj.TotalSpace = po.TotalSpace;
                        existingObj.LivingSpace = po.LivingSpace;
                        existingObj.KitchenSpace = po.KitchenSpace;
                        existingObj.Floor = po.Floor;
                        existingObj.CountFloors = po.CountFloors;
                        existingObj.IsNewBuilding = po.IsNewBuilding;
                        existingObj.BalconAvailable = po.BalconAvailable;
                        existingObj.BalconSpace = po.BalconSpace;
                        existingObj.isBalconGlassed = po.isBalconGlassed;
                        existingObj.ContactName = po.ContactName;
                        existingObj.Price = po.Price;
                        existingObj.NoCommission = po.NoCommission;
                        existingObj.Phone1 = po.Phone1;
                        existingObj.Phone2 = po.Phone2;
                        existingObj.Phone3 = po.Phone3;
                        existingObj.SourceUrl = po.SourceUrl;
                        existingObj.DistanceToCity = po.DistanceToCity;
                        existingObj.CountPhotos = po.CountPhotos;
                        existingObj.IsDeleted = po.IsDeleted;
                        existingObj.DeletedDate = po.DeletedDate;

                        //_db.PropertyObjects.Attach(existingObj);
                        _db.Entry(existingObj).State = System.Data.EntityState.Modified;



                    }
                    else
                    {
                        po.IsActive = true;
                        po.Currency = currencyObj;
                        po.PriceForTypeName = priceforTypeObj;
                        po.PropertyType = propertyTypeObj;
                        po.PropertyAction = PropertyTypeActionObj;
                        po.City = PropertyTypeCitiesObj;
                        po.CityDistrict = PropertyTypeCityDistrictObj;
                        po.Periods = periods;
                        po.UserOwner = user;
                        po.CreatedDate = DateTime.Now;
                        po.WCType = wcTypeObj;
                        po.BuildingTypeName = buildingTypeObj;
                        po.CommercialPropertyType = commercialPropertyType;
                        po.ServiceType = serviceType;


                        _db.PropertyObjects.Add(po);


                        if (po.ServiceType != null)
                            _db.Entry(po.ServiceType).State = System.Data.EntityState.Unchanged;

                        if (po.CommercialPropertyType != null)
                            _db.Entry(po.CommercialPropertyType).State = System.Data.EntityState.Unchanged;

                        if (po.BuildingTypeName != null)
                            _db.Entry(po.BuildingTypeName).State = System.Data.EntityState.Unchanged;

                        if (po.WCType != null)
                            _db.Entry(po.WCType).State = System.Data.EntityState.Unchanged;

                        if (po.BuildingTypeName != null)
                            _db.Entry(po.BuildingTypeName).State = System.Data.EntityState.Unchanged;

                        if (po.Periods != null)
                            _db.Entry(po.Periods).State = System.Data.EntityState.Unchanged;

                        if (po.WCType != null)
                            _db.Entry(po.WCType).State = System.Data.EntityState.Unchanged;

                        if (user != null)
                            _db.Entry(po.UserOwner).State = System.Data.EntityState.Unchanged;

                        _db.Entry(po.Currency).State = System.Data.EntityState.Unchanged;

                        if (po.PriceForTypeName != null)
                            _db.Entry(po.PriceForTypeName).State = System.Data.EntityState.Unchanged;

                        if (po.PropertyType != null)
                            _db.Entry(po.PropertyType).State = System.Data.EntityState.Unchanged;

                        if (po.PropertyAction != null)
                            _db.Entry(po.PropertyAction).State = System.Data.EntityState.Unchanged;

                        if (po.City != null)
                            _db.Entry(po.City).State = System.Data.EntityState.Unchanged;

                        if (po.CityDistrict != null)
                            _db.Entry(po.CityDistrict).State = System.Data.EntityState.Unchanged;
                    }
                    _db.SaveChanges();
                    AddLandSpecifics(po);

                    //PropertyStatsHelper.CreateProperyStat(po, _db);

                }
                catch (Exception ex)
                {
                    return Content(errMsg + ": " + ex.Message);
                }
            }
            else
            {
                Content(errMsg);
            }

            return RedirectToAction("GetImageUploadPreview", new { objId = po.Id });
        }

        private void AddLandSpecifics(PropertyObject po)
        {
            #region landCommunications

            // var _dbForInsert = new RealtyDb();
            var LandCommunicationToObjForAdding = new List<LandCommunicationToObj>();
            var landCommunications = from s in _db.LandCommunications
                                     select s;
            bool isChecked = false;
            foreach (var item in landCommunications)
            {
                if (Request.Form[item.guid.ToString()] != null &&
                    Request.Form[item.guid.ToString()].Contains(',') &&
                    Request.Form[item.guid.ToString()].Split(',').Count() == 2 &&
                    bool.TryParse(Request.Form[item.guid.ToString()].Split(',')[0].ToString(), out isChecked))
                {
                    if (isChecked)
                    {
                        isChecked = false;
                        var landCommunicationToObj = new LandCommunicationToObj()
                        {
                            LandCommunicationId = item,
                            PropertyObjectId = po
                        };
                        LandCommunicationToObjForAdding.Add(landCommunicationToObj);

                    }
                }
            }

            foreach (var item in LandCommunicationToObjForAdding)
            {
                if (_db.Entry(item).State == System.Data.EntityState.Detached)
                    _db.LandCommunicationToObjs.Attach(item);

                _db.LandCommunicationToObjs.Add(item);
                _db.Entry(item.PropertyObjectId).State = System.Data.EntityState.Unchanged;
                _db.Entry(item.LandCommunicationId).State = System.Data.EntityState.Unchanged;
                _db.SaveChanges();
            }
            #endregion

            #region landFunctions routine
            isChecked = false;
            var LandFunctionToObjForAdding = new List<LandFunctionToObj>();
            var landFunctions = from s in _db.LandFunctions
                                select s;

            foreach (var item in landFunctions)
            {
                if (Request.Form[item.guid.ToString()] != null &&
                    Request.Form[item.guid.ToString()].Contains(',') &&
                    Request.Form[item.guid.ToString()].Split(',').Count() == 2 &&
                    bool.TryParse(Request.Form[item.guid.ToString()].Split(',')[0].ToString(), out isChecked))
                {
                    if (isChecked)
                    {
                        isChecked = false;
                        var landFunctionToObj = new LandFunctionToObj()
                        {
                            LandFunctionId = item,
                            PropertyObjectId = po
                        };
                        LandFunctionToObjForAdding.Add(landFunctionToObj);
                        //_dbForInsert.LandFunctionToObjs.Add(landFunctionToObj);
                        //_dbForInsert.Entry(landFunctionToObj.PropertyObjectId).State = System.Data.EntityState.Unchanged;
                        //_dbForInsert.Entry(landFunctionToObj.LandFunctionId).State = System.Data.EntityState.Unchanged;
                        //_dbForInsert.SaveChanges();
                    }
                }
            }
            foreach (var item in LandFunctionToObjForAdding)
            {

                if (_db.Entry(item).State == System.Data.EntityState.Detached)
                    _db.LandFunctionToObjs.Attach(item);

                _db.LandFunctionToObjs.Add(item);
                _db.Entry(item.PropertyObjectId).State = System.Data.EntityState.Unchanged;
                _db.Entry(item.LandFunctionId).State = System.Data.EntityState.Unchanged;
                _db.SaveChanges();
            }
            #endregion


        }

        public ActionResult PreviewAndAddImages(int propertyObjId)
        {
            var obj = (from s in _db.PropertyObjects
                       where s.Id == propertyObjId
                       select s).SingleOrDefault<PropertyObject>();
            return View(obj);
        }

        // This action handles the form POST and the upload
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UploadImage(int objId, HttpPostedFileBase file = null)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                // var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                // var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                // file.SaveAs(path);


                var propObj = (from s in _db.PropertyObjects
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
                               where s.Id == objId
                               select s).SingleOrDefault<PropertyObject>();

                ImageProcessing imgHelper = new ImageProcessing();
                using (System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream))
                {
                    //Initialise the size of the array
                    byte[] f = new byte[file.InputStream.Length];
                    //Create a new BinaryReader and set the InputStream  for the Images InputStream to the beginning, as we create the img using a stream.
                    var reader = new BinaryReader(file.InputStream);
                    file.InputStream.Seek(0, SeekOrigin.Begin);
                    //Load the image binary.
                    f = reader.ReadBytes((int)file.InputStream.Length);
                    //Create a new image to be added to the database
                    if (img.Width > 512)
                        f = imgHelper.ResizeImage(f, new System.Drawing.Size(512, 389));
                    ObjectImages objImages = new ObjectImages()
                    {
                        Content_Type = file.ContentType,
                        FileName = file.FileName,
                        FileSize = file.ContentLength,
                        Height = (img.Width > 512 ? 512 : img.Height),
                        Width = (img.Width > 512 ? 512 : img.Width),
                        Image = f,
                        ImagePreview = imgHelper.ResizeImage(f, new System.Drawing.Size(120, 90)),
                        PropertyObject = propObj,
                        IsDeleted = false
                    };

                    _db.ObjectImages.Add(objImages);
                    _db.Entry(objImages.PropertyObject).State = System.Data.EntityState.Unchanged;
                    _db.SaveChanges();
                }
                var countImages = (from s in _db.ObjectImages
                                   where s.PropertyObject.Id == propObj.Id
                                   && s.IsDeleted == false
                                   select 1).Count<int>();

                propObj.CountPhotos = countImages;
                _db.PropertyObjects.Attach(propObj);
                _db.Entry(propObj).Property(x => x.CountPhotos).IsModified = true;
                _db.SaveChanges();
            }
            PropertyPreview result = new PropertyPreview();

            var smallImageIds = from s in _db.ObjectImages.AsQueryable()
                                where s.PropertyObject.Id == objId
                                && s.IsDeleted == false
                                select s.Id;

            foreach (var item in smallImageIds)
            {
                if (result.ImgPreviewIds == null) result.ImgPreviewIds = new List<int>();
                result.ImgPreviewIds.Add(item);
            }

            result.PropertObject = (from s in _db.PropertyObjects
                                    where s.Id == objId
                                    select s).Single<PropertyObject>();

            return RedirectToAction("GetImageUploadPreview", new { objId = objId });
            //ViewBag.ObjId = objId;
            //return View(result);


            // redirect back to the index action to show the form once again
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteImage(int imgId, int objId)
        {
            var imgObj = (from s in _db.ObjectImages.Include("PropertyObject")
                          where s.Id == imgId
                          select s).SingleOrDefault<ObjectImages>();

            imgObj.IsDeleted = true;
            _db.Entry(imgObj.PropertyObject).State = System.Data.EntityState.Unchanged;
            _db.SaveChanges();

            #region update count photos for specific PropertyObject
            var propObj = (from s in _db.PropertyObjects
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
                           where s.Id == objId
                           select s).SingleOrDefault<PropertyObject>();

            var countImages = (from s in _db.ObjectImages
                               where s.PropertyObject.Id == propObj.Id
                               && s.IsDeleted == false
                               select 1).Count<int>();

            propObj.CountPhotos = countImages;
            _db.PropertyObjects.Attach(propObj);
            _db.Entry(propObj).Property(x => x.CountPhotos).IsModified = true;
            _db.SaveChanges();
            #endregion

            return RedirectToAction("GetImageUploadPreview", new { objId = objId });

        }

        public ActionResult GetImageUploadPreview(string objId)
        {
            int iObjId;
            if (!int.TryParse(objId, out iObjId))
                return RedirectToAction("index", "home");
            PropertyPreview result = new PropertyPreview();

            var smallImageIds = from s in _db.ObjectImages.AsQueryable()
                                where s.PropertyObject.Id == iObjId
                                && s.IsDeleted == false
                                select s.Id;

            foreach (var item in smallImageIds)
            {
                if (result.ImgPreviewIds == null) result.ImgPreviewIds = new List<int>();
                result.ImgPreviewIds.Add(item);
            }

            result.PropertObject = (from s in _db.PropertyObjects
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
                                    where s.Id == iObjId
                                    select s).SingleOrDefault<PropertyObject>();
            if (result == null || result.PropertObject==null)
                return RedirectToAction("index", "home");

            if (result.PropertObject != null && result.PropertObject.PropertyType.Id == 3)
            {
                var landCommunication = (from s in _db.LandCommunicationToObjs
                                         .Include("LandCommunicationId")
                                         where s.PropertyObjectId.Id==iObjId
                                         select s.LandCommunicationId).ToList<LandCommunication>();
                ViewBag.landCommunication = landCommunication;

                var landFunction = (from s in _db.LandFunctionToObjs
                                    .Include("LandFunctionId")
                                    where s.PropertyObjectId.Id == iObjId
                                    select s.LandFunctionId).ToList<LandFunction>();
                ViewBag.landFunction = landFunction;
            }
            return View(result);
        }

        public ActionResult PreviewAd(string objId)
        {
            return RedirectToAction("ObjectDetails", "SearchResult", new { Id = objId, IsJustAdded = true });
        }

        public FileContentResult getImg(int id)
        {
            byte[] byteArray = (from s in _db.ObjectImages.AsQueryable()
                                where s.Id == id
                                select s.ImagePreview).SingleOrDefault<byte[]>();
            if (byteArray != null)
            {
                return new FileContentResult(byteArray, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        public FileContentResult getOriginalImg(int id)
        {
            byte[] byteArray = (from s in _db.ObjectImages.AsQueryable()
                                where s.Id == id
                                select s.Image).SingleOrDefault<byte[]>();
            if (byteArray != null)
            {
                return new FileContentResult(byteArray, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        private RealtyDomainObjects.User AddNewUserIfNotExist(RealtyDomainObjects.PropertyObject po)
        {
            int membershipId = -1;
            RealtyDomainObjects.User user = null;
            if (Auth.IsAuthenticated(Request, User, out membershipId))
            {
                user = (from s in _db.Users
                        where s.MembershipId == membershipId
                        select s).SingleOrDefault<RealtyDomainObjects.User>();
                if (user == null)
                {
                    user = new RealtyDomainObjects.User()
                    {
                        MembershipId = membershipId,
                        Phone1 = po.Phone1,
                        Phone2 = po.Phone2,
                        Phone3 = po.Phone3,
                        url = po.SourceUrl,
                        UserName = po.ContactName
                    };
                    _db.Users.Add(user);
                    _db.SaveChanges();
                }
            }
            return user;

        }

        private PropertyObject GetPropertyObject(string id)
        {
            int iID = -1;
            if (!int.TryParse(id, out iID))
                return null;

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

                       where s.Id == iID
                       select s).SingleOrDefault<PropertyObject>();
            return obj;
        }
    }
}
