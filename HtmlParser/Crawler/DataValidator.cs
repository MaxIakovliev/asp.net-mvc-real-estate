using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RealtyDomainObjects;

namespace HtmlParser.Crawler
{
    public class DataValidator
    {
        public void ValidateAndFix(PropertyObject po)
        {
            if (po == null) return;
            string reason = string.Empty;
            bool markToDelete = false;
    
            if (po.PropertyType == null)
            {
                markToDelete = true;
                reason = "Не указан тип нобъекта";
            }



            if (string.IsNullOrEmpty(po.Title) || po.Title.Length < 10)
            {
                reason = "Заголовок для данного объявления не найден";
                markToDelete = true;
            }

            if (!string.IsNullOrEmpty(po.Title) && po.Title.Length >= 500)
            {
                po.Title = po.Title.Substring(0, 499);
            }

            if (string.IsNullOrEmpty(po.PropertyDescription) || po.PropertyDescription.Length < 50)
            {
                reason = "Текст объявления не найден";
                markToDelete = true;
            }

            if (string.IsNullOrEmpty(po.Phone1) || po.Phone1.Length < 5)
            {
                reason = "Телефон не найден";
                markToDelete = true;
            }

            if (!string.IsNullOrEmpty(po.Phone1) && po.Phone1.Length >= 20)
            {
                po.Phone1 = po.Phone1.Substring(0, 19);                
            }


            if (string.IsNullOrEmpty(po.ContactName))
                po.ContactName = "Агент";

            po.IsDeleted = (markToDelete ? 1 : 0);
            po.ReasonOfDelete = reason;
            if (markToDelete)
                po.DeletedDate = DateTime.Now;
        }
    }
}
