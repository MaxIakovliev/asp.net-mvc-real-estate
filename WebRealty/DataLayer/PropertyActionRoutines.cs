using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.DataLayer
{
    public class PropertyActionRoutines
    {
        private RealtyDb _db;

        public PropertyActionRoutines(RealtyDb _db)
        {
            this._db = _db;
        }

        public PropertyAction GetById(int id)
        {
            return (from s in this._db.PropertyActions
                    where s.Id == id
                    select s).SingleOrDefault<PropertyAction>();
        }

        public IQueryable<PropertyAction> GetPropertyActions()
        {
            return from s in this._db.PropertyActions.AsQueryable()
                   select s;
        }
    }
}