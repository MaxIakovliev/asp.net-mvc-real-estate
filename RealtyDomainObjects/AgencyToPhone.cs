using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RealtyDomainObjects
{
    public class AgencyToPhone
    {
        public int Id { get; set; }
        [Required]
        public Agency Agency { get; set; }
        [Required]
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
