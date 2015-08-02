using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RealtyDomainObjects
{
    public class Realtor
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string About { get; set; }
        public Agency Agency { get; set; }
        [Required]
        public City City { get; set; }
        public CityDisctict District { get; set; }
        public bool CallBack { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
