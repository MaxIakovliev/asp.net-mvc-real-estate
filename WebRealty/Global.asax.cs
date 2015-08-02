using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using WebRealty.Models;
using System.Data;
using System.IO;
using RealtyDomainObjects;
using WebRealty.Common;

namespace WebRealty
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            Database.SetInitializer(new RealtyDbInitializer());// uncomment it 

            AreaRegistration.RegisterAllAreas();


            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
    public class RealtyDbInitializer : DropCreateDatabaseIfModelChanges<RealtyDb>
    {

        private void AddPropertyTypes(RealtyDb context, string PropertyActionName, PropertyType pt)
        {
            context.PropertyActions.Add(new RealtyDomainObjects.PropertyAction()
            {
                Id = 1,
                PropertyActionName = PropertyActionName,
                PropertyType = pt
            });
            context.SaveChanges();
        }


        protected override void Seed(RealtyDb context)
        {
            base.Seed(context);
           
            DBBasicDataLoader dataLoader = new DBBasicDataLoader(
                System.Web.Hosting.HostingEnvironment.MapPath("~/Infrastructure"), context);

            dataLoader.LoadCities();
            dataLoader.LoadDistricts();
            dataLoader.LoadPropertyTypes();
            dataLoader.LoadPropertyActions();
            dataLoader.LoadPeriod();
            dataLoader.LoadBuildingType();
            dataLoader.LoadPriceForType();
            dataLoader.LoadCurrencyType();
            dataLoader.LoadWCType();
            dataLoader.LoadLandCommunication();
            dataLoader.LoadLandFunction();
            dataLoader.LoadCommercialPropertyType();
            dataLoader.LoadServiceType();


            for (int i = 0; i < 10; i++)
            {
                var agency = new Agency()
                {
                    City = null,
                    District = null,
                    Name = "ОЛИМП "+i.ToString(),
                    AboutBrief = "АН ОЛІМП надає наступні послуги: рекомендації по процедурі проведення правочину та взаєморозрахунків між його учасниками, консультування клієнта про стан ринку нерухомості, формування пакету правовстановлюючих та інших документів, експертне визначення ринкової вартості об’єкта нерухомості, консультації клієнта про можливість та процедуру отримання кредиту під купівлю нерухомості та інш. ",
                    AboutDetailed = "Detailed info АН ОЛІМП надає наступні послуги: рекомендації по процедурі проведення правочину та взаєморозрахунків між його учасниками, консультування клієнта про стан ринку нерухомості, формування пакету правовстановлюючих та інших документів, експертне визначення ринкової вартості об’єкта нерухомості, консультації клієнта про можливість та процедуру отримання кредиту під купівлю нерухомості та інш.",
                    Photo = null
                    
                };

                context.Agencies.Add(agency);
                context.SaveChanges();
            }

            //dataLoader.LoadFakeData();
            
            /*
            context.PropertyObjects.Add(new PropertyObject()
            {
                Id = 1,
                City = c1,
                CityDistrict = cd1,
                ContactName = "Вася Пупкин",
                CreatedDate = DateTime.Now,
                //Currency = "UAH",
                IsActive = true,
                IsDeleted = 0,
                KitchenSpace = 20,
                LivingSpace = 60,
                Price = 7000,
                PropertyAction = pa1,
                PropertyDescription = "Десятинная 13, дореволюционный дом, реконструкция 2011г с надстройкой 2-х этажей3 комнаты, в т.ч. гостиная с зоной кухни и столовой, сделаны в одном пространстве",
                PropertyType = pt1,
                RoomCount = 3,
                SourceUrl = "google.com",
                Title = "Тестовая объект 1",
                TotalSpace = 200,
                Periods = pr2,
                Phone1 = "0663334433"

            });

            context.SaveChanges();

            context.PropertyObjects.Add(new PropertyObject()
            {
                Id = 2,
                City = c1,
                CityDistrict = cd2,
                ContactName = "Вася Пупкин 2",
                CreatedDate = DateTime.Now,
                //Currency = "UAH",
                IsActive = true,
                IsDeleted = 0,
                KitchenSpace = 40,
                LivingSpace = 80,
                Price = 7000,
                PropertyAction = pa1,
                PropertyDescription = "bla bla bla bla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla bla",
                PropertyType = pt1,
                RoomCount = 3,
                SourceUrl = "google.com",
                Title = "Тестовая объект 2",
                TotalSpace = 200,
                Periods = pr2,
                Phone1 = "0663334433"

            });
            context.SaveChanges();

            context.PropertyObjects.Add(new PropertyObject()
            {
                Id = 3,
                City = c2,
                CityDistrict = cd3,
                ContactName = "Вася Пупкин 3",
                CreatedDate = DateTime.Now,
                //Currency = "UAH",
                IsActive = true,
                IsDeleted = 0,
                KitchenSpace = 40,
                LivingSpace = 80,
                Price = 7000,
                PropertyAction = pa2,
                PropertyDescription = "bla dfgdfgd dfggd  bla bla dfgdfgd dfggd  blabla dfgdfgd dfggd  blabla dfgdfgd dfggd  blabla dfgdfgd dfggd  blabla dfgdfgd dfggd  bla",
                PropertyType = pt2,
                RoomCount = 3,
                SourceUrl = "google.com",
                Title = "Тестовая объект 3",
                TotalSpace = 200,
                Periods = pr2,
                Phone1 = "0663334433"

            });
            context.SaveChanges();

            context.PropertyObjects.Add(new PropertyObject()
            {
                Id = 3,
                City = c2,
                CityDistrict = cd4,
                ContactName = "Вася Пупкин 4",
                CreatedDate = DateTime.Now,
                //Currency = "UAH",
                IsActive = true,
                IsDeleted = 0,
                KitchenSpace = 90,
                LivingSpace = 180,
                Price = 7000,
                PropertyAction = pa2,
                PropertyDescription = "asd  asd as d asd as dd asd asd  asd as d asd as dd asd asd  asd as d asd as dd asd asd  asd as d asd as dd asd ",
                PropertyType = pt2,
                RoomCount = 3,
                SourceUrl = "google.com",
                Title = "Тестовая объект 4",
                TotalSpace = 500,
                Periods = pr2,
                Phone1 = "0663334433"

            });
            context.SaveChanges();


            



            bool switchCity = true;
            for (int i = 0; i < 10; i++)
            {
                var tmpObj = new PropertyObject()
                {
                    Id = i,
                    City = (switchCity ? c1 : c2),
                    CityDistrict = (switchCity ? cd1 : cd4),
                    ContactName = "Вася Пупкин 4" + i.ToString(),
                    CreatedDate = DateTime.Now,
                    //Currency = "UAH",
                    IsActive = true,
                    IsDeleted = 0,
                    KitchenSpace = 90,
                    LivingSpace = 180,
                    Price = 70 * i,
                    PropertyAction = pa2,
                    PropertyDescription = "asd  asd as d asd as dd asd asd  asd as d asd as dd asd asd  asd as d asd as dd asd asd  asd as d asd as dd asd ",
                    PropertyType = pt2,
                    RoomCount = i,
                    SourceUrl = "google.com",
                    Title = "Тестовая объект " + i.ToString(),
                    TotalSpace = 50 + i,
                    CountPhotos = i,
                    Periods = pr1,
                    Phone1 = "0663334433",
                    Currency = ct1

                };
                context.PropertyObjects.Add(tmpObj);
                context.Entry(tmpObj.Currency).State=EntityState.Unchanged;
                switchCity = !switchCity;
                context.SaveChanges();
                //System.Threading.Thread.Sleep(10);
            }

            #endregion
 */ 
            var nc1 = new NewsCategory() { Id = 1, Category = "Недвижимость" };

            #region news
            context.News.Add(new News()
           {
               Id = 1,
               Title = "Киевляне сдают в аренду почти 20 тысяч квартир",
               Content = @"По словам специалистов, наибольшим спросом пользовались однокомнатные квартиры (43% сделок), 38% приходится на двухкомнатные квартиры и 19% на объекты с тремя комнатами и более.
Стоит почеркнуть, что максимальное количество сделок (41%) приходится на квартиры до 4,5 тыс. грн. в месяц, 46% в ценовой категории от 4,5 до 7,0 тыс. гривен и 13% - в категории свыше 7,0 тыс. гривен.",
               NewsCategory = nc1,
               PrintTime = DateTime.Today
           });
            context.SaveChanges();

            context.News.Add(new News()
            {
                Id = 2,
                Title = "Киевляне скупают дешевые двушки",
                Content = @"Наибольшим спросом пользовались двухкомнатные квартиры (47% сделок), 41% приходится на однокомнатные квартиры и 12% на объекты с тремя комнатами и более.
Максимальное количество сделок (63%) приходится на квартиры до 100 тыс. долл., 24% в ценовой категории от 100 до 200 тыс. долларов и 13% - в категории свыше 200 тыс. долларов.",
                NewsCategory = nc1,
                PrintTime = DateTime.Today
            });
            context.SaveChanges();

            context.News.Add(new News()
            {
                Id = 3,
                Title = "В Киеве подешевели особняки",
                Content = @"За последний квартал – с сентября по декабрь 2012 года – средняя цена продажи частных домов в Киеве снизилась на 5,3% до 13 тыс. 488 гривен за кв. м, сообщают аналитики.
«Количество выставленных на продажу частных домов, в декабре 2012 г. по сравнению с сентябрем 2012 г., увеличилось на 31,1% до 1 тыс. 112 объектов. ",
                NewsCategory = nc1,
                PrintTime = DateTime.Today
            });
            context.SaveChanges();
            #endregion



            //AddPropertyTypes(context, "Продажа", pt1);
            //AddPropertyTypes(context, "Аренда по суточно", pt1);
            //AddPropertyTypes(context, "Аренда долгосрочно", pt1);


            //AddCityDistricts(context, c1, "Голосеевский р-н");
            //AddCityDistricts(context, c1, "Дарницкий р-н");
            //AddCityDistricts(context, c2, "Малиновский р-н");
            //AddCityDistricts(context, c2, "Приморский р-н");

        }

        private static void AddCityDistricts(RealtyDb context, City c1, string District)
        {
            context.CityDistricts.Add(new CityDisctict()
            {
                Id = 1,
                City = c1,
                District = District
            });
            context.SaveChanges();

        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
        }
    }
}