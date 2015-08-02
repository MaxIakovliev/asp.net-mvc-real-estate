using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRealty.Models;
using WebRealty.Common;
using System.IO;
using WebRealty.DataLayer;

namespace WebRealty.Controllers
{
    public class SearchResultController : Controller
    {
        RealtyDb _db = new RealtyDb();
        //
        // GET: /SearchResult/

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult ShowSearchPath()//(string PropertyType, string PropertyTypeAction, string PropertyTypeCities, string PropertyTypeCityDistrict)
        {
            string PropertyType = (Request.QueryString["PropertyType"]);
            string PropertyTypeAction = Request.QueryString["PropertyTypeAction"];
            string PropertyTypeCities = Request.QueryString["PropertyTypeCities"];
            string PropertyTypeCityDistrict = Request.QueryString["PropertyTypeCityDistrict"];
            string NavigatePage = Request.QueryString["NavigatePage"];
            string SortTypeDll = Request.QueryString["sortType"];

            //int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict;
            //iPropertyType = iPropertyTypeAction = iPropertyTypeCities = iPropertyTypeCityDistrict = -1;

            //int.TryParse(PropertyType, out iPropertyType);
            //int.TryParse(PropertyTypeAction, out iPropertyTypeAction);
            //int.TryParse(PropertyTypeCities, out iPropertyTypeCities);
            //int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict);

            var model = new SearchPathModel();
            //model.PropertyType = (from s in _db.PropertyTypes.AsQueryable()
            //                      where s.Id == iPropertyType
            //                      select new Pair<int, string>() { Key = iPropertyType, Value = s.PropertyTypeName }).FirstOrDefault<Pair<int, string>>();

            //model.PropertyAction = (from s in _db.PropertyActions.AsQueryable()
            //                        where s.Id == iPropertyTypeAction
            //                        select new Pair<int, string>() { Key = s.Id, Value = s.PropertyActionName }).FirstOrDefault<Pair<int, string>>();

            //model.City = (from s in _db.Cities.AsQueryable()
            //              where s.Id == iPropertyTypeCities
            //              select new Pair<int, string>() { Key = s.Id, Value = s.CityName }).FirstOrDefault<Pair<int, string>>();

            //model.CityDisctict = (from s in _db.CityDistricts.AsQueryable()
            //                      where s.Id == iPropertyTypeCityDistrict
            //                      select new Pair<int, string>() { Key = s.Id, Value = s.District }).FirstOrDefault<Pair<int, string>>();
            int id;

            #region propertyType populating
            int.TryParse(PropertyType, out id);
            if (id > 0)
            {
                var data = new PropertyTypeRoutines(this._db);
                model.PropertyType = data.GetById(id);
                model.PropertyTypes = data.GetPropertyTypes();
                model.PropertyTypes = model.PropertyTypes.Where(m => m.PropertyTypeName != model.PropertyType.PropertyTypeName);
                model.PropertyTypes = model.PropertyTypes.Where(m => !m.PropertyTypeName.Contains("Выберите"));
            }
            #endregion

            #region PropertyTypeAction populating
            int.TryParse(PropertyTypeAction, out id);
            if (id > 0)
            {
                var data = new PropertyActionRoutines(this._db);
                model.PropertyAction = data.GetById(id);
                model.PropertyActions = data.GetPropertyActions();
                if (model.PropertyType != null)
                {
                    model.PropertyActions = model.PropertyActions.Where(m => m.PropertyType.Id == model.PropertyType.Id);
                }
            }

           
            #endregion

            #region PropertyTypeCities populating
            int.TryParse(PropertyTypeCities, out id);
            if (id > 0)
            {
                var data = new CityRoutines(this._db);
                model.City = data.GetById(id);
                model.Cities = data.GetCities();
                model.Cities = model.Cities.Where(m => m.Id != model.City.Id);
            }
            #endregion

            #region PropertyTypeCityDistrict populating
            int.TryParse(PropertyTypeCityDistrict, out id);
            if (id > 0)
            {
                var data = new CityDistrictRoutines(this._db);
                model.CityDisctict = data.GetById(id);
                model.CityDiscticts = data.GetCityDiscticts();
                model.CityDiscticts = model.CityDiscticts.Where(m => m.City.Id == model.CityDisctict.City.Id);
                model.CityDiscticts = model.CityDiscticts.Where(m => m.Id != model.CityDisctict.Id);
            }
            #endregion


            return PartialView("ShowSearchPath", model);
        }

