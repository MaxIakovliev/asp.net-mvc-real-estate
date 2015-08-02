using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealtyDomainObjects;
using System.Collections;

namespace WebRealty.Models
{
    public class SearchObjectParams
    {
        public IEnumerable<PropertyType> PropertyType { get; set; }
        //public IEnumerable<Pair<int, string>> PropertyAction { get; set; }
        public System.Web.Mvc.SelectList PropertyAction { get; set; }
        public System.Web.Mvc.SelectList CityDisctict { get; set; }
        public System.Web.Mvc.SelectList City { get; set; }
        public int PropertyTypeId { get; set; }
        public int? minPrice { get; set; }
        public int? maxPrice { get; set; }
        public int CurrencyId { get; set; }
        public bool? NoCommission { get; set; }


        
    }// IEnumerable<RealtyDomainObjects.PropertyType>
}