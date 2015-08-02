using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtyDomainObjects
{
    public class LandFunctionToObj
    {
        public int Id { get; set; }
        public LandFunction LandFunctionId { get; set; }
        public PropertyObject PropertyObjectId { get; set; }
    }
}
