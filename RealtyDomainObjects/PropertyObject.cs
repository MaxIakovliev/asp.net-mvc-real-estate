using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace RealtyDomainObjects
{
    [MetadataType(typeof(PropertyObject))]
    public class PropertyObject
    {        
        public int Id { get; set; }

        //[Required]
        public City City { get; set; }

        //[Required]
        public CityDisctict CityDistrict { get; set; }

        [Required]
        public PropertyType PropertyType { get; set; }//тип недвижимости

        //[Required]
        public PropertyAction PropertyAction { get; set; }//что делать с недвижимостью (продажа аренда)
        
        [DisplayName("Заголовок*:")]
        [Required(ErrorMessage="Это поле необходимо заполнить")]
        [StringLength(500,MinimumLength=10)]
        public string Title { get; set; }//заголовок

        [DisplayName("Текст*:")]
        [Required(ErrorMessage = "Это поле необходимо заполнить")]
        //[StringLength(2000, MinimumLength = 50)]
        [Column(DbType = "Text NOT NULL")]
        public string PropertyDescription { get; set; }//описание объявления
        
        [DisplayName("Количество комнат:")]
        [Required(ErrorMessage = "Заполните пожалуйста количество комнат")]
        public int RoomCount { get; set; }//колво комнат
        
        [DisplayName("Общая площадь:")]
        public double? TotalSpace { get; set; }//общая площадь
        
        [DisplayName("Жилая:")]
        public double? LivingSpace { get; set; }//жилая площадь
        
        [DisplayName("Кухня:")]
        public double? KitchenSpace { get; set; }//площадь кухни

        [DisplayName("Тип дома:")]
        public BuildingType BuildingTypeName { get; set; }//тип здания

        [DisplayName("Этаж:")]
        public int? Floor { get; set; }//этаж

        [DisplayName("Этажность:")]
        public int? CountFloors { get; set; }//этажность здания

        [DisplayName("Новострой:")]
        public bool IsNewBuilding { get; set; }//новострой

        [DisplayName("Балкон:")]
        public bool BalconAvailable { get; set; }//есть ли балкон

        [DisplayName("Площадь балкона:")]
        public double? BalconSpace { get; set; }//площадь балкона        

        [DisplayName("Застеклённость")]
        public bool isBalconGlassed { get; set; }//Застеклённость        

        [DisplayName("Контактное имя:")]
        [Required(ErrorMessage = "Заполните пожалуйста контактное имя")]
        public string ContactName { get; set; }//контактное имя        

        [Required]
        [DisplayName("Цена *:")]
        public double Price { get; set; }//цена (число)

        [DisplayName("Валюта")]
        public CurrencyType Currency { get; set; }//тип валюты
        public PriceForType PriceForTypeName { get; set; }//цена за м2/ за объект

        [DisplayName("Без комиссии")]
        public bool NoCommission { get; set; }//без коммисии
        [DisplayName("Телефон 1:")]
        [Required(ErrorMessage = "Это поле необходимо заполнить")]
        [StringLength(20, MinimumLength = 5)]
        public string Phone1 { get; set; }//телефон
        [DisplayName("Телефон 2:")]
        public string Phone2 { get; set; }
        [DisplayName("Телефон 3:")]
        public string Phone3 { get; set; }
        public Periods Periods { get; set; }
        public DateTime CreatedDate { get; set; }//дата создания

        [DisplayName("Сайт:")]
        public string SourceUrl { get; set; }//ссылка на сайт источник
        public User UserOwner { get; set; }//владелец объявления

        [DisplayName("Расстояние до города:")]
        public double DistanceToCity { get; set; }//ссылка на сайт источник
        
        [DisplayName("Санузел:")]
        public WCType WCType { get; set; }//тип сан узла 
        public int CountPhotos { get; set; }//количество фотографий

        [DisplayName("Тип недвижимости")]
        public CommercialPropertyType CommercialPropertyType { get; set; }//тип коммерческой недвижимости

        public ServiceType ServiceType { get; set; }//тип услуги
        
        public bool IsActive { get; set; }

        public int IsDeleted { get; set; }
        public DateTime DeletedDate { get; set; }
        public string LinkOfObjectGrab { get; set; }//линк отлкуда объявление спарсили

        public string ReasonOfDelete { get; set; } //причина удаления объекта (используется пауком при проверке данных)


    }
}
