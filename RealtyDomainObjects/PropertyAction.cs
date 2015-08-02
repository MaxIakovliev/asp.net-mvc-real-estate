using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtyDomainObjects
{
    public class PropertyAction
    {
        public int Id { get; set; }
        public string PropertyActionName { get; set; }
        public PropertyType PropertyType { get; set; }
    }
}
