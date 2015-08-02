using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.DataLayer
{
    public class AgencyToPhoneRoutines
    {
        private RealtyDb _db;
        public AgencyToPhoneRoutines(RealtyDb _db)
        {
            this._db = _db;
        }
        public IQueryable<AgencyToPhone> GetPhonesByAgencyId(List<Agency> agencies)
        {
            var result = from s in this._db.AgencyToPhones.Include("Agency")
                         where s.IsDeleted == false
                         select s;
            for (int i=0; i<agencies.Count(); i++)
                result=result.Where(m=>m.Agency.Id==agencies[i].Id);
            result = result.OrderBy(m => m.Agency.Id);
            return result;
                         
        }

    }
}