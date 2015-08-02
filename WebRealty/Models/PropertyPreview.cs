using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealtyDomainObjects;
using System.Drawing;

namespace WebRealty.Models
{
    public class PropertyPreview
    {
        public PropertyObject PropertObject { get; set; }
        public List<int> ImgPreviewIds { get; set; }

    }
}