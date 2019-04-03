using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeknikServisCore.Models.ViewModels
{
  public  class ProfileViewModel
    {
        [Required, MinLength(3, ErrorMessage = "Kullanıcı adınız 3 karakterden fazla olmalıdır."), MaxLength(15, ErrorMessage = "Kullanıcı adınız 15 karakterden az olmalıdır."), DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required, MinLength(2, ErrorMessage = "İsminiz 2 karakterden fazla olmalıdır."), MaxLength(25, ErrorMessage = "İsminiz 25 karakterden az olmalıdır."), DisplayName("İsim")]
        public string Name { get; set; }
        [Required, MinLength(2, ErrorMessage = "Soyisminiz 2 karakterden fazla olmalıdır."), MaxLength(25, ErrorMessage = "Soyisminiz 25 karakterden az olmalıdır."
         ), DisplayName("Soyisim")]
        public string Surname { get; set; }
        [Required, DataType("Email"), DisplayName("Email")]
        public string Email { get; set; }
        [Required, StringLength(11, ErrorMessage = "Telefon numaranız 11 haneli olmalıdır."), DisplayName("Telefon")]
        public string PhoneNumber { get; set; }
        [Required, DisplayName("Doğum Tarihi")]
        public DateTime BirthDate { get; set; }

        [Required, StringLength(100, ErrorMessage = "Adresiniz en fazla 100 karakter olabilir."), DisplayName("Adres")]
        public string Address { get; set; }
    }
}