        public PartialViewResult ShowPaging(
            string PropertyType,
            string PropertyTypeAction,
            string PropertyTypeCities,
            string PropertyTypeCityDistrict,
            string NavigatePage,
            string SortType,
            string ShowType,
            string pricemin,
            string pricemax,
            string currency,
            string commercialPropertyType="",
            string countFloors="",
            string distToCityMin="",
            string distToCityMax="",
            string buildingType="",
            string wcType="",
            string roomCount="",
            string sizemin="",
            string sizemax=""
            )
        {
           

            int pageSize = WebRealty.Common.PagingHelper.GetPageSize();

            int iPropertyType, iPropertyTypeAction, iPropertyTypeCities, iPropertyTypeCityDistrict, iNavigatePage, iSortTypeDll,
                iShowType, iPriceMin, iPriceMax, iCurrency,
                iBuildingType, iWcType, iRoomCount, iSizemin, iSizemax,
                iCommercialPropertyType, iCountFloors, iDistToCityMin, iDistToCityMax;

            iPropertyType = iPropertyTypeAction = iPropertyTypeCities = iPropertyTypeCityDistrict = iSortTypeDll = iPriceMin = iPriceMax = iCurrency = -1;
            iNavigatePage = iShowType = 
                iBuildingType= iWcType= iRoomCount= iSizemin= iSizemax=
                iCommercialPropertyType= iCountFloors= iDistToCityMin= iDistToCityMax=1;

            if (!int.TryParse(PropertyType, out iPropertyType)) iPropertyType = -1;
            if (!int.TryParse(PropertyTypeAction, out iPropertyTypeAction)) iPropertyTypeAction = -1;
            if (!int.TryParse(PropertyTypeCities, out iPropertyTypeCities)) iPropertyTypeCities = -1;
            if (!int.TryParse(PropertyTypeCityDistrict, out iPropertyTypeCityDistrict)) iPropertyTypeCityDistrict = -1;
            if (!int.TryParse(NavigatePage, out iNavigatePage)) iNavigatePage = 1;
            if (!int.TryParse(SortType, out iSortTypeDll)) iSortTypeDll = 1;
            if (!int.TryParse(ShowType, out iShowType)) iShowType = -1;

            if (!int.TryParse(commercialPropertyType, out iCommercialPropertyType)) iCommercialPropertyType = -1;
            if (!int.TryParse(countFloors, out iCountFloors)) iCountFloors = -1;
            if (!int.TryParse(distToCityMin, out iDistToCityMin)) iDistToCityMin = -1;
            if (!int.TryParse(distToCityMax, out iDistToCityMax)) iDistToCityMax = -1;
            if (!int.TryParse(buildingType, out iBuildingType)) iBuildingType = -1;
            if (!int.TryParse(wcType, out iWcType)) iWcType = -1;
            if (!int.TryParse(roomCount, out iRoomCount)) iRoomCount = -1;
            if (!int.TryParse(sizemin, out iSizemin)) iSizemin = -1;
            if (!int.TryParse(sizemax, out iSizemax)) iSizemax = -1;


            int countObjs = 0;
            var result = from s in _db.PropertyObjects
                         where s.IsDeleted == (int)DeleteStatus.Available
                         select s;
            if (iPropertyType > -1)
                result = result.Where(x => x.PropertyType.Id == iPropertyType);

            if (iPropertyTypeAction > -1)
                result = result.Where(x => x.PropertyAction.Id == iPropertyTypeAction);

            if (iPropertyTypeCities > -1)
                result = result.Where(x => x.City.Id == iPropertyTypeCities);

            if (iPropertyTypeCityDistrict > -1)
                result = result.Where(x => x.CityDistrict.Id == iPropertyTypeCityDistrict);

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

            countObjs = (from s in result
                         select 1).Count<int>();



            SearchPageParams model = new SearchPageParams()
            {
                iPropertyType = iPropertyType,
                iPropertyTypeAction = iPropertyTypeAction,
                iPropertyTypeCities = iPropertyTypeCities,
                iPropertyTypeCityDistrict = iPropertyTypeCityDistrict,
                countObjectFound = countObjs,
                countPagesFound = countObjs / pageSize,
                currentPage = iNavigatePage,
                navigatePage = iNavigatePage,
                ShowType = iShowType,
                SortType = iSortTypeDll,
                
                commercialPropertyType=iCommercialPropertyType,
                countFloors=iCountFloors,
                distToCityMin=iDistToCityMin,
                distToCityMax=iDistToCityMax,
                buildingType=iBuildingType,
                wcType=iWcType,
                roomCount=iRoomCount,
                sizemin=iSizemin,
                sizemax = iSizemax
            };
            if (countObjs % pageSize > 0) model.countPagesFound++;
            return PartialView("ShowPaging", model);
        }

        public PartialViewResult LastAddedObjects()
        {
            int pageSize = WebRealty.Common.PagingHelper.GetPageSize();
            var model = (from s in _db.PropertyObjects.Include("Currency")
                         where s.IsDeleted == (int)DeleteStatus.Available
                         select s)
                .OrderByDescending(t => t.CreatedDate)
                                      .Take(pageSize);
            return PartialView("LastAddedObjects", model);
        }

