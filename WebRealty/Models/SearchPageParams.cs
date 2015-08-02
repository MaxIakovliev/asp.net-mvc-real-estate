using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRealty.Models
{
    public class SearchPageParams
    {
        public int iPropertyType;
        public int iPropertyTypeAction;
        public int iPropertyTypeCities;
        public int iPropertyTypeCityDistrict;
        public int countObjectFound;
        public int countPagesFound;
        public int currentPage;
        public int navigatePage;
        public int SortType;
        public int ShowType;

        public int commercialPropertyType;
        public int countFloors;
        public int distToCityMin;
        public int distToCityMax;
        public int buildingType;
        public int wcType;
        public int roomCount;
        public int sizemin;
        public int sizemax;

    }
}