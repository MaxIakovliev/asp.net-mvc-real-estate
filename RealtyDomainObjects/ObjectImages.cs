using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtyDomainObjects
{
    public class ObjectImages
    {
        public int Id { get; set; }
        public PropertyObject PropertyObject { get; set; }
        public int FileSize { get; set; }
        public string FileName { get; set; }
        public string Content_Type { get; set; }
        public int Height { get; set; }
        public int Width  { get; set; }
        public byte[] Image { get; set; }
        public byte[] ImagePreview { get; set; }
        public bool IsDeleted { get; set; }


    }
}
