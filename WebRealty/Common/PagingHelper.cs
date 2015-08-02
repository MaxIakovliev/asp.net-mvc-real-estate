using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRealty.Common
{
    public static class PagingHelper
    {
        public static int GetPageSize()
        {
            //int pageSize = 0;
            return Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["SearchResultPageSize"].ToString());


            //System.Configuration.Configuration rootWebConfig1 =
            //    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            //if (rootWebConfig1.AppSettings.Settings.Count > 0)
            //{
            //    System.Configuration.KeyValueConfigurationElement customSetting =
            //        rootWebConfig1.AppSettings.Settings["SearchResultPageSize"];
            //    if (customSetting != null)
            //        pageSize = Convert.ToInt32(customSetting.Value);
            //    else
            //        pageSize = 10;
            //}
            //return pageSize;
        }
    }
}