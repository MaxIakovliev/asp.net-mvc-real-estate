using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RealtyDomainObjects
{
    public class User
    {
        public int Id { get; set; }
        [DisplayName("Контактное имя*:")]
        [Required]
        public string UserName { get; set; }
        [DisplayName("Телефон 1*:")]
        [Required]
        public string Phone1 { get; set; }
        [DisplayName("Телефон 2:")]
        public string Phone2 { get; set; }
        [DisplayName("Телефон 3:")]
        public string Phone3 { get; set; }
        public string url { get; set; }
        [DisplayName("Добавлять автоматически при подаче объявления:")]
        public bool ApplyToAd { get; set; }
        public int MembershipId { get; set; }
        public string tmp{ get; set; }


    }
}
