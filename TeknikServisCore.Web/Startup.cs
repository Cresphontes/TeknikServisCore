using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using AutoMapper;
using TeknikServisCore.BLL.Helpers;
using TeknikServisCore.DAL;
using TeknikServisCore.Models.IdentityModels;
using TeknikServisCore.Models.ViewModels;

namespace TeknikServisCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = false;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddAutoMapper();
            services.AddMvc();

            Mapper.Initialize(cfg => MapConfig(cfg));

            //services.AddWebOptimizer();
            //services.AddWebOptimizer(pipeline =>
            //{
            //    pipeline.AddJavaScriptBundle("/bundle/scripts.js", "/Themes/blue-free/js/bootstrap.min.js")
            //    .AdjustRelativePaths()
            //    .UseContentRoot();
            //});
            //services.AddWebOptimizer(pipeline =>
            //{
            //    pipeline.AddCssBundle("/bundle/styles.css", "/Themes/blue-free/css/bootstrap.min.css")
            //    .AdjustRelativePaths()
            //    .UseContentRoot();
            //});

            //services.AddIdentity<ApplicationUser,ApplicationRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void MapConfig(IMapperConfigurationExpression cfg)
        {

            cfg.CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.ActivationCode, opt => opt.MapFrom(x => StringHelpers.GetCode()));
            cfg.CreateMap<ApplicationUser, RegisterViewModel>()
                .ForMember(dest => dest.ActivationCode, opt => opt.MapFrom(x => x.ActivationCode));

            //cfg.CreateMap<ProfileViewModel, ApplicationUser>()
            //    .ForMember(dest => dest.NormalizedUserName,
            //        opt => opt.MapFrom(x => x.Username.ToUpper()));
            //    //.ForMember(dest => dest.Password,
            //    //opt => opt.MapFrom(x => x.PasswordEditViewModel.NewPassword));

            cfg.CreateMap<ApplicationUser, ProfileViewModel>();
            cfg.CreateMap<ProfileViewModel, ApplicationUser>().ForMember(dest => dest.NormalizedUserName,
                opt => opt.MapFrom(x => x.UserName.ToUpper()));
            cfg.CreateMap<ApplicationUser, PasswordEditViewModel>().ReverseMap();
            cfg.CreateMap<ApplicationUser, ProfileViewModel>().ReverseMap();






        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseAuthentication();

            //app.UseWebOptimizer();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();
        }


    }
}