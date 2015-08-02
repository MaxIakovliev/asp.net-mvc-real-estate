using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace RealtyDomainObjects
{
    /// <summary>
    /// назначение земли
    /// </summary>
    public class LandFunction
    {
        
        public int Id { get; set; }
        [DisplayName("Назначение:")]
        public string LandFunctionName { get; set; }
        public Guid guid { get; set; }
        //public string unicName { get; set; }
    }
}
