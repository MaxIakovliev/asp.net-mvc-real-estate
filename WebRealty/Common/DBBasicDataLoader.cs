using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.Common
{
    public class DBBasicDataLoader
    {
        private string sourceFolder = null;
        private RealtyDb context = null;
        public DBBasicDataLoader(string sourceFolder, RealtyDb context)
        {
            this.sourceFolder = sourceFolder;
            this.context = context;
        }

        public void LoadCities()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\city.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                var tmp = item.Split('\t');
                if (tmp != null && tmp.Length != 3) continue;
                this.context.Cities.Add(new RealtyDomainObjects.City()
                {
                    CityName = tmp[0],
                    CityNamePadej = tmp[1],
                    important=(tmp[2]=="1"?true:false),
                    Id = ++i
                });
                this.context.SaveChanges();
            }

        }

        public void LoadDistricts()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\district.txt");
            if (data == null)
                return;

            var city = (from s in this.context.Cities
                        select s).ToList<City>();

            int i = 0;
            foreach (var item in data)
            {
                var tmp = item.Split('\t');
                if (tmp != null && tmp.Length != 2) continue;


                if (city == null) continue;
                City selectedCity = null;
                foreach (var c in city)
                {
                    if (c.CityName.ToLower()==tmp[0].ToLower())
                    {
                        selectedCity = c;
                        break;
                    }
                }
                if (selectedCity == null) continue;
                this.context.CityDistricts.Add(new CityDisctict()
                {
                    City = selectedCity,
                    District = tmp[1],
                    Id = ++i
                });
                this.context.Entry(selectedCity).State = System.Data.EntityState.Unchanged;
                this.context.SaveChanges();
            }

        }

        public void LoadPropertyTypes()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\PropertyType.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                var tmp = item.Split('\t');
                if (tmp != null && tmp.Length != 3) continue;
                this.context.PropertyTypes.Add(new RealtyDomainObjects.PropertyType()
                {
                    Id=++i,
                    important=(tmp[2]=="1"?true:false),
                    PropertyTypeName=tmp[0],
                    PropertyTypeNamePadej=tmp[1]
                });
                this.context.SaveChanges();
            }

        }

        public void LoadPropertyActions()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\PropertyAction.txt");
            if (data == null)
                return;

            var propertyType = (from s in this.context.PropertyTypes
                        select s).ToList<PropertyType>();

            int i = 0;
            foreach (var item in data)
            {
                var tmp = item.Split('\t');
                if (tmp != null && tmp.Length != 2) continue;


                if (propertyType == null) continue;
                PropertyType selectedpropertyType = null;
                foreach (var c in propertyType)
                {
                    if (c.PropertyTypeName.ToLower() == tmp[0].ToLower())
                    {
                        selectedpropertyType = c;
                        break;
                    }
                }
                if (selectedpropertyType == null) continue;
                this.context.PropertyActions.Add(new PropertyAction()
                {                    
                    Id = ++i,
                    PropertyType=selectedpropertyType,
                    PropertyActionName=tmp[1]
                });
                this.context.Entry(selectedpropertyType).State = System.Data.EntityState.Unchanged;
                this.context.SaveChanges();
            }

        }

        public void LoadPeriod()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\Periods.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {                                
                this.context.Periods.Add(new RealtyDomainObjects.Periods()
                {
                    PeriodName=item,
                    Id = ++i
                });
                this.context.SaveChanges();
            }
        }

        public void LoadBuildingType()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\BuildingType.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                this.context.BuildingTypes.Add(new RealtyDomainObjects.BuildingType()
                {
                    BuildingTypeName = item,
                    Id = ++i
                });
                this.context.SaveChanges();
            }
        }

        public void LoadPriceForType()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\PriceForType.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                this.context.PriceForTypes.Add(new RealtyDomainObjects.PriceForType()
                {
                    PriveForTypeName = item,
                    Id = ++i
                });
                this.context.SaveChanges();
            }
        }
        public void LoadCurrencyType()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\CurrencyType.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                this.context.CurrencyTypes.Add(new RealtyDomainObjects.CurrencyType()
                {
                    CurrencyTypeName = item,
                    Id = ++i
                });
                this.context.SaveChanges();
            }
        }
        public void LoadWCType()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\WCType.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                this.context.WCTypes.Add(new RealtyDomainObjects.WCType()
                {
                    WCTypeName = item,
                    Id = ++i
                });
                this.context.SaveChanges();
            }
        }
        public void LoadLandCommunication()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\LandCommunication.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                this.context.LandCommunications.Add(new RealtyDomainObjects.LandCommunication()
                {
                    LandCommunicationsName = item,
                    Id = ++i,
                    guid = Guid.NewGuid()
                });
                this.context.SaveChanges();
            }
        }
        public void LoadLandFunction()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\LandFunction.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                this.context.LandFunctions.Add(new RealtyDomainObjects.LandFunction()
                {
                    LandFunctionName = item,
                    Id = ++i,
                    guid = Guid.NewGuid()
                });
                this.context.SaveChanges();
            }
        }
        public void LoadCommercialPropertyType()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\CommercialPropertyType.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                this.context.CommercialPropertyTypes.Add(new RealtyDomainObjects.CommercialPropertyType()
                {
                    CommercialPropertyTypeName= item,
                    Id = ++i
                });
                this.context.SaveChanges();
            }
        }
        public void LoadServiceType()
        {
            var data = File.ReadAllLines(this.sourceFolder + "\\ServiceType.txt");
            if (data == null)
                return;
            int i = 0;
            foreach (var item in data)
            {
                this.context.ServiceTypes.Add(new RealtyDomainObjects.ServiceType()
                {
                    ServiceTypeName = item,
                    Id = ++i
                });
                this.context.SaveChanges();
            }
        }

        public void LoadFakeData()
        {
            var cities = (from s in this.context.Cities
                          select s).ToList<City>();

            var district = (from s in this.context.CityDistricts
                            select s).ToList<CityDisctict>();

            var propertyType = (from s in this.context.PropertyTypes
                                select s).ToList<PropertyType>();

            var propertyActions = (from s in this.context.PropertyActions
                                   select s).ToList<PropertyAction>();

            var periods = (from s in this.context.Periods
                           select s).ToList<Periods>();

            var priceForType = (from s in this.context.PriceForTypes
                                select s).ToList<PriceForType>();

            var currencyType = (from s in this.context.CurrencyTypes
                                select s).ToList<CurrencyType>();

            var wCType=(from s in this.context.WCTypes
                        select s).ToList<WCType>();

             var buildingType=(from s in this.context.BuildingTypes
                        select s).ToList<BuildingType>();

            for (int i = 0; i < 50; i++)
            {
                this.context.PropertyObjects.Add(new PropertyObject() {
                BalconAvailable=true,
                BalconSpace=i+10,
                BuildingTypeName = buildingType[0],
                City=cities[0],
                CityDistrict=district[0],
                CommercialPropertyType=null,
                ContactName="test "+i.ToString(),
                CountFloors=i+10,
                CountPhotos=0,
                CreatedDate=DateTime.Today,
                Currency=currencyType[0],
                Floor=i,
                IsActive=true,
                isBalconGlassed=true,
                IsNewBuilding=true,
                KitchenSpace=i+4,
                LivingSpace=i+20,
                NoCommission=true,
                Periods=periods[0],
                Phone1="0908098098080809",
                Phone2="909-09-09-9-090-9",
                Price=i*10+10,
                PriceForTypeName=priceForType[0],
                PropertyAction=propertyActions[0],
                PropertyDescription = "here is some description here is some description here is some description here is some description here is some description here is some description here is some description ",
                PropertyType=propertyType[0],
                RoomCount=i,
                Title="some title here test test "+i.ToString(),
                TotalSpace=i*20,
                WCType=wCType[0]                
                });

                this.context.Entry(buildingType[0]).State = System.Data.EntityState.Unchanged;
                this.context.Entry(cities[0]).State = System.Data.EntityState.Unchanged;
                this.context.Entry(district[0]).State = System.Data.EntityState.Unchanged;
                this.context.Entry(currencyType[0]).State = System.Data.EntityState.Unchanged;
                this.context.Entry(periods[0]).State = System.Data.EntityState.Unchanged;
                this.context.Entry(priceForType[0]).State = System.Data.EntityState.Unchanged;
                this.context.Entry(propertyActions[0]).State = System.Data.EntityState.Unchanged;
                this.context.Entry(propertyType[0]).State = System.Data.EntityState.Unchanged;
                this.context.Entry(wCType[0]).State = System.Data.EntityState.Unchanged;

                this.context.SaveChanges();
            }
        }
    }
}