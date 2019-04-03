using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TeknikServisCore.BLL.Helpers;
using TeknikServisCore.BLL.Services;
using TeknikServisCore.DAL;
using TeknikServisCore.Models.Enums;
using TeknikServisCore.Models.IdentityModels;
using TeknikServisCore.Models.ViewModels;

namespace TeknikServisCore.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;


        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {

                TempData["Message"] = ("Kayıt işlemi başarısız oldu. Girdiğiniz Bilgileri kontrol ediniz.");
                return View("Register", model);
            }
            else
            {
                var user = Mapper.Map<ApplicationUser>(model);

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await CreateRoles();

                    if (_userManager.Users.Count() == 1)
                    {
                        await _userManager.AddToRoleAsync(user, IdentityRoles.Admin.ToString());
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, IdentityRoles.User.ToString());
                    }

                    string siteUrl = HttpContext.Request.GetEncodedUrl().Substring(0, 23);

                    var emailService = new EmailService();

                    var body = $"Merhaba <b>{user.Name} {user.Surname}</b><br>Hesabınızı aktif etmek için aşağıdaki linke tıklayınız<br> <a href='{siteUrl}/Account/Activation?code={user.ActivationCode}'>Aktivasyon Linki</a>";

                    await emailService.SendAsync(new MessageViewModel()
                    {
                        Body = body,
                        Subject = "Sitemize Hoşgeldiniz."

                    }, user.Email);

                    return RedirectToAction("Login");
                }
                else
                {
                    var errMsg = "";

                    foreach (var error in result.Errors)
                    {
                        errMsg += error.Code;
                    }


                    TempData["Message"] = (errMsg);
                    return View("Register", model);
                }


            }

        }

        private async Task CreateRoles()
        {
            var roleNames = Enum.GetNames(typeof(IdentityRoles));

            foreach (var roleName in roleNames)
            {
                if (!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    await _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = roleName
                    });
                }
            }
        }


        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            try
            {

                //var user = _userManager.Users.FirstOrDefault(x => x.UserName == model.UserName);
                ApplicationUser user =await _userManager.FindByNameAsync(model.UserName);

                if (user.EmailConfirmed == true)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Register", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                        return View(model);
                    }
                }
                else
                {
                    TempData["Message"] = "Hesabınız aktif edilmemiştir. Mailinizi kontrol ediniz.";
                    return View(model);
                }


            }
            catch (Exception)
            {

                TempData["Message"] = "Giriş işleminde bir hata oluştu.";
                return View(model);
            }

            
            
        }



        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Activation(string code)
        {

            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.ActivationCode == code);

                if (user != null)
                {
                    if (user.EmailConfirmed == true)
                    {
                        TempData["Message"] = "<span class='alert alert-danger'>Bu Hesap Daha Önce Aktif edilmiştir.</span>";
                    }
                    else
                    {

                        user.EmailConfirmed = true;
                        _dbContext.SaveChanges();
                        TempData["Message"] = "<span class='alert alert-succcess'>Aktivasyon işlemi başarılı.</span>";
                    }
                }
                else
                {

                    ViewBag.Message = "<span class='alert alert-danger'>Aktivasyon işleminiz başarısız.</span>";

                }
            }

            catch (Exception)
            {

                TempData["Message"] = $"<span class='alert alert-danger'>Aktivasyon işleminde bir hata oluştu.</span>";
            }


            return RedirectToAction("Login");
        }
    }
}