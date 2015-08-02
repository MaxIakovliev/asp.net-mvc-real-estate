using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;

namespace WebRealty.Common
{
    public static class Auth
    {
        public static bool IsAuthenticated(System.Web.HttpRequestBase Request, IPrincipal User, out int membershipId)
        {
            membershipId = -1;
            #region check for security
            if (!Request.IsAuthenticated)
                return false;

            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);

            if (currentUser == null || currentUser.ProviderUserKey == null || !int.TryParse(currentUser.ProviderUserKey.ToString(), out membershipId))
            {
                membershipId = -1;
                return false;
            }
            #endregion
            return true;
        }

        public static RealtyDomainObjects.PropertyObject GetUserDetails(int membershipId, WebRealty.Models.RealtyDb _db)
        {
            var userDetails = (from s in _db.Users
                           where s.MembershipId == membershipId
                           && s.ApplyToAd == true
                           select s).SingleOrDefault<RealtyDomainObjects.User>();
             var po = new RealtyDomainObjects.PropertyObject();
            if (userDetails != null)
            {
                po.Phone1 = userDetails.Phone1;
                po.Phone2 = userDetails.Phone2;
                po.Phone3 = userDetails.Phone3;
                po.SourceUrl = userDetails.url;
                po.ContactName = userDetails.UserName;
            }
            return po;
        }
    }
}