using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HtmlParser
{
    [Serializable]
    public class Template
    {
        public string HOST { get; set; }
        public List<string> pages { get; private set; }
        public List<string> links { get; private set; }
        public List<string> title { get; private set; }
        public List<string> description { get; private set; }
        public List<string> roomsCount { get; private set; }
        public List<string> allSize { get; private set; }
        public List<string> livingSize { get; private set; }
        public List<string> kitchenSize { get; private set; }
        public List<string> floor { get; private set; }
        public List<string> houseType { get; private set; }
        public List<string> wctype { get; private set; }
        public List<string> balcony1 { get; private set; }
        public List<string> balcony2 { get; private set; }
        public List<string> price { get; private set; }
        public List<string> createddate { get; private set; }
        public List<string> commision { get; private set; }
        public List<string> contact { get; private set; }
        public List<string> photos { get; private set; }
        public List<string> phone { get; private set; }
        public List<string> avatar { get; private set; }
        public List<string> sleepingPlaces { get; private set; }
        public List<string> houseSize { get; private set; }
        public List<string> houseGardenSize { get; private set; }
        public List<string> distanceToCity { get; private set; }
        public List<string> houseCountFloor { get; private set; }
        public List<string> commercialObjectSize { get; private set; }


        public List<string> currency { get; private set; }
        public List<string> wall { get; private set; }
        //public List<string> house_state { get; private set; }
        //public List<string> type_of_perekritiya { get; private set; }
        public List<string> flat_planirovka { get; private set; }
        public List<string> visota_potolka { get; private set; }
        public List<string> state_of_flat { get; private set; }
        public List<string> uteplenie { get; private set; }
        public List<string> state_of_building { get; private set; }
        public List<string> building_type_of_perekritiya { get; private set; }
        public List<string> building_type_of_roof { get; private set; }
        public List<string> characteristic_of_space_planirovka { get; private set; }
        public List<string> characteristic_of_space_some_feature { get; private set; }
        public List<string> characteristic_of_space_height { get; private set; }
        public List<string> characteristic_of_space_state { get; private set; }
        public List<string> characteristic_of_space_gips { get; private set; }
        public List<string> communication_gas { get; private set; }
        public List<string> communication_water { get; private set; }
        public List<string> communication_heating { get; private set; }
        public List<string> communication_water_heating { get; private set; }
        public List<string> communication_tv { get; private set; }
        public List<string> communication_internet { get; private set; }
        public List<string> communication_phone { get; private set; }
        public List<string> communication_conditioner { get; private set; }
        public List<string> owner_object_type { get; private set; }
        public List<string> nearest_metro { get; private set; }
        public List<string> nearest_metro_distance { get; private set; }
        public List<string> nearest_metro_howto_get { get; private set; }
        public List<string> center_city_distance { get; private set; }
        public List<string> center_city_howto_get { get; private set; }
        public List<string> doors_and_windows_indoor { get; private set; }
        public List<string> doors_and_windows_window_type { get; private set; }
        public List<string> doors_and_windows_count_glass { get; private set; }
        public List<string> school_distance { get; private set; }
        public List<string> school_howto_get { get; private set; }
        public List<string> childrengarden_distance { get; private set; }
        public List<string> childrengarden_howto_get { get; private set; }
        public List<string> policlinic_distance { get; private set; }
        public List<string> policlinic_howto_get { get; private set; }
        public List<string> market_distance { get; private set; }
        public List<string> market_howto_get { get; private set; }
        public List<string> relax_zone_type { get; private set; }
        public List<string> relax_zone_distance { get; private set; }
        public List<string> relax_zone_howto_get { get; private set; }
        public List<string> other_pravo_spbst_na_nedvig { get; private set; }
        public List<string> other_pravo_spbst_na_zemlyu { get; private set; }
        public List<string> other_docs_na_pravo_sobstv { get; private set; }
        public List<string> other_obstoyatelstva { get; private set; }
        public List<string> building_haracter_class_object { get; private set; }
        public List<string> building_haracter_building_year { get; private set; }
        public List<string> building_haracter_state { get; private set; }
        public List<string> haracter_bussiness_pravovaya_forma { get; private set; }
        public List<string> haracter_bussiness_srok_okupaemosti { get; private set; }
        public List<string> haracter_bussiness_average_income { get; private set; }
        public List<string> haracter_bussiness_debit_dolg { get; private set; }
        public List<string> haracter_bussiness_credit_dolg { get; private set; }
        public List<string> haracter_bussiness_count_empl { get; private set; }
        public List<string> haracter_bussiness_selling_part { get; private set; }
        public List<string> space_poshad_hoz_pomesh { get; private set; }
        public List<string> space_poshad_torg_zala { get; private set; }
        public List<string> space_poshad_sklad { get; private set; }
        public List<string> space_poshad_rampa { get; private set; }
        public List<string> parking_distance { get; private set; }
        public List<string> parking_howto_get { get; private set; }
        public List<string> parking_type { get; private set; }
        public List<string> parking_count_place { get; private set; }
        public List<string> offer_type { get; private set; }

        public Template(string filePath)
        {
            var data = File.ReadAllLines(filePath);
            int countSteps = -1;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Contains("---") && char.IsNumber(data[i][0]))
                {
                    countSteps = Convert.ToInt32(data[i][0].ToString());
                    string fieldName = string.Empty;
                    for (int j = 1; j <= countSteps; j++)
                    {
                        string tmp = FillTemplate(data[++i]);
                        if (string.IsNullOrEmpty(fieldName))
                            fieldName = tmp;                       
                    }
                    if (string.IsNullOrEmpty(fieldName))
                        throw new Exception("field not found for-"+data[i]);
                    
                    var field = GetField(fieldName);
                    if(field==null)
                        throw new Exception("field verification fail for " + data[i]);
                    if (field.Count() != countSteps)
                        throw new Exception(string.Format("template error. Expected {0} but retrived {1}" + countSteps, field.Count()));

                    countSteps = -1;
                }
            }
        }

        private List<string> GetField(string fieldName)
        {
            switch (fieldName)
            {

                case Fields.OFFER_TYPE: return offer_type;
                case Fields.PARKING_COUNT_PLACE: return parking_count_place;
                case Fields.PARKING_HOWTO_GET: return parking_howto_get;
                case Fields.PARKING_DISTANCE: return parking_distance;
                case Fields.STATE_OF_BUILDING: return state_of_building;
                case Fields.BUILDING_TYPE_OF_PEREKRITIYA: return building_type_of_perekritiya;
                case Fields.BUILDING_TYPE_OF_ROOF: return building_type_of_roof;
                case Fields.CHARACTERISTIC_OF_SPACE_PLANIROVKA: return characteristic_of_space_planirovka;
                case Fields.CHARACTERISTIC_OF_SPACE_SOME_FEATURE: return characteristic_of_space_some_feature;
                case Fields.CHARACTERISTIC_OF_SPACE_HEIGHT: return characteristic_of_space_height;
                case Fields.CHARACTERISTIC_OF_SPACE_STATE: return characteristic_of_space_state;
                case Fields.CHARACTERISTIC_OF_SPACE_GIPS: return characteristic_of_space_gips;
                case Fields.COMMUNICATION_GAS: return communication_gas;
                case Fields.COMMUNICATION_WATER: return communication_water;
                case Fields.COMMUNICATION_HEATING: return communication_heating;
                case Fields.COMMUNICATION_WATER_HEATING: return communication_water_heating;
                case Fields.COMMUNICATION_TV: return communication_tv;
                case Fields.COMMUNICATION_INTERNET: return communication_internet;
                case Fields.COMMUNICATION_PHONE: return communication_phone;
                case Fields.COMMUNICATION_CONDITIONER: return communication_conditioner;
                case Fields.OWNER_OBJECT_TYPE: return owner_object_type;
                case Fields.NEAREST_METRO: return nearest_metro;
                case Fields.NEAREST_METRO_DISTANCE: return nearest_metro_distance;
                case Fields.NEAREST_METRO_HOWTO_GET: return nearest_metro_howto_get;
                case Fields.CENTER_CITY_DISTANCE: return center_city_distance;
                case Fields.CENTER_CITY_HOWTO_GET: return center_city_howto_get;
                case Fields.DOORS_AND_WINDOWS_INDOOR: return doors_and_windows_indoor;
                case Fields.DOORS_AND_WINDOWS_WINDOW_TYPE: return doors_and_windows_window_type;
                case Fields.DOORS_AND_WINDOWS_COUNT_GLASS: return doors_and_windows_count_glass;
                case Fields.SCHOOL_DISTANCE: return school_distance;
                case Fields.SCHOOL_HOWTO_GET: return school_howto_get;

                case Fields.CHILDRENGARDEN_DISTANCE: return childrengarden_distance;
                case Fields.CHILDRENGARDEN_HOWTO_GET: return childrengarden_howto_get;
                case Fields.POLICLINIC_DISTANCE: return policlinic_distance;
                case Fields.POLICLINIC_HOWTO_GET: return policlinic_howto_get;
                case Fields.MARKET_DISTANCE: return market_distance;
                case Fields.MARKET_HOWTO_GET: return market_howto_get;
                case Fields.RELAX_ZONE_TYPE: return relax_zone_type;
                case Fields.RELAX_ZONE_DISTANCE: return relax_zone_distance;
                case Fields.RELAX_ZONE_HOWTO_GET: return relax_zone_howto_get;
                case Fields.OTHER_PRAVO_SPBST_NA_NEDVIG: return other_pravo_spbst_na_nedvig;
                case Fields.OTHER_PRAVO_SPBST_NA_ZEMLYU: return other_pravo_spbst_na_zemlyu;
                case Fields.OTHER_DOCS_NA_PRAVO_SOBSTV: return other_docs_na_pravo_sobstv;
                case Fields.OTHER_OBSTOYATELSTVA: return other_obstoyatelstva;
                case Fields.BUILDING_HARACTER_CLASS_OBJECT: return building_haracter_class_object;
                case Fields.BUILDING_HARACTER_BUILDING_YEAR: return building_haracter_building_year;
                case Fields.BUILDING_HARACTER_STATE: return building_haracter_state;
                case Fields.HARACTER_BUSSINESS_PRAVOVAYA_FORMA: return haracter_bussiness_pravovaya_forma;
                case Fields.HARACTER_BUSSINESS_SROK_OKUPAEMOSTI: return haracter_bussiness_srok_okupaemosti;
                case Fields.HARACTER_BUSSINESS_AVERAGE_INCOME: return haracter_bussiness_average_income;
                case Fields.HARACTER_BUSSINESS_DEBIT_DOLG: return haracter_bussiness_debit_dolg;
                case Fields.HARACTER_BUSSINESS_CREDIT_DOLG: return haracter_bussiness_credit_dolg;
                case Fields.HARACTER_BUSSINESS_COUNT_EMPL: return haracter_bussiness_count_empl;
                case Fields.HARACTER_BUSSINESS_SELLING_PART: return haracter_bussiness_selling_part;
                case Fields.SPACE_POSHAD_HOZ_POMESH: return space_poshad_hoz_pomesh;
                case Fields.SPACE_POSHAD_TORG_ZALA: return space_poshad_torg_zala;
                case Fields.SPACE_POSHAD_SKLAD: return space_poshad_sklad;
                case Fields.SPACE_POSHAD_RAMPA: return space_poshad_rampa;
                case Fields.PARKING_TYPE: return parking_type;



                case Fields.CURRENCY:
                    return currency;
                case Fields.WALL:
                    return wall;
                //case Fields.HOUSE_STATE:
                //    return house_state;
                //case Fields.TYPE_OF_PEREKRITIYA:
                //    return type_of_perekritiya;
                case Fields.FLAT_PLANIROVKA:
                    return flat_planirovka;
                case Fields.VISOTA_POTOLKA:
                    return visota_potolka;
                case Fields.STATE_OF_FLAT:
                    return state_of_flat;
                case Fields.UTEPLENIE:
                    return uteplenie;


                case Fields.PAGES:
                    return pages;
                case Fields.LINKS:
                    return links;
                case Fields.TITLE:
                    return title;
                case Fields.DESCRIPTION:
                    return description;
                case Fields.ROOMSCOUNT:
                    return roomsCount;
                case Fields.ALLSIZE:
                    return allSize;
                case Fields.LIVINGSIZE:
                    return livingSize;
                case Fields.KITCHENSIZE:
                    return kitchenSize;
                case Fields.FLOOR:
                    return floor;
                case Fields.HOUSETYPE:
                    return houseType;
                case Fields.WCTYPE:
                    return wctype;
                case Fields.BALCONY1:
                    return balcony1;
                case Fields.BALCONY2:
                    return balcony2;
                case Fields.PRICE:
                    return price;
                case Fields.CREATEDDATE:
                    return createddate;
                case Fields.COMMISION:
                    return commision;
                case Fields.CONTACT:
                    return contact;
                case Fields.PHOTOS:
                    return photos;
                case Fields.PHONE:
                    return phone;
                case Fields.AVATAR:
                    return avatar;
                case Fields.SLEEPINGPLACES:
                    return sleepingPlaces;
                case Fields.HOUSESIZE:
                    return houseSize;
                case Fields.HOUSEGARDENSIZE:
                    return houseGardenSize;
                case Fields.DISTANCETOCITY:
                    return distanceToCity;
                case Fields.HOUSECOUNTFLOOR:
                    return houseCountFloor;
                case Fields.COMMERCIALOBJECTSIZE:
                    return commercialObjectSize;

                default:
                    return null;
            }
        }

        private string FillTemplate(string row)
        {
            string fieldName = string.Empty;
            if (!row.Contains('|'))
                throw new Exception("GetField -  '|' not found. Wrong Template format for row " + row);

            var field = row.Split('|');
            //Console.WriteLine(field[0]);
            switch (field[0])
            {

                case Fields.OFFER_TYPE:
                    if (offer_type == null) offer_type = new List<string>();
                    offer_type.Add(field[1]);
                    fieldName = Fields.OFFER_TYPE;
                    break;

                case Fields.PARKING_COUNT_PLACE:
                    if (parking_count_place == null) parking_count_place = new List<string>();
                    parking_count_place.Add(field[1]);
                    fieldName = Fields.PARKING_COUNT_PLACE;
                    break;

                case Fields.PARKING_TYPE:
                    if (parking_type == null) parking_type = new List<string>();
                    parking_type.Add(field[1]);
                    fieldName = Fields.PARKING_TYPE;
                    break;

                case Fields.PARKING_HOWTO_GET:
                    if (parking_howto_get == null) parking_howto_get = new List<string>();
                    parking_howto_get.Add(field[1]);
                    fieldName = Fields.PARKING_HOWTO_GET;
                    break;

                case Fields.PARKING_DISTANCE:
                    if (parking_distance == null) parking_distance = new List<string>();
                    parking_distance.Add(field[1]);
                    fieldName = Fields.PARKING_DISTANCE;
                    break;

                case Fields.NEAREST_METRO_HOWTO_GET:
                    if (nearest_metro_howto_get == null) nearest_metro_howto_get = new List<string>();
                    nearest_metro_howto_get.Add(field[1]);
                    fieldName = Fields.NEAREST_METRO_HOWTO_GET;
                    break;

                case Fields.SPACE_POSHAD_RAMPA:
                    if (space_poshad_rampa == null) space_poshad_rampa = new List<string>();
                    space_poshad_rampa.Add(field[1]);
                    fieldName = Fields.SPACE_POSHAD_RAMPA;
                    break;

                case Fields.SPACE_POSHAD_SKLAD:
                    if (space_poshad_sklad == null) space_poshad_sklad = new List<string>();
                    space_poshad_sklad.Add(field[1]);
                    fieldName = Fields.SPACE_POSHAD_SKLAD;
                    break;

                case Fields.SPACE_POSHAD_TORG_ZALA:
                    if (space_poshad_torg_zala == null) space_poshad_torg_zala = new List<string>();
                    space_poshad_torg_zala.Add(field[1]);
                    fieldName = Fields.SPACE_POSHAD_TORG_ZALA;
                    break;

                case Fields.SPACE_POSHAD_HOZ_POMESH:
                    if (space_poshad_hoz_pomesh == null) space_poshad_hoz_pomesh = new List<string>();
                    space_poshad_hoz_pomesh.Add(field[1]);
                    fieldName = Fields.SPACE_POSHAD_HOZ_POMESH;
                    break;

                case Fields.HARACTER_BUSSINESS_SELLING_PART:
                    if (haracter_bussiness_selling_part == null) haracter_bussiness_selling_part = new List<string>();
                    haracter_bussiness_selling_part.Add(field[1]);
                    fieldName = Fields.HARACTER_BUSSINESS_SELLING_PART;
                    break;

                case Fields.HARACTER_BUSSINESS_COUNT_EMPL:
                    if (haracter_bussiness_count_empl == null) haracter_bussiness_count_empl = new List<string>();
                    haracter_bussiness_count_empl.Add(field[1]);
                    fieldName = Fields.HARACTER_BUSSINESS_COUNT_EMPL;
                    break;

                case Fields.HARACTER_BUSSINESS_CREDIT_DOLG:
                    if (haracter_bussiness_credit_dolg == null) haracter_bussiness_credit_dolg = new List<string>();
                    haracter_bussiness_credit_dolg.Add(field[1]);
                    fieldName = Fields.HARACTER_BUSSINESS_CREDIT_DOLG;
                    break;

                case Fields.HARACTER_BUSSINESS_DEBIT_DOLG:
                    if (haracter_bussiness_debit_dolg == null) haracter_bussiness_debit_dolg = new List<string>();
                    haracter_bussiness_debit_dolg.Add(field[1]);
                    fieldName = Fields.HARACTER_BUSSINESS_DEBIT_DOLG;
                    break;

                case Fields.HARACTER_BUSSINESS_AVERAGE_INCOME:
                    if (haracter_bussiness_average_income == null) haracter_bussiness_average_income = new List<string>();
                    haracter_bussiness_average_income.Add(field[1]);
                    fieldName = Fields.HARACTER_BUSSINESS_AVERAGE_INCOME;
                    break;

                case Fields.HARACTER_BUSSINESS_SROK_OKUPAEMOSTI:
                    if (haracter_bussiness_srok_okupaemosti == null) haracter_bussiness_srok_okupaemosti = new List<string>();
                    haracter_bussiness_srok_okupaemosti.Add(field[1]);
                    fieldName = Fields.HARACTER_BUSSINESS_SROK_OKUPAEMOSTI;
                    break;

                case Fields.HARACTER_BUSSINESS_PRAVOVAYA_FORMA:
                    if (haracter_bussiness_pravovaya_forma == null) haracter_bussiness_pravovaya_forma = new List<string>();
                    haracter_bussiness_pravovaya_forma.Add(field[1]);
                    fieldName = Fields.HARACTER_BUSSINESS_PRAVOVAYA_FORMA;
                    break;

                case Fields.BUILDING_HARACTER_STATE:
                    if (building_haracter_state == null) building_haracter_state = new List<string>();
                    building_haracter_state.Add(field[1]);
                    fieldName = Fields.BUILDING_HARACTER_STATE;
                    break;

                case Fields.BUILDING_HARACTER_BUILDING_YEAR:
                    if (building_haracter_building_year == null) building_haracter_building_year = new List<string>();
                    building_haracter_building_year.Add(field[1]);
                    fieldName = Fields.BUILDING_HARACTER_BUILDING_YEAR;
                    break;

                case Fields.BUILDING_HARACTER_CLASS_OBJECT:
                    if (building_haracter_class_object == null) building_haracter_class_object = new List<string>();
                    building_haracter_class_object.Add(field[1]);
                    fieldName = Fields.BUILDING_HARACTER_CLASS_OBJECT;
                    break;

                case Fields.OTHER_OBSTOYATELSTVA:
                    if (other_obstoyatelstva == null) other_obstoyatelstva = new List<string>();
                    other_obstoyatelstva.Add(field[1]);
                    fieldName = Fields.OTHER_OBSTOYATELSTVA;
                    break;

                case Fields.OTHER_DOCS_NA_PRAVO_SOBSTV:
                    if (other_docs_na_pravo_sobstv == null) other_docs_na_pravo_sobstv = new List<string>();
                    other_docs_na_pravo_sobstv.Add(field[1]);
                    fieldName = Fields.OTHER_DOCS_NA_PRAVO_SOBSTV;
                    break;

                case Fields.OTHER_PRAVO_SPBST_NA_ZEMLYU:
                    if (other_pravo_spbst_na_zemlyu == null) other_pravo_spbst_na_zemlyu = new List<string>();
                    other_pravo_spbst_na_zemlyu.Add(field[1]);
                    fieldName = Fields.OTHER_PRAVO_SPBST_NA_ZEMLYU;
                    break;

                case Fields.OTHER_PRAVO_SPBST_NA_NEDVIG:
                    if (other_pravo_spbst_na_nedvig == null) other_pravo_spbst_na_nedvig = new List<string>();
                    other_pravo_spbst_na_nedvig.Add(field[1]);
                    fieldName = Fields.OTHER_PRAVO_SPBST_NA_NEDVIG;
                    break;

                case Fields.RELAX_ZONE_HOWTO_GET:
                    if (relax_zone_howto_get == null) relax_zone_howto_get = new List<string>();
                    relax_zone_howto_get.Add(field[1]);
                    fieldName = Fields.RELAX_ZONE_HOWTO_GET;
                    break;

                case Fields.RELAX_ZONE_DISTANCE:
                    if (relax_zone_distance == null) relax_zone_distance = new List<string>();
                    relax_zone_distance.Add(field[1]);
                    fieldName = Fields.RELAX_ZONE_DISTANCE;
                    break;

                case Fields.RELAX_ZONE_TYPE:
                    if (relax_zone_type == null) relax_zone_type = new List<string>();
                    relax_zone_type.Add(field[1]);
                    fieldName = Fields.RELAX_ZONE_TYPE;
                    break;

                case Fields.MARKET_HOWTO_GET:
                    if (market_howto_get == null) market_howto_get = new List<string>();
                    market_howto_get.Add(field[1]);
                    fieldName = Fields.MARKET_HOWTO_GET;
                    break;

                case Fields.MARKET_DISTANCE:
                    if (market_distance == null) market_distance = new List<string>();
                    market_distance.Add(field[1]);
                    fieldName = Fields.MARKET_DISTANCE;
                    break;

                case Fields.POLICLINIC_HOWTO_GET:
                    if (policlinic_howto_get == null) policlinic_howto_get = new List<string>();
                    policlinic_howto_get.Add(field[1]);
                    fieldName = Fields.POLICLINIC_HOWTO_GET;
                    break;

                case Fields.POLICLINIC_DISTANCE:
                    if (policlinic_distance == null) policlinic_distance = new List<string>();
                    policlinic_distance.Add(field[1]);
                    fieldName = Fields.POLICLINIC_DISTANCE;
                    break;

                case Fields.CHILDRENGARDEN_HOWTO_GET:
                    if (childrengarden_howto_get == null) childrengarden_howto_get = new List<string>();
                    childrengarden_howto_get.Add(field[1]);
                    fieldName = Fields.CHILDRENGARDEN_HOWTO_GET;
                    break;

                case Fields.CHILDRENGARDEN_DISTANCE:
                    if (childrengarden_distance == null) childrengarden_distance = new List<string>();
                    childrengarden_distance.Add(field[1]);
                    fieldName = Fields.CHILDRENGARDEN_DISTANCE;
                    break;

                case Fields.SCHOOL_HOWTO_GET:
                    if (school_howto_get == null) school_howto_get = new List<string>();
                    school_howto_get.Add(field[1]);
                    fieldName = Fields.SCHOOL_HOWTO_GET;
                    break;

                case Fields.SCHOOL_DISTANCE:
                    if (school_distance == null) school_distance = new List<string>();
                    school_distance.Add(field[1]);
                    fieldName = Fields.SCHOOL_DISTANCE;
                    break;

                case Fields.DOORS_AND_WINDOWS_COUNT_GLASS:
                    if (doors_and_windows_count_glass == null) doors_and_windows_count_glass = new List<string>();
                    doors_and_windows_count_glass.Add(field[1]);
                    fieldName = Fields.DOORS_AND_WINDOWS_COUNT_GLASS;
                    break;

                case Fields.DOORS_AND_WINDOWS_WINDOW_TYPE:
                    if (doors_and_windows_window_type == null) doors_and_windows_window_type = new List<string>();
                    doors_and_windows_window_type.Add(field[1]);
                    fieldName = Fields.DOORS_AND_WINDOWS_WINDOW_TYPE;
                    break;

                case Fields.DOORS_AND_WINDOWS_INDOOR:
                    if (doors_and_windows_indoor == null) doors_and_windows_indoor = new List<string>();
                    doors_and_windows_indoor.Add(field[1]);
                    fieldName = Fields.DOORS_AND_WINDOWS_INDOOR;
                    break;

                case Fields.CENTER_CITY_DISTANCE:
                    if (center_city_distance == null) center_city_distance = new List<string>();
                    center_city_distance.Add(field[1]);
                    fieldName = Fields.CENTER_CITY_DISTANCE;
                    break;

                case Fields.CENTER_CITY_HOWTO_GET:
                    if (center_city_howto_get == null) center_city_howto_get = new List<string>();
                    center_city_howto_get.Add(field[1]);
                    fieldName = Fields.CENTER_CITY_HOWTO_GET;
                    break;

                case Fields.NEAREST_METRO_DISTANCE:
                    if (nearest_metro_distance == null) nearest_metro_distance = new List<string>();
                    nearest_metro_distance.Add(field[1]);
                    fieldName = Fields.NEAREST_METRO_DISTANCE;
                    break;

                case Fields.NEAREST_METRO:
                    if (nearest_metro == null) nearest_metro = new List<string>();
                    nearest_metro.Add(field[1]);
                    fieldName = Fields.NEAREST_METRO;
                    break;

                case Fields.OWNER_OBJECT_TYPE:
                    if (owner_object_type == null) owner_object_type = new List<string>();
                    owner_object_type.Add(field[1]);
                    fieldName = Fields.OWNER_OBJECT_TYPE;
                    break;


                case Fields.COMMUNICATION_CONDITIONER:
                    if (communication_conditioner == null) communication_conditioner = new List<string>();
                    communication_conditioner.Add(field[1]);
                    fieldName = Fields.COMMUNICATION_CONDITIONER;
                    break;

                case Fields.COMMUNICATION_PHONE:
                    if (communication_phone == null) communication_phone = new List<string>();
                    communication_phone.Add(field[1]);
                    fieldName = Fields.COMMUNICATION_PHONE;
                    break;

                case Fields.COMMUNICATION_INTERNET:
                    if (communication_internet == null) communication_internet = new List<string>();
                    communication_internet.Add(field[1]);
                    fieldName = Fields.COMMUNICATION_INTERNET;
                    break;

                case Fields.COMMUNICATION_TV:
                    if (communication_tv == null) communication_tv = new List<string>();
                    communication_tv.Add(field[1]);
                    fieldName = Fields.COMMUNICATION_TV;
                    break;

                case Fields.COMMUNICATION_WATER_HEATING:
                    if (communication_water_heating == null) communication_water_heating = new List<string>();
                    communication_water_heating.Add(field[1]);
                    fieldName = Fields.COMMUNICATION_WATER_HEATING;
                    break;

                case Fields.COMMUNICATION_HEATING:
                    if (communication_heating == null) communication_heating = new List<string>();
                    communication_heating.Add(field[1]);
                    fieldName = Fields.COMMUNICATION_HEATING;
                    break;

                case Fields.COMMUNICATION_WATER:
                    if (communication_water == null) communication_water = new List<string>();
                    communication_water.Add(field[1]);
                    fieldName = Fields.COMMUNICATION_WATER;
                    break;

                case Fields.COMMUNICATION_GAS:
                    if (communication_gas == null) communication_gas = new List<string>();
                    communication_gas.Add(field[1]);
                    fieldName = Fields.COMMUNICATION_GAS;
                    break;

                case Fields.CHARACTERISTIC_OF_SPACE_GIPS:
                    if (characteristic_of_space_gips == null) characteristic_of_space_gips = new List<string>();
                    characteristic_of_space_gips.Add(field[1]);
                    fieldName = Fields.CHARACTERISTIC_OF_SPACE_GIPS;
                    break;

                case Fields.CHARACTERISTIC_OF_SPACE_STATE:
                    if (characteristic_of_space_state == null) characteristic_of_space_state = new List<string>();
                    characteristic_of_space_state.Add(field[1]);
                    fieldName = Fields.CHARACTERISTIC_OF_SPACE_STATE;
                    break;


                case Fields.CHARACTERISTIC_OF_SPACE_HEIGHT:
                    if (characteristic_of_space_height == null) characteristic_of_space_height = new List<string>();
                    characteristic_of_space_height.Add(field[1]);
                    fieldName = Fields.CHARACTERISTIC_OF_SPACE_HEIGHT;
                    break;

                case Fields.CHARACTERISTIC_OF_SPACE_SOME_FEATURE:
                    if (characteristic_of_space_some_feature == null) characteristic_of_space_some_feature = new List<string>();
                    characteristic_of_space_some_feature.Add(field[1]);
                    fieldName = Fields.CHARACTERISTIC_OF_SPACE_SOME_FEATURE;
                    break;

                case Fields.CHARACTERISTIC_OF_SPACE_PLANIROVKA:
                    if (characteristic_of_space_planirovka == null) characteristic_of_space_planirovka = new List<string>();
                    characteristic_of_space_planirovka.Add(field[1]);
                    fieldName = Fields.CHARACTERISTIC_OF_SPACE_PLANIROVKA;
                    break;

                case Fields.BUILDING_TYPE_OF_ROOF:
                    if (building_type_of_roof == null) building_type_of_roof = new List<string>();
                    building_type_of_roof.Add(field[1]);
                    fieldName = Fields.BUILDING_TYPE_OF_ROOF;
                    break;

                case Fields.BUILDING_TYPE_OF_PEREKRITIYA:
                    if (building_type_of_perekritiya == null) building_type_of_perekritiya = new List<string>();
                    building_type_of_perekritiya.Add(field[1]);
                    fieldName = Fields.BUILDING_TYPE_OF_PEREKRITIYA;
                    break;

                case Fields.STATE_OF_BUILDING:
                    if (state_of_building == null) state_of_building = new List<string>();
                    state_of_building.Add(field[1]);
                    fieldName = Fields.STATE_OF_BUILDING;
                    break;

                case Fields.UTEPLENIE:
                    if (uteplenie == null) uteplenie = new List<string>();
                    uteplenie.Add(field[1]);
                    fieldName = Fields.UTEPLENIE;
                    break;

                case Fields.STATE_OF_FLAT:
                    if (state_of_flat == null) state_of_flat = new List<string>();
                    state_of_flat.Add(field[1]);
                    fieldName = Fields.STATE_OF_FLAT;
                    break;

                case Fields.VISOTA_POTOLKA:
                    if (visota_potolka == null) visota_potolka = new List<string>();
                    visota_potolka.Add(field[1]);
                    fieldName = Fields.VISOTA_POTOLKA;
                    break;

                case Fields.FLAT_PLANIROVKA:
                    if (flat_planirovka == null) flat_planirovka = new List<string>();
                    flat_planirovka.Add(field[1]);
                    fieldName = Fields.FLAT_PLANIROVKA;
                    break;

                //case Fields.TYPE_OF_PEREKRITIYA:
                //    if (type_of_perekritiya == null) type_of_perekritiya = new List<string>();
                //    type_of_perekritiya.Add(field[1]);
                //    fieldName = Fields.TYPE_OF_PEREKRITIYA;
                //    break;

                //case Fields.HOUSE_STATE:
                //    if (house_state == null) house_state = new List<string>();
                //    house_state.Add(field[1]);
                //    fieldName = Fields.HOUSE_STATE;
                //    break;

                case Fields.WALL:
                    if (wall == null) wall = new List<string>();
                    wall.Add(field[1]);
                    fieldName = Fields.WALL;
                    break;

                case Fields.CURRENCY:
                    if (currency == null) currency = new List<string>();
                    currency.Add(field[1]);
                    fieldName = Fields.CURRENCY;
                    break;

                case Fields.PAGES:
                    if (pages == null) pages = new List<string>();
                    pages.Add(field[1]);
                    fieldName = Fields.PAGES;
                    break;
                case Fields.LINKS:
                    if (links == null) links = new List<string>();
                    links.Add(field[1]);
                    fieldName = Fields.LINKS;
                    break;
                case Fields.TITLE:
                    if (title == null) title = new List<string>();
                    title.Add(field[1]);
                    fieldName = Fields.TITLE;
                    break;
                case Fields.DESCRIPTION:
                    if (description == null) description = new List<string>();
                    description.Add(field[1]);
                    fieldName = Fields.DESCRIPTION;
                    break;
                case Fields.ROOMSCOUNT:
                    if (roomsCount == null) roomsCount = new List<string>();
                    roomsCount.Add(field[1]);
                    fieldName = Fields.ROOMSCOUNT;
                    break;
                case Fields.ALLSIZE:
                    if (allSize == null) allSize = new List<string>();
                    allSize.Add(field[1]);
                    fieldName = Fields.ALLSIZE;
                    break;
                case Fields.LIVINGSIZE:
                    if (livingSize == null) livingSize = new List<string>();
                    livingSize.Add(field[1]);
                    fieldName = Fields.LIVINGSIZE;
                    break;
                case Fields.KITCHENSIZE:
                    if (kitchenSize == null) kitchenSize = new List<string>();
                    kitchenSize.Add(field[1]);
                    fieldName = Fields.KITCHENSIZE;
                    break;
                case Fields.FLOOR:
                    if (floor == null) floor = new List<string>();
                    floor.Add(field[1]);
                    fieldName = Fields.FLOOR;
                    break;
                case Fields.HOUSETYPE:
                    if (houseType == null) houseType = new List<string>();
                    houseType.Add(field[1]);
                    fieldName = Fields.HOUSETYPE;
                    break;
                case Fields.WCTYPE:
                    if (wctype == null) wctype = new List<string>();
                    wctype.Add(field[1]);
                    fieldName = Fields.WCTYPE;
                    break;
                case Fields.BALCONY1:
                    if (balcony1 == null) balcony1 = new List<string>();
                    balcony1.Add(field[1]);
                    fieldName = Fields.BALCONY1;
                    break;
                case Fields.BALCONY2:
                    if (balcony2 == null) balcony2 = new List<string>();
                    balcony2.Add(field[1]);
                    fieldName = Fields.BALCONY2;
                    break;
                case Fields.PRICE:
                    if (price == null) price = new List<string>();
                    price.Add(field[1]);
                    fieldName = Fields.PRICE;
                    break;
                case Fields.CREATEDDATE:
                    if (createddate == null) createddate = new List<string>();
                    createddate.Add(field[1]);
                    fieldName = Fields.CREATEDDATE;
                    break;
                case Fields.COMMISION:
                    if (commision == null) commision = new List<string>();
                    commision.Add(field[1]);
                    fieldName = Fields.COMMISION;
                    break;
                case Fields.CONTACT:
                    if (contact == null) contact = new List<string>();
                    contact.Add(field[1]);
                    fieldName = Fields.CONTACT;
                    break;
                case Fields.AVATAR:
                    if (avatar == null) avatar = new List<string>();
                    avatar.Add(field[1]);
                    fieldName = Fields.AVATAR;
                    break;
                case Fields.PHOTOS:
                    if (photos == null) photos = new List<string>();
                    photos.Add(field[1]);
                    fieldName = Fields.PHOTOS;
                    break;
                case Fields.PHONE:
                    if (phone == null) phone = new List<string>();
                    phone.Add(field[1]);
                    fieldName = Fields.PHONE;
                    break;
                case Fields.SLEEPINGPLACES:
                    if (sleepingPlaces == null) sleepingPlaces = new List<string>();
                    sleepingPlaces.Add(field[1]);
                    fieldName = Fields.SLEEPINGPLACES;
                    break;

                case Fields.HOUSESIZE:
                    if (houseSize == null) houseSize = new List<string>();
                    houseSize.Add(field[1]);
                    fieldName = Fields.HOUSESIZE;
                    break;

                case Fields.HOUSEGARDENSIZE:
                    if (houseGardenSize == null) houseGardenSize = new List<string>();
                    houseGardenSize.Add(field[1]);
                    fieldName = Fields.HOUSEGARDENSIZE;
                    break;

                case Fields.DISTANCETOCITY:
                    if (distanceToCity == null) distanceToCity = new List<string>();
                    distanceToCity.Add(field[1]);
                    fieldName = Fields.DISTANCETOCITY;
                    break;

                case Fields.HOUSECOUNTFLOOR:
                    if (houseCountFloor == null) houseCountFloor = new List<string>();
                    houseCountFloor.Add(field[1]);
                    fieldName = Fields.HOUSECOUNTFLOOR;
                    break;
                case Fields.COMMERCIALOBJECTSIZE:
                    if (commercialObjectSize == null) commercialObjectSize = new List<string>();
                    commercialObjectSize.Add(field[1]);
                    fieldName = Fields.COMMERCIALOBJECTSIZE;
                    break;



                default:
                    break;
            }

            if (string.IsNullOrEmpty(fieldName))
                throw new Exception("GetField- field not found for " + row);

            return fieldName;

        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
