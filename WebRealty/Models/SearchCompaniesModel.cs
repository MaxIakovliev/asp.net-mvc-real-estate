using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRealty.Models
{
    public class SearchCompaniesModel
    {
        public SelectList City { get; set; }
        public SelectList District { get; set; }
    }
}