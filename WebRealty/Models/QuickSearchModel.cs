using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealtyDomainObjects;

namespace WebRealty.Models
{
    public class QuickSearchModel
    {        
        public List<PropertyAction> PropertyAction { get; set; }
        public List<City> City { get; set; }
    }
}