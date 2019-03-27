using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeknikServisCore.Models.IdentityModels
{
    public class ApplicationRole:IdentityRole
    {
        [StringLength(50)]
        public string Description { get; set; }
    }
}
