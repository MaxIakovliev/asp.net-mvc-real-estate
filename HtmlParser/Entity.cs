using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlParser
{
   public class Entity
   {

       #region заполняются отдельно от
       public string City { get; set; }
        public string District { get; set; }
        public string propertyType { get; set; }
        public string propertyAction { get; set; }
        public string linkToOriginalObject { get; set; }//линк на объявление откуда спарсили
       #endregion

        public string title { get; set; }
        public string description { get; set; }
        public int roomsCount { get; set; }
        public double allSize { get; set; }
        public double livingSize { get; set; }
        public double kitchenSize { get; set; }

        public double houseSize { get; set; }//площадь частного дома
        public double houseGardenSize { get; set; }//площадь участка
        public double distanceToCity { get; set; }//расстояние до города
        public double houseCountFloor { get; set; }//колличество этажей в доме
        public double commercialObjectSize { get; set; }//площадь коммерческой недвижимости
        public int floor { get; set; }
        public int countFloors { get; set; }
        public string houseType { get; set; }
        public bool isNewBuilding { get; set; }
        public string wctype { get; set; }
        public int balconySize { get; set; }
        public bool balconyAvailable { get; set; }
        public bool isBalconyGlassed { get; set; }
        public int price { get; set; }
        public int SleepingPlaces { get; set; }
        public string CreatedDate { get; set; }
        public string currency { get; set; }
        public string priceFor { get; set; }//цена за месяц сутки кв.м и тд
        public bool noComission { get; set; }
        public string contactName { get; set; }
        public List<byte[]> photos { get; set; }
        public List<string> LinkToPhotos { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string phone3 { get; set; }
        public string userAvatarLink { get; set; }
        public byte[] userAvatar { get; set; }

        public string wall { get; set; }
        public string house_state { get; set; }
        //public string type_of_perekritiya { get; set; }
        public string flat_planirovka { get; set; }
        public string visota_potolka { get; set; }
        public string state_of_flat { get; set; }
        public string uteplenie { get; set; }


        public string state_of_building { get; set; }
        public string building_type_of_perekritiya { get; set; }
        public string building_type_of_roof { get; set; }
        public string characteristic_of_space_planirovka { get; set; }
        public string characteristic_of_space_some_feature { get; set; }
        public string characteristic_of_space_height { get; set; }
        public string characteristic_of_space_state { get; set; }
        public string characteristic_of_space_gips { get; set; }
        public string communication_gas { get; set; }
        public string communication_water { get; set; }
        public string communication_heating { get; set; }
        public string communication_water_heating { get; set; }
        public string communication_tv { get; set; }
        public string communication_internet { get; set; }
        public string communication_phone { get; set; }
        public string communication_conditioner { get; set; }
        public string owner_object_type { get; set; }
        public string nearest_metro { get; set; }
        public string nearest_metro_distance { get; set; }
        public string nearest_metro_howto_get { get; set; }
        public string center_city_distance { get; set; }
        public string center_city_howto_get { get; set; }
        public string doors_and_windows_indoor { get; set; }
        public string doors_and_windows_window_type { get; set; }
        public string doors_and_windows_count_glass { get; set; }
        public string school_distance { get; set; }
        public string school_howto_get { get; set; }
        public string childrengarden_distance { get; set; }
        public string childrengarden_howto_get { get; set; }
        public string policlinic_distance { get; set; }
        public string policlinic_howto_get { get; set; }
        public string market_distance { get; set; }
        public string market_howto_get { get; set; }
        public string relax_zone_type { get; set; }
        public string relax_zone_distance { get; set; }
        public string relax_zone_howto_get { get; set; }
        public string other_pravo_spbst_na_nedvig { get; set; }
        public string other_pravo_spbst_na_zemlyu { get; set; }
        public string other_docs_na_pravo_sobstv { get; set; }
        public string other_obstoyatelstva { get; set; }
        public string building_haracter_class_object { get; set; }
        public string building_haracter_building_year { get; set; }
        public string building_haracter_state { get; set; }
        public string haracter_bussiness_pravovaya_forma { get; set; }
        public string haracter_bussiness_srok_okupaemosti { get; set; }
        public string haracter_bussiness_average_income { get; set; }
        public string haracter_bussiness_debit_dolg { get; set; }
        public string haracter_bussiness_credit_dolg { get; set; }
        public string haracter_bussiness_count_empl { get; set; }
        public string haracter_bussiness_selling_part { get; set; }
        public string space_poshad_hoz_pomesh { get; set; }
        public string space_poshad_torg_zala { get; set; }
        public string space_poshad_sklad { get; set; }
        public string space_poshad_rampa { get; set; }
        public string parking_count_place { get; set; }
        public string parking_type { get; set; }
        public string parking_howto_get { get; set; }
        public string parking_distance { get; set; }
        public string offer_type { get; set; }

        public Entity()
        {
            this.isNewBuilding = false;
            this.balconyAvailable = false;
            this.isBalconyGlassed = false;
            this.noComission = false;

        }
    }
}
