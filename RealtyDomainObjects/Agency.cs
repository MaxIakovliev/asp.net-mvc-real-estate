using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RealtyDomainObjects
{
    /// <summary>
    /// агенство недвижимости
    /// </summary>
    public class Agency
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //[Required]
        public City City { get; set; }
        public CityDisctict District { get; set; }
        public string AboutBrief { get; set; }
        public string AboutDetailed { get; set; }       
        public byte[] Photo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
