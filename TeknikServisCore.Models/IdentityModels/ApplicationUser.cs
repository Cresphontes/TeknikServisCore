using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace TeknikServisCore.Models.IdentityModels
{
   
    public class ApplicationUser:IdentityUser
    {
     
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string ActivationCode { get; set; } 

    }
}
