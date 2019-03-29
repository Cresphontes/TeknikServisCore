using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.IdentityModel.Tokens;
using TeknikServisCore.DAL;
using TeknikServisCore.Models.IdentityModels;
using TeknikServisCore.Models.ViewModels;

namespace TeknikServisCore.Web.Controllers
{
    public class ProfileController:Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public ProfileController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {

            var user = await _userManager.GetUserAsync(User);

            var model = Mapper.Map<ProfileEditViewModel>(user);
           

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileEditViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {

                var user = await _userManager.GetUserAsync(User);

                user = Mapper.Map<ApplicationUser>(model);
                _db.SaveChanges();

                TempData["Message"] = "Güncelleme işlemi başarılı.";

                return View(model);
            }
            catch (Exception)
            {
                TempData["Message"] = "Kaydetmek istediğiniz bölümdeki tüm alanları doldurunuz.";

                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(PasswordEditViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("Partials/_EditPasswordPartial",model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);


                if (model.OldPassword == user.Password)
                {
                    user.Password = model.NewPassword;
                    _db.SaveChanges();

                    TempData["Message"] = "Şifre güncelleme işlemi başarılı.";

                    return View("Partials/_EditPasswordPartial", model);
                }
                else
                {
                    TempData["Message"] = "Eski şifre ile yeni şifre uyuşmuyor";

                    return View("Partials/_EditPasswordPartial", model);
                }
                

            }
            catch (Exception e)
            { 
                TempData["Message"] = e;
                return View("Partials/_EditPasswordPartial", model);
            }

          
        }
     
    }
}
