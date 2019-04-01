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

            if (user == null)
            {
                return RedirectToAction("Login");
            }

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

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model1 = Mapper.Map<ProfileEditViewModel>(user);

            if (!ModelState.IsValid)
            {
                return View("EditProfile", model1);
            }

            try
            {
                
                if (model.OldPassword == user.Password)
                {
                    user.Password = model.NewPassword;
                    user.ConfirmPassword = model.ConfirmNewPassword;
                    _db.SaveChanges();

                    TempData["Message"] = "Şifre güncelleme işlemi başarılı.";

                    return View("EditProfile", model1);
                }
                else
                {
                    TempData["Message"] = "Mevcut şifrenizi yanlış girdiniz.";

                    return View("EditProfile", model1);
                }
                

            }
            catch (Exception e)
            { 
                TempData["Message"] = e;
                return View("EditProfile", model1);
            }

          
        }
     
    }
}
