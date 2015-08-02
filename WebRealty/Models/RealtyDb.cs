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

        #region some additional params
        public DbSet<Building_type_of_roof> Building_type_of_roofs { get; set; }
        public DbSet<Characteristic_of_space_gips> Characteristic_of_space_gipss { get; set; }
        public DbSet<Characteristic_of_space_some_feature> Characteristic_of_space_some_features { get; set; }
        public DbSet<Characteristic_of_space_state> Characteristic_of_space_states { get; set; }
        public DbSet<Communication_conditioner> Communication_conditioners { get; set; }
        public DbSet<Communication_gas> Communication_gass { get; set; }
        public DbSet<Communication_heating> Communication_heatings { get; set; }
        public DbSet<Communication_internet> Communication_internets { get; set; }
        public DbSet<Communication_phone> Communication_phones { get; set; }
        public DbSet<Communication_tv> Communication_tvs { get; set; }
        public DbSet<Communication_water> Communication_waters { get; set; }
        public DbSet<Communication_water_heating> Communication_water_heatings { get; set; }
        public DbSet<Doors_and_windows_count_glass> Doors_and_windows_count_glasses { get; set; }
        public DbSet<Doors_and_windows_indoor> Doors_and_windows_indoors { get; set; }
        public DbSet<Doors_and_windows_window_type> Doors_and_windows_window_types { get; set; }
        public DbSet<Flat_planirovka> Flat_planirovkas { get; set; }
        public DbSet<Other_docs_na_pravo_sobstv> Other_docs_na_pravo_sobstvs { get; set; }
        public DbSet<Other_obstoyatelstva> Other_obstoyatelstvas { get; set; }
        public DbSet<Other_pravo_spbst_na_nedvig> Other_pravo_spbst_na_nedvigs { get; set; }
        public DbSet<Other_pravo_spbst_na_zemlyu> Other_pravo_spbst_na_zemlyus { get; set; }
        public DbSet<State_of_building> State_of_buildings { get; set; }

        #endregion


        #region компании
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencyToPhone> AgencyToPhones { get; set; }
        public DbSet<Realtor> Realtors { get; set; }
        public DbSet<RealtorToPhone> RealtorToPhones { get; set; }
        #endregion

    }
}