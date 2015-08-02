using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace RealtyDomainObjects
{
    public class ServiceType
    {
        public int Id { get; set; }
        [DisplayName("Тип услуги")]
        public string ServiceTypeName { get; set; }
    }
}
