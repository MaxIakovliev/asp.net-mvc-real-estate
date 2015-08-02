using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealtyDomainObjects;

namespace WebRealty.Models
{
    public class AgencyModel
    {
        public List<Agency> Agencies { get; set; }
        public List<AgencyToPhone> Phones { get; set; }

    }
}