using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.DataLayer
{
    public class CityDistrictRoutines
    {
        private RealtyDb _db;

        public CityDistrictRoutines(RealtyDb _db)
        {
            this._db = _db;
        }

        public CityDisctict GetById(int id)
        {
            return (from s in this._db.CityDistricts
                    where s.Id == id
                    select s).SingleOrDefault<CityDisctict>();
        }

        public IQueryable<CityDisctict> GetCityDiscticts()
        {
            return from s in this._db.CityDistricts.AsQueryable()
                   select s;
        }
    }
}