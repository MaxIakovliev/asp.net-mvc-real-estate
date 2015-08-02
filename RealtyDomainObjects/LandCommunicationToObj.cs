using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtyDomainObjects
{
    public class LandCommunicationToObj
    {
        public int Id { get; set; }
        public LandCommunication LandCommunicationId { get; set; }
        public PropertyObject PropertyObjectId { get; set; }
    }
}