        public ActionResult ObjectDetails(string Id, bool IsJustAdded = false)
        {
            var model = new DetailedObject();
            int tmp = -1;
            if (!int.TryParse(Id, out tmp))
            {
                return View("Home");
            }
            model.PropertyObject = (from s in _db.PropertyObjects.Include("City")
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
                                        .Include("CommercialPropertyType")
                                        .AsQueryable()
                                    where s.Id == tmp
                                    && s.IsDeleted == (int)DeleteStatus.Available
                                    select s).SingleOrDefault();


            var images = (from s in _db.ObjectImages
                          where s.PropertyObject.Id == tmp
                          select s.Id).ToList<int>();
            ViewBag.imgIds = images;

            if (model.PropertyObject == null)
                return RedirectToAction("index", "home");

            if (model.PropertyObject.PropertyType.Id == 3) //земельные участки
            {
                var landCommunicationToObj = (from s in _db.LandCommunicationToObjs
                                              .Include("LandCommunicationId")
                                              where s.PropertyObjectId.Id == model.PropertyObject.Id
                                              select s).ToList<RealtyDomainObjects.LandCommunicationToObj>();
                ViewBag.landCommunicationToObj = landCommunicationToObj;

                var landFunctionToObj = (from s in _db.LandFunctionToObjs
                                         .Include("LandFunctionId")
                                         where s.PropertyObjectId.Id == model.PropertyObject.Id
                                         select s).ToList<RealtyDomainObjects.LandFunctionToObj>();
                ViewBag.landFunctionToObj = landFunctionToObj;
            }


            #region показываем фотографию пользователя объявления
            if (model.PropertyObject.UserOwner != null)
            {
                var existUserData = (from s in _db.UserPhotos
                                     where s.MembershipId == model.PropertyObject.UserOwner.MembershipId
                                     select s).SingleOrDefault<RealtyDomainObjects.UserPhoto>();
                if (existUserData != null && existUserData.img != null)
                {
                    ViewBag.membershipId = model.PropertyObject.UserOwner.MembershipId;
                    ViewBag.UserOwner = existUserData.Title;
                }
            }
            #endregion

            if (IsJustAdded)
                ViewBag.IsJustAdded = true;


            //#region check if user have a profile avatar


            //if (model.PropertyObject.UserOwner!=null)
            //{
            //    var userImg = (from s in _db.UserPhotos.AsQueryable()
            //                   where s.Id == model.PropertyObject.UserOwner.Id
            //                   && s.img != null
            //                   select 1).Count<int>();
            //    if (userImg != null && userImg > 0)
            //        ViewBag.userImg = userImg;
            //}
            //#endregion



            PropertyStatsHelper.CreateProperyStat(model.PropertyObject, _db);

            model.stats = new ConclusiveViewStats();

            model.stats.totalView = (from s in _db.PropertyStats
                                     where s.PropertyObject.Id == model.PropertyObject.Id
                                     select 1).Count<int>();

            model.stats.todayView = (from s in _db.PropertyStats
                                     where s.day == DateTime.Today.Day
                                     && s.month == DateTime.Today.Month
                                     && s.year == DateTime.Today.Year
                                     && s.PropertyObject.Id == model.PropertyObject.Id
                                     select 1).Count<int>();

            return View(model);


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileContentResult getImgPreview(string id)
        {
            int intId;
            if (!int.TryParse(id, out intId))
            {
                return new FileContentResult(getFakeImg(), "image/jpeg");
            }
            else
            {

                byte[] byteArray = (from s in _db.ObjectImages.AsQueryable()
                                    where s.PropertyObject.Id == intId
                                    select s.ImagePreview).FirstOrDefault<byte[]>();
                if (byteArray != null)
                {
                    return new FileContentResult(byteArray, "image/jpeg");
                }
                else
                {
                    return new FileContentResult(getFakeImg(), "image/jpeg");

                }
            }
        }


        //private static object fakeFileLocker = new object();
        
        [OutputCache (Duration=1600)]
        private byte[] getFakeImg()
        {

            
            var path = Server.MapPath("~/Content/1.jpg");
            //System.IO.FileStream fs =null;
            byte[] f = System.IO.File.ReadAllBytes(path);
            //lock (fakeFileLocker)
            //{
            //    fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
            //    using (System.Drawing.Image img = System.Drawing.Image.FromStream(fs))
            //    {
            //        //Initialise the size of the array
            //        f = new byte[fs.Length];
            //        //Create a new BinaryReader and set the InputStream  for the Images InputStream to the beginning, as we create the img using a stream.
            //        var reader = new BinaryReader(fs);
            //        fs.Seek(0, SeekOrigin.Begin);
            //        //Load the image binary.
            //        f = reader.ReadBytes((int)fs.Length);
            //    }
            //}
            return f;
        }


        
        public ActionResult GetPhones(int ID)
        {
            var phones = from s in _db.PropertyObjects
                                  where s.Id == ID
                                  select new { phone1 = s.Phone1, phone2 = s.Phone2, phone3=s.Phone3 };

           // if (HttpContext.Request.IsAjaxRequest())
                return Json(phones , JsonRequestBehavior.AllowGet);

           // return RedirectToAction("Index");
        }

    }
}
