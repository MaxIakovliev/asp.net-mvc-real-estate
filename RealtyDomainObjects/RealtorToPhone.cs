using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtyDomainObjects
{
   public class RealtorToPhone
    {
        public int Id { get; set; }
        public Realtor Realtor { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
