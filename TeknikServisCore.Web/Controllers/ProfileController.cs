using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.IdentityModel.Tokens;
using TeknikServisCore.DAL;
using TeknikServisCore.Models.IdentityModels;
using TeknikServisCore.Models.ViewModels;

namespace TeknikServisCore.Web.Controllers
{
    public class ProfileController : Controller
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

            ProfileEditViewModel model = new ProfileEditViewModel()
            {
                PasswordEditViewModel =
                {
                    NewPassword =  "",
                    OldPassword = "",
                    ConfirmNewPassword = ""
                },
                ProfileViewModel =
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    Address = user.Address,
                    Email = user.Email,
                    BirthDate = user.BirthDate,
                    PhoneNumber = user.PhoneNumber

                }
            };

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

                //user = Mapper.Map<ApplicationUser>(model);
                user.UserName = model.ProfileViewModel.UserName;
                user.NormalizedUserName = model.ProfileViewModel.UserName.ToUpper();
                user.Address = model.ProfileViewModel.Address;
                user.PhoneNumber = model.ProfileViewModel.PhoneNumber;
                user.BirthDate = model.ProfileViewModel.BirthDate;
                user.Email = model.ProfileViewModel.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Güncelleme işlemi başarılı.";
                }
                else
                {
                    TempData["Message"] = "Güncelleme işlemi başarısız.";
                }


                return View(model);
            }
            catch (Exception)
            {
                TempData["Message"] = "Güncelleme işlemi sırasında bir hata oluştu.";

                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(ProfileEditViewModel model)
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }


            if (!ModelState.IsValid)
            {
                return View("EditProfile");
            }

            try
            {

                var result = await _userManager.ChangePasswordAsync(user, model.PasswordEditViewModel.OldPassword,
                    model.PasswordEditViewModel.ConfirmNewPassword);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Şifre güncelleme işlemi başarılı.";
                }
                else
                {
                    TempData["Message"] = "Şifre güncelleme işlemi başarısız.";

                }


                return View("EditProfile");

            }
            catch (Exception e)
            {
                TempData["Message"] = e;


                return View("EditProfile");
            }


        }

    }
}
