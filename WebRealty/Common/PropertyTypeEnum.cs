using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRealty.Common
{
    public enum PropertyTypeEnum
    {
        [StringValue("Квартиры")]
        flat = 1,
        [StringValue("Дома. Дачи")]
        houses,
        [StringValue("Замельные участки")]
        land,
        [StringValue("Коммерческая недвижимость")]
        commercialProperty,
        [StringValue("Гаражи и паркинги")]
        parkingGarage,
        [StringValue("Недвижимость за рубежом")]
        foreignProperty,
        [StringValue("Услуги")]
        services


    }
}