using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RealtyDomainObjects;

namespace WebRealty.Models
{
    public class RealtyDb : DbContext
    {
        public RealtyDb()
            : base("RealtyDb")
        {

        }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityDisctict> CityDistricts { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<PropertyAction> PropertyActions { get; set; }
        public DbSet<PropertyObject> PropertyObjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ObjectImages> ObjectImages { get; set; }
        public DbSet<Periods> Periods { get; set; }
        public DbSet<BuildingType> BuildingTypes { get; set; }
        public DbSet<WCType> WCTypes { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<PriceForType> PriceForTypes { get; set; }
        public DbSet<LandCommunication> LandCommunications { get; set; }
        public DbSet<LandFunction> LandFunctions { get; set; }
        public DbSet<LandFunctionToObj> LandFunctionToObjs { get; set; }
        public DbSet<LandCommunicationToObj> LandCommunicationToObjs { get; set; }
        public DbSet<CommercialPropertyType> CommercialPropertyTypes { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<PropertyStat> PropertyStats { get; set; }
    }
}