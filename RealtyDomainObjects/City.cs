using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtyDomainObjects
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CityNamePadej { get; set; } //Название города в падеже
        public bool important { get; set; }
        
        
    }
}
