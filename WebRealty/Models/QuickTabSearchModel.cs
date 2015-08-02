using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRealty.Models
{
    public class QuickTabSearchModel
    {
        public SelectList propertyType { get; set; }
        public SelectList propertyActionGeneral { get; set; }

        public SelectList flatPropertyActions { get; set; }
        public SelectList commercialPropertyAction { get; set; }
        public SelectList foreignPropertyAction { get; set; }
        public SelectList housesPropertyAction { get; set; }
        public SelectList landPropertyAction { get; set; }
        public SelectList parkingGaragePropertyAction { get; set; }
        public SelectList servicesPropertyAction { get; set; }

        public SelectList cities { get; set; }
        public SelectList districts { get; set; }
        public SelectList currecy { get; set; }


    }
}