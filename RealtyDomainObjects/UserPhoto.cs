using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RealtyDomainObjects
{
    public class UserPhoto
    {
        public int Id { get; set; }
        [Required]
        public int MembershipId { get; set; }
        [DisplayName("Подпись")]
        public string Title { get; set; }//подпись пользователя
        public byte[] img { get; set; }
    }
}
