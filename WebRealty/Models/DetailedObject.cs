using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealtyDomainObjects;

namespace WebRealty.Models
{
    public class DetailedObject
    {
        public PropertyObject PropertyObject { get; set; }
        public List<ObjectImages> images { get; set; }
        public List<int> imgIds { get; set; }
        public ConclusiveViewStats stats { get; set; }
    }
}