using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtyDomainObjects
{
    /// <summary>
    /// статистика просмотров объектов
    /// </summary>
    public class PropertyStat
    {
        public int Id { get; set; }
        public PropertyObject PropertyObject { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }
}
