using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeknikServisCore.Models.ViewModels
{
    public class ProfileEditViewModel
    {
        public ProfileViewModel ProfileViewModel { get; set; }

        public PasswordEditViewModel PasswordEditViewModel { get; set; }
    }
}
