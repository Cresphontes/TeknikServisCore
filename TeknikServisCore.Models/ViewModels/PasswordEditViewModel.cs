using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeknikServisCore.Models.ViewModels
{
    public class PasswordEditViewModel
    {
        [Required, MinLength(6, ErrorMessage = "Eski şifreniz en az 6 karakter olmalıdır."), MaxLength(25, ErrorMessage = "Eski şifreniz en fazla 25 karakter olabilir."), DataType("Password"), DisplayName("Mevcut Şifre")]
        public string OldPassword { get; set; }
        [Required, MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır."), MaxLength(25, ErrorMessage = "Şifreniz en fazla 25 karakter olabilir."), DataType("Password"), DisplayName("Yeni Şifre")]
        public string NewPassword { get; set; }
        [Required, DataType("Password"), Compare("NewPassword"), DisplayName("Yeni Şifre Tekrar")]
        public string ConfirmNewPassword { get; set; }
    }
}
