using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeknikServisCore.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required, MinLength(3, ErrorMessage = "Kullanıcı adınız 3 karakterden fazla olmalıdır."), MaxLength(15, ErrorMessage = "Kullanıcı adınız 15 karakterden az olmalıdır."), DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required, MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır."), MaxLength(25, ErrorMessage = "Şifreniz en fazla 25 karakter olabilir."), DataType("Password"), DisplayName("Şifre")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
