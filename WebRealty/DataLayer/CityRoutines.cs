using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.DataLayer
{
    public class CityRoutines
    {
        private RealtyDb _db;

        public CityRoutines(RealtyDb _db)
        {
            this._db = _db;
        }

        public City GetById(int id)
        {
            return (from s in this._db.Cities
                    where s.Id == id
                    select s).SingleOrDefault<City>();
        }

        public IQueryable<City> GetCities()
        {
            return from s in this._db.Cities.AsQueryable()
                   select s;
        }



    }
}