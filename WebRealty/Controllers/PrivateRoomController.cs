using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RealtyDomainObjects;
using WebRealty.Models;
using System.Data;
using WebRealty.Common;
using System.IO;

namespace WebRealty.Controllers
{
    public class PrivateRoomController : Controller
    {
        //
        // GET: /PrivateRoom/
        RealtyDb _db = new RealtyDb();

        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                MembershipUser CurrentUser = Membership.GetUser(User.Identity.Name);
                //Response.Write("CurrentUser ID :: " + CurrentUser.ProviderUserKey);
                var model = new List<DetailedObject>();

                int userKey = Convert.ToInt32(CurrentUser.ProviderUserKey);
                var propertyObjects = (from s in _db.PropertyObjects.Include("Currency")
                                       where s.UserOwner.MembershipId == userKey
                                       &&s.IsDeleted!=2 //удалено навсегда
                                       select s).OrderByDescending(c=>c.CreatedDate).ToList<PropertyObject>();


                foreach (var item in propertyObjects)
                {
                    ConclusiveViewStats stat = new ConclusiveViewStats();
                    stat.totalView= (from s in _db.PropertyStats
                                     where s.PropertyObject.Id == item.Id
                                     select 1).Count<int>();
                    var imgIds = (from s in _db.ObjectImages
                                  where s.PropertyObject.Id == item.Id
                                  select s.Id).ToList<int>();
                    stat.todayView=-1;//not important here
                    
                    model.Add(new DetailedObject() {
                    PropertyObject=item,
                    stats=stat,
                    imgIds=imgIds
                    });


                }

                return View(model);
            }
            return RedirectToAction("LogOn", "Account");
        }

        public ActionResult ChangeUserSignatures()
        {
            int membershipId = -1;
            if(Auth.IsAuthenticated(Request,User,out membershipId))
            //if (IsAuthenticated(out membershipId))
            {
                var existUserData = (from s in _db.Users
                                     where s.MembershipId == membershipId
                                     select s).SingleOrDefault<User>();
                return PartialView("ChangeUserSignatures", existUserData);
            }
            return RedirectToAction("index", "home");
        }


        public ActionResult SaveUserSignatures(User user)
        {
            int membershipId = -1;
            if (Auth.IsAuthenticated(Request, User, out membershipId))
                //if (IsAuthenticated(out membershipId))
            {

                bool newRecord = true;
                var existUserData = (from s in _db.Users
                                     where s.MembershipId == membershipId
                                     select s).SingleOrDefault<User>();

                if (existUserData == null)
                {
                    existUserData = user;
                }
                else
                {
                    newRecord = false;
                    existUserData.Phone1 = user.Phone1;
                    existUserData.Phone2 = user.Phone2;
                    existUserData.Phone3 = user.Phone3;
                    existUserData.url = user.url;
                    existUserData.UserName = user.UserName;
                    existUserData.ApplyToAd = user.ApplyToAd;
                }
                existUserData.MembershipId = membershipId;

                if (newRecord)
                    _db.Users.Add(existUserData);
                else
                {
                    _db.Users.Attach(existUserData);
                    _db.Entry(existUserData).State = EntityState.Modified;
                }
                _db.SaveChanges();

                return RedirectToAction("index", "PrivateRoom");
            }
            return RedirectToAction("index", "home");


        }


        public ActionResult UploadUserPhoto()
        {
            int membershipId = -1;
            if (Auth.IsAuthenticated(Request, User, out membershipId))
            //if (IsAuthenticated(out membershipId))
            {
                var existUserData = (from s in _db.UserPhotos
                                     where s.MembershipId == membershipId
                                     select s).SingleOrDefault<UserPhoto>();
                if (existUserData != null && existUserData.img != null)
                {
                    ViewBag.membershipId = membershipId;
                    ViewBag.title = existUserData.Title;
                }

                return PartialView("UploadUserPhoto");
            }
            return RedirectToAction("index", "home");
        }

        public ActionResult SaveUserPhoto(HttpPostedFileBase file, string title)
        {
            int membershipId = -1;
            if (Auth.IsAuthenticated(Request, User, out membershipId))
            //if (IsAuthenticated(out membershipId))
            {
                ImageProcessing imgHelper = new ImageProcessing();
                using (System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream))
                {
                    //Initialise the size of the array
                    byte[] f = new byte[file.InputStream.Length];
                    //Create a new BinaryReader and set the InputStream  for the Images InputStream to the beginning, as we create the img using a stream.
                    var reader = new BinaryReader(file.InputStream);
                    file.InputStream.Seek(0, SeekOrigin.Begin);
                    //Load the image binary.
                    f = reader.ReadBytes((int)file.InputStream.Length);
                    f = imgHelper.ResizeImage(f, new System.Drawing.Size(120, 90));

                    bool newRecord = true;
                    var existUserData = (from s in _db.UserPhotos
                                         where s.MembershipId == membershipId
                                         select s).SingleOrDefault<UserPhoto>();

                    if (existUserData == null)
                    {
                        existUserData = new UserPhoto() { 
                        img=f,
                        MembershipId=membershipId
                        };
                    }
                    else
                    {
                        newRecord = false;
                        existUserData.img = f;                        
                    }
                    existUserData.MembershipId = membershipId;
                    existUserData.Title = Server.HtmlEncode(title);


                    if (newRecord)
                        _db.UserPhotos.Add(existUserData);
                    else
                    {
                        _db.UserPhotos.Attach(existUserData);
                        _db.Entry(existUserData).State = EntityState.Modified;
                    }
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("index", "home");
        }

        public FileContentResult getImg(string id)
        {
            int iId;
            if (!int.TryParse(id, out iId))
                return null;

            byte[] byteArray = (from s in _db.UserPhotos.AsQueryable()
                                where s.MembershipId == iId
                                select s.img).SingleOrDefault<byte[]>();
            if (byteArray != null)
            {
                return new FileContentResult(byteArray, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        public ActionResult DeleteUserPhoto()
        {
            int membershipId;
            if (!Auth.IsAuthenticated(Request, User, out membershipId))
                return RedirectToAction("index", "home");

            var obj = (from s in _db.UserPhotos
                      where s.MembershipId == membershipId
                      select s).SingleOrDefault<UserPhoto>();

            if (obj != null)
            {
                _db.UserPhotos.Remove(obj);
                _db.SaveChanges();
            }
            return RedirectToAction("index", "PrivateRoom");
        }
        
        /*
        private bool IsAuthenticated(out int membershipId)
        {
            membershipId = -1;
            #region check for security
            if (!Request.IsAuthenticated)
                return false;

            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);

            if (currentUser == null || currentUser.ProviderUserKey == null || !int.TryParse(currentUser.ProviderUserKey.ToString(), out membershipId))
                return false;
            #endregion
            return true;
        }
         */ 

    }
}
