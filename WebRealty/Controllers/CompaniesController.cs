using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRealty.DataLayer;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.Controllers
{
    public class CompaniesController : Controller
    {
        //
        // GET: /Companies/

        RealtyDb _db = new RealtyDb();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchCompanies(int cityId = -1, int districtId = -1, int page = 1)
        {
            var model = new AgencyModel();
            int pageSize = WebRealty.Common.PagingHelper.GetPageSize();            
            model.Agencies = new AgencyRoutines(this._db).GetAgency(cityId, districtId, page, pageSize).ToList<Agency>();
            //model.Phones = new AgencyToPhoneRoutines(this._db).GetPhonesByAgencyId(model.Agencies).ToList<AgencyToPhone>();
            return View(model);
        }

        public PartialViewResult AgencySearchBox()
        {
            var data = new CityRoutines(this._db).GetCities().ToList<City>();
            var model = new SelectList(data, "id", "CityName", 0);
            return PartialView(model);
        }



    }
}
