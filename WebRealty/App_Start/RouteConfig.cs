using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebRealty
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "NewAdRoute",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "ad",
                    action="NewAd",
                    id=UrlParameter.Optional
                });

            routes.MapRoute(
                name: "PopulateSearchBox",
                url: "{controller}/PopulateSearchBox/PropertyType/PropertyTypeAction/PropertyTypeCities/PropertyTypeCityDistrict",
                defaults: new
                {
                    controller = "Home",
                    PropertyTypeAction = "-1",
                    PropertyTypeCities = "-1",
                    PropertyTypeCityDistrict = "-1"
                });


            routes.MapRoute(
                name: "ShowSearchPath",
                url: "{controller}/{SearchByParams}/{PropertyType}/{PropertyTypeAction}/{PropertyTypeCities}/{PropertyTypeCityDistrict}/{NavigatePage}/{sortType}",
                defaults: new
                {
                    controller = "Home",
                    PropertyTypeAction = "-1",
                    PropertyTypeCities = "-1",
                    PropertyTypeCityDistrict = "-1",
                    NavigatePage = "1",
                    sortType = "1"
                });


            routes.MapRoute(
                name: "SearchByParams",
                url: "{controller}/{SearchByParams}/{PropertyType}/{PropertyTypeAction}/{PropertyTypeCities}/{PropertyTypeCityDistrict}/{NavigatePage}/{sortType}",
                defaults: new
                {
                    controller = "Home",
                    SearchByParams= "1",
                    PropertyTypeAction = "-1",
                    PropertyTypeCities = "-1",
                    PropertyTypeCityDistrict = "-1",
                    NavigatePage="1",
                    sortType="1"
                });

            routes.MapRoute(
                name: "SearchBox",
                url: "{controller}/{action}/PropertyType/PropertyTypeAction/PropertyTypeCities/PropertyTypeCityDistrict",
                defaults: new
                {
                    controller = "SearchBox",
                    action = "SearchByParams",
                    PropertyType="1",
                    PropertyTypeAction = "-1",
                                PropertyTypeCities = "-1",
                                PropertyTypeCityDistrict = "-1"
                }
            );
        }
    }
}