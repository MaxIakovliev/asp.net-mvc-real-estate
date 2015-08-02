using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealtyDomainObjects;
using WebRealty.Models;

namespace WebRealty.Common
{
    public  class PropertyStatsHelper
    {
        public static void CreateProperyStat(PropertyObject po, RealtyDb _db)
        {
            _db.PropertyStats.Add(new PropertyStat()
            {
                PropertyObject = po,
                day = DateTime.Today.Day,
                month = DateTime.Today.Month,
                year = DateTime.Today.Year
            });
            _db.SaveChanges();
        }
    }
}