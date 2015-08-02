using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRealty.Models;
using WebRealty.Common;
using RealtyDomainObjects;
using WebRealty.DataLayer;

namespace WebRealty.Controllers
{
    public class SearchBoxController : Controller
    {
        //
        // GET: /SearchBox/

        RealtyDb _db = new RealtyDb();

        [OutputCache(Duration = 500)]
        public ActionResult Index()
        {
            var model = _db.PropertyTypes;
            return View(model);
        }

        [OutputCache(Duration = 500)]
        public PartialViewResult PartiallyIndex()
        {
            //var model = _db.PropertyTypes;
            var model = from s in _db.PropertyTypes
                        select new { id = s.Id, name = s.PropertyTypeName };
            var sl = new SelectList(model, "id", "name", 8);
            ViewBag.PropertyTypes = sl;
            return PartialView("_SearchBox");
        }


        [OutputCache(Duration = 60, VaryByParam = "ID")]
        public ActionResult PropertyAction(int ID)
        {
            var propertyActions = from s in _db.PropertyActions.AsQueryable()
                                  where s.PropertyType.Id == ID
                                  select new { id = s.Id, value = s.PropertyActionName };

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                propertyActions.ToArray(),
                                "id",
                                "value")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");
        }

        [OutputCache(Duration = 60, VaryByParam = "ID")]
        public ActionResult PropertyCities(int ID)
        {
            if (ID == -1) return Json(new SelectList(Enumerable.Empty<Object>().ToArray())
                           , JsonRequestBehavior.AllowGet);

            var propertyCities = from s in _db.Cities.AsQueryable()
                                 //where s..Id == ID
                                 select new { id = s.Id, value = s.CityName };

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                propertyCities.ToArray(),
                                "id",
                                "value")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");
        }


        [OutputCache(Duration = 60, VaryByParam = "ID")]
        public ActionResult PropertyCityDistricts(int ID)
        {
            if (ID == -1) return Json(new SelectList(Enumerable.Empty<Object>().ToArray())
               , JsonRequestBehavior.AllowGet);

            var propertyCityDistricts = from s in _db.CityDistricts.AsQueryable()
                                        where s.City.Id == ID
                                        select new { id = s.Id, value = s.District };

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                propertyCityDistricts.ToArray(),
                                "id",
                                "value")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");
        }

        /*
        [HttpPost]
        public ActionResult SearchByParams(FormCollection values)
        {
            int PropertyType, PropertyTypeAction, PropertyTypeCities, PropertyTypeCityDistrict;
            PropertyType = PropertyTypeAction = PropertyTypeCities = PropertyTypeCityDistrict = -1;

            #region validate inputParams
            if (values["PropertyType"] != null)
                if (!int.TryParse(values["PropertyType"], out PropertyType)) PropertyType = -1;
            if (values["PropertyTypeAction"] != null)
                if (!int.TryParse(values["PropertyTypeAction"], out PropertyTypeAction)) PropertyTypeAction = -1;
            if (values["PropertyTypeCities"] != null)
                if (!int.TryParse(values["PropertyTypeCities"], out PropertyTypeCities)) PropertyTypeCities = -1;
            if (values["PropertyTypeCityDistrict"] != null)
                if (!int.TryParse(values["PropertyTypeCityDistrict"], out PropertyTypeCityDistrict)) PropertyTypeCityDistrict = -1;
            #endregion
            if (PropertyType == -1) return View("SeachBox");

            if (PropertyType > -1 && PropertyTypeAction > -1 && PropertyTypeCities > -1 && PropertyTypeCityDistrict > -1)
            {
                var searchByParamsResult = from s in _db.PropertyObjects
                                           where s.PropertyType.Id == PropertyType
                                           && s.PropertyAction.Id == PropertyTypeAction
                                           && s.City.Id == PropertyTypeCities
                                           && s.CityDistrict.Id == PropertyTypeCityDistrict
                                           select s;

                return View(searchByParamsResult);
            }
            else if (PropertyType > -1 && PropertyTypeAction > -1 && PropertyTypeCities > -1 && PropertyTypeCityDistrict == -1)
            {
                var searchByParamsResult = from s in _db.PropertyObjects
                                           where s.PropertyType.Id == PropertyType
                                           && s.PropertyAction.Id == PropertyTypeAction
                                           && s.City.Id == PropertyTypeCities
                                           select s;

                return View(searchByParamsResult);
            }
            else if (PropertyType > -1 && PropertyTypeAction > -1 && PropertyTypeCities == -1 && PropertyTypeCityDistrict == -1)
            {
                var searchByParamsResult = from s in _db.PropertyObjects
                                           where s.PropertyType.Id == PropertyType
                                           && s.PropertyAction.Id == PropertyTypeAction
                                           select s;

                return View(searchByParamsResult);
            }
            else if (PropertyType > -1 && PropertyTypeAction == -1 && PropertyTypeCities == -1 && PropertyTypeCityDistrict == -1)
            {
                var searchByParamsResult = from s in _db.PropertyObjects
                                           where s.PropertyType.Id == PropertyType
                                           select s;

                return View(searchByParamsResult);
            }
            else
                return View("SearchBox");
        }
        */




        [HttpGet]
        public ActionResult SearchByParams()
        {
            string PropertyType = (Request.QueryString["PropertyType"]);
            string PropertyTypeAction = Request.QueryString["PropertyTypeAction"];
            string PropertyTypeCities = Request.QueryString["PropertyTypeCities"];
            string PropertyTypeCityDistrict = Request.QueryString["PropertyTypeCityDistrict"];
            string NavigatePage = Request.QueryString["NavigatePage"];
            string SortTypeDll = Request.QueryString["SortType"];
            string ShowType = Request.QueryString["ShowType"];//witch  type of layout to chose
            string PriceMin = Request.QueryString["pricemin"];
            string PriceMax = Request.QueryString["pricemax"];
            string Currency = Request.QueryString["currency"];

            string commercialPropertyType = Request.QueryString["commercialPropertyType"];
            string countFloors = Request.QueryString["countFloors"];
            string distToCityMin = Request.QueryString["distToCityMin"];
            string distToCityMax = Request.QueryString["distToCityMax "];

            string buildingType = Request.QueryString["buildingType"];
            string wcType = Request.QueryString["wcType"];
            string roomCount = Request.QueryString["roomCount"];
            string sizemin = Request.QueryString["sizemin"];
            string sizemax = Request.QueryString["sizemax"];


            int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict,
                iNavigatePage, iSortTypeDll, iShowType, iPriceMin, iPriceMax, iCurrency,
                iBuildingType, iWcType, iRoomCount, iSizemin, iSizemax,
                iCommercialPropertyType, iCountFloors, iDistToCityMin, iDistToCityMax;

            iPropertyTypeAction = iPropertyTypeCities = iPropertyTypeCityDistrict = iShowType = iPriceMin = iPriceMax = iCurrency
                = iBuildingType = iWcType = iRoomCount = iSizemin = iSizemax =
                iCommercialPropertyType = iCountFloors = iDistToCityMin = iDistToCityMax = -1;

            iNavigatePage = iSortTypeDll = iPropertyType = 1;

            #region validate inputParams
            if (!string.IsNullOrEmpty(PropertyType))
                if (!int.TryParse(PropertyType, out iPropertyType)) iPropertyType = -1;

            if (!string.IsNullOrEmpty(PropertyTypeAction))
                if (!int.TryParse(PropertyTypeAction, out iPropertyTypeAction)) iPropertyTypeAction = -1;

            if (!string.IsNullOrEmpty(PropertyTypeCities))
                if (!int.TryParse(PropertyTypeCities, out iPropertyTypeCities)) iPropertyTypeCities = -1;

            if (!string.IsNullOrEmpty(PropertyTypeCityDistrict))
                if (!int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict)) iPropertyTypeCityDistrict = -1;

            if (!string.IsNullOrEmpty(NavigatePage))
                if (!int.TryParse(NavigatePage, out iNavigatePage)) iNavigatePage = 1;

            if (!string.IsNullOrEmpty(SortTypeDll))
                if (!int.TryParse(SortTypeDll, out iSortTypeDll)) iSortTypeDll = 1;

            if (!string.IsNullOrEmpty(ShowType))
                if (!int.TryParse(ShowType, out iShowType)) iShowType = -1;

            if (!string.IsNullOrEmpty(PriceMin))
                if (!int.TryParse(PriceMin, out iPriceMin)) iPriceMin = -1;

            if (!string.IsNullOrEmpty(PriceMax))
                if (!int.TryParse(PriceMax, out iPriceMax)) iPriceMax = -1;

            if (!string.IsNullOrEmpty(Currency))
                if (!int.TryParse(Currency, out iCurrency)) iCurrency = -1;

            if (!string.IsNullOrEmpty(buildingType))
                if (!int.TryParse(buildingType, out iBuildingType)) iBuildingType = -1;

            if (!string.IsNullOrEmpty(wcType))
                if (!int.TryParse(wcType, out iWcType)) iWcType = -1;

            if (!string.IsNullOrEmpty(roomCount))
                if (!int.TryParse(roomCount, out iRoomCount)) iRoomCount = -1;

            if (!string.IsNullOrEmpty(sizemin))
                if (!int.TryParse(sizemin, out iSizemin)) iSizemin = -1;

            if (!string.IsNullOrEmpty(sizemax))
                if (!int.TryParse(sizemax, out iSizemax)) iSizemax = -1;

            if (!string.IsNullOrEmpty(commercialPropertyType))
                if (!int.TryParse(commercialPropertyType, out iCommercialPropertyType)) iCommercialPropertyType = -1;

            if (!string.IsNullOrEmpty(countFloors))
                if (!int.TryParse(countFloors, out iCountFloors)) iCountFloors = -1;

            if (!string.IsNullOrEmpty(distToCityMin))
                if (!int.TryParse(distToCityMin, out iDistToCityMin)) iDistToCityMin = -1;

            if (!string.IsNullOrEmpty(distToCityMax))
                if (!int.TryParse(distToCityMax, out iDistToCityMax)) iDistToCityMax = -1;


            #endregion


            int _pageSize = WebRealty.Common.PagingHelper.GetPageSize();

            var result = from s in _db.PropertyObjects
                         .Include("Currency")
                         where s.IsDeleted == 0
                         select s;
            if (iPropertyType != -1)
                result = result.Where(q => q.PropertyType.Id == iPropertyType);
            if (iPropertyTypeAction != -1)
                result = result.Where(q => q.PropertyAction.Id == iPropertyTypeAction);
            if (iPropertyTypeCities != -1)
                result = result.Where(q => q.City.Id == iPropertyTypeCities);
            if (iPropertyTypeCityDistrict != -1)
                result = result.Where(q => q.CityDistrict.Id == iPropertyTypeCityDistrict);
            if (iPriceMin != -1 && iCurrency != -1)
                result = result.Where(q => q.Price >= iPriceMin && q.Currency.Id == iCurrency);
            if (iPriceMax != -1 && iCurrency != -1)
                result = result.Where(q => q.Price <= iPriceMax && q.Currency.Id == iCurrency);

            if (iBuildingType != -1)
                result = result.Where(q => q.BuildingTypeName.Id == iBuildingType);
            if (iWcType != -1)
                result = result.Where(q => q.WCType.Id == iWcType);
            if (iRoomCount != -1)
                if (roomCount == "более")//если  пришло  "более" значит  ищем  количество комнат больше  4
                    result = result.Where(q => q.RoomCount > 4);//4- ВАЖНО
                else
                    result = result.Where(q => q.RoomCount == iRoomCount);
            if (iSizemin != -1)
                result = result.Where(q => q.TotalSpace >= iSizemin);
            if (iSizemax != -1)
                result = result.Where(q => q.TotalSpace <= iSizemax);

            if (iCommercialPropertyType != -1)
                result = result.Where(q => q.CommercialPropertyType.Id == iCommercialPropertyType);

            if (iCountFloors != -1)
                result = result.Where(q => q.CountFloors == iCountFloors);

            if (iDistToCityMin != -1)
                result = result.Where(q => q.DistanceToCity >= iDistToCityMin);

            if (iDistToCityMax != -1)
                result = result.Where(q => q.DistanceToCity <= iDistToCityMax);


            if (iSortTypeDll == 1)
                result = result.OrderByDescending(t => t.CreatedDate).
                                  Skip(iNavigatePage == 1 ? 0 : (iNavigatePage - 1) * _pageSize)
                                  .Take(_pageSize).OrderByDescending(t => t.CreatedDate);
            else if (iSortTypeDll == 2)
                result = result.OrderBy(t => t.CreatedDate).
                                  Skip(iNavigatePage == 1 ? 0 : (iNavigatePage - 1) * _pageSize)
                                  .Take(_pageSize).OrderBy(t => t.CreatedDate);
            else if (iSortTypeDll == 3)
                result = result.OrderBy(t => t.Price).
                                  Skip(iNavigatePage == 1 ? 0 : (iNavigatePage - 1) * _pageSize)
                                  .Take(_pageSize).OrderBy(t => t.Price);
            else if (iSortTypeDll == 4)
                result = result.OrderByDescending(t => t.Price).
                                  Skip(iNavigatePage == 1 ? 0 : (iNavigatePage - 1) * _pageSize)
                                  .Take(_pageSize).OrderByDescending(t => t.Price);



            if (iShowType == -1)
            {
                return View("SearchByParams", result);
            }
            else
            {
                return View("SearchByParamsList", result);

            }

        }


        public PartialViewResult PopulateSearchBox(string PropertyType, string PropertyTypeAction, string PropertyTypeCities, string PropertyTypeCityDistrict)
        {
            int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict;
            iPropertyType = iPropertyTypeAction = iPropertyTypeCities = iPropertyTypeCityDistrict = -1;

            int.TryParse(PropertyType, out iPropertyType);

            var model = new SearchObjectParams();

            model.PropertyType = _db.PropertyTypes;

            #region populate PropertyTypeAction
            if (int.TryParse(PropertyTypeAction, out iPropertyTypeAction) && iPropertyType > 0)
            {
                var propertyActions = (from s in _db.PropertyActions.AsQueryable()
                                       where 1 == 1
                                       select new Pair<int, string>() { Key = -1, Value = "Выберите..." }).Distinct()
                    .Union((from s in _db.PropertyActions.AsQueryable()
                            where s.PropertyType.Id == iPropertyType
                            select new Pair<int, string>() { Key = s.Id, Value = s.PropertyActionName })
                                      );

                model.PropertyAction = new SelectList(propertyActions.ToArray(), "key", "value", iPropertyTypeAction);

            }
            else
            {
                model.PropertyAction = null;
            }
            #endregion

            #region  populate city and cityDistrict
            if (int.TryParse(PropertyTypeCities, out iPropertyTypeCities) && iPropertyTypeAction > 0)
            {
                var cities = (from s in _db.Cities.AsQueryable()
                              select new Pair<int, string>() { Key = -1, Value = "Выберите..." }).Distinct()
                         .Union(
                                from s in _db.Cities.AsQueryable()
                                select new Pair<int, string> { Key = s.Id, Value = s.CityName }
                             );

                model.City = new SelectList(cities.ToArray(), "Key", "Value", iPropertyTypeCities);

                var disctricts = (from s in _db.CityDistricts.AsQueryable()
                                  where s.City.Id == iPropertyTypeCities
                                  select new Pair<int, string>() { Key = -1, Value = "Выберите..." }).Distinct()
                         .Union(
                                from s in _db.CityDistricts.AsQueryable()
                                where s.City.Id == iPropertyTypeCities
                                select new Pair<int, string> { Key = s.Id, Value = s.District }
                                 );

                if (int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict) && iPropertyTypeCities > 0)
                {
                    model.CityDisctict = new SelectList(disctricts.ToArray(), "Key", "Value", iPropertyTypeCityDistrict);
                }
                else
                {
                    model.CityDisctict = new SelectList(disctricts.ToArray(), "Key", "Value");
                }

            }
            else
            {
                model.City = null;
                model.CityDisctict = null;

            }


            #endregion

            return PartialView("_PopulateSearchBox", model);
        }


        [OutputCache(Duration = 3600)]
        public PartialViewResult QuickSearchBox()
        {

            var propertyAction = (from s in _db.PropertyActions.Include("PropertyType")
                                  where s.PropertyType.important == true
                                  select s).OrderBy(t => t.PropertyType.PropertyTypeName).ToList<RealtyDomainObjects.PropertyAction>();

            var city = (from s in _db.Cities
                        where s.important == true
                        select s).ToList<RealtyDomainObjects.City>();
            var model = new QuickSearchModel()
            {
                City = city,
                PropertyAction = propertyAction
            };
            return PartialView(model);
        }


        //TODO добавить кеширование к данному методу
        public PartialViewResult QuickTabSearch()
        {
            SelectList sl = null;
            var model = new QuickTabSearchModel();

            var propertyTypes = (from s in _db.PropertyTypes
                                 select s.PropertyTypeName).ToList<string>();

            foreach (var item in propertyTypes)
            {
                int? key = (from s in _db.PropertyTypes
                            where s.PropertyTypeName == item
                            select s.Id).SingleOrDefault<int>();
                if (key != null && key > 0)
                {
                    var propertyActions = from s in _db.PropertyActions.AsQueryable()
                                          where s.PropertyType.Id == key
                                          select new { id = s.Id, value = s.PropertyActionName };
                    sl = new SelectList(propertyActions, "id", "value");

                    if (item == PropertyTypeEnum.flat.GetStringValue())
                        model.flatPropertyActions = sl;
                    else if (item == PropertyTypeEnum.commercialProperty.GetStringValue())
                        model.commercialPropertyAction = sl;
                    else if (item == PropertyTypeEnum.foreignProperty.GetStringValue())
                        model.foreignPropertyAction = sl;
                    else if (item == PropertyTypeEnum.houses.GetStringValue())
                        model.housesPropertyAction = sl;
                    else if (item == PropertyTypeEnum.land.GetStringValue())
                        model.landPropertyAction = sl;
                    else if (item == PropertyTypeEnum.parkingGarage.GetStringValue())
                        model.parkingGaragePropertyAction = sl;
                    else if (item == PropertyTypeEnum.services.GetStringValue())
                        model.servicesPropertyAction = sl;
                }
            }

            var cities = from s in _db.Cities
                         select new { id = s.Id, value = s.CityName };
            sl = new SelectList(cities, "id", "value");
            model.cities = sl;

            return PartialView(model);
        }

        public PartialViewResult AdvancedSearchBox()
        {
            SelectList sl = null;
            var model = new QuickTabSearchModel();
            var propertyTypes = from s in _db.PropertyTypes
                                where s.Id != 8
                                select new { id = s.Id, value = s.PropertyTypeName };
            sl = new SelectList(propertyTypes, "id", "value");
            model.propertyType = sl;

            var cities = from s in _db.Cities
                         select new { id = s.Id, value = s.CityName };
            sl = new SelectList(cities, "id", "value");
            model.cities = sl;

            var currecy = from s in _db.CurrencyTypes
                          select new { id = s.Id, value = s.CurrencyTypeName };
            sl = new SelectList(currecy, "id", "value");
            model.currecy = sl;

            return PartialView(model);

        }


        public PartialViewResult AdvancedSearchFlatPart()
        {
            SelectList sl = null;
            var buildingType = from s in _db.BuildingTypes.AsQueryable()
                               select new { id = s.Id, value = s.BuildingTypeName };
            sl = new SelectList(buildingType, "id", "value");
            ViewBag.buildingType = sl;

            var wcType = from s in _db.WCTypes.AsQueryable()
                         select new { id = s.Id, value = s.WCTypeName };
            sl = new SelectList(wcType, "id", "value");
            ViewBag.wcType = sl;

            int tmp = 0;
            var roomCount = from s in "1 2 3 4 более".Split(' ')
                            select new { id = (++tmp), value = s };
            sl = new SelectList(roomCount, "id", "value");
            ViewBag.roomCount = sl;

            return PartialView();
        }

        public PartialViewResult AdvancedSearchHousePart()
        {
            SelectList sl = null;

            int tmp = 0;
            var countFloors = from s in "1 2 3 более".Split(' ')
                              select new { id = (++tmp), value = s };
            sl = new SelectList(countFloors, "id", "value");
            ViewBag.countFloors = sl;

            return PartialView();
        }

        public PartialViewResult AdvancedSearchLandPart()
        {
            var landCommunication = (from s in _db.LandCommunications
                                     select s).ToList<LandCommunication>();
            ViewBag.landCommunication = landCommunication;

            var landFunction = (from s in _db.LandFunctions
                                select s).ToList<LandFunction>();
            ViewBag.landFunction = landFunction;

            return PartialView();
        }

        public PartialViewResult AdvancedSearchCommercialPropertyPart()
        {
            SelectList sl = null;
            var commercialPropertyType = from s in _db.CommercialPropertyTypes
                                         select new { id = s.Id, name = s.CommercialPropertyTypeName };
            sl = new SelectList(commercialPropertyType, "id", "name");
            ViewBag.commercialPropertyType = sl;

            return PartialView();
        }


        public PartialViewResult SearchOnMap()
        {
            var data = new PropertyTypeRoutines(this._db).GetPropertyTypes().ToList<PropertyType>();
            var model = new SelectList(data, "id", "PropertyTypeName", 8);
            return PartialView(model);
        }

       
    }
}
