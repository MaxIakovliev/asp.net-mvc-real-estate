using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.DataLayer
{
    public class PropertyTypeRoutines
    {
        private RealtyDb _db;

        public PropertyTypeRoutines(RealtyDb _db)
        {
            this._db = _db;
        }

        public PropertyType GetById(int id)
        {
            return (from s in _db.PropertyTypes
                    where s.Id == id
                    select s).SingleOrDefault<PropertyType>();
        }

        public IQueryable<PropertyType> GetPropertyTypes()
        {
            return from s in _db.PropertyTypes.AsQueryable()
                    select s;
        }
    }
}