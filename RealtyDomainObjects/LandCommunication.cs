using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace RealtyDomainObjects
{
    /// <summary>
    /// городские коммуникации
    /// </summary>
    public class LandCommunication
    {
        //Коммуникации
        public int Id { get; set; }
        [DisplayName("Коммуникации:")]
        public string LandCommunicationsName { get; set; }
        public Guid guid { get; set; }
        //public string  unicName { get;  set; }
    }
}
