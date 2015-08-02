using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealtyDomainObjects;

namespace WebRealty.Models
{
    public class SearchPathModel
    {
        //public Pair<int,string> PropertyType { get; set; }
        //public Pair<int, string> PropertyAction { get; set; }
        //public Pair<int, string> CityDisctict { get; set; }
        //public Pair<int, string> City { get; set; }

        public PropertyType PropertyType { get; set; }
        public PropertyAction PropertyAction { get; set; }
        public CityDisctict CityDisctict { get; set; }
        public City City { get; set; }

        public IQueryable<PropertyType> PropertyTypes { get; set; }
        public IQueryable<PropertyAction> PropertyActions { get; set; }
        public IQueryable<City> Cities { get; set; }
        public IQueryable<CityDisctict> CityDiscticts { get; set; }
    }
}