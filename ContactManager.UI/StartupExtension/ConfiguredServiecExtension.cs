using ContactsManager.Core.Domain.IndentityEntities;
using CountriesService;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContract;
using ServiceContract;
using Services;

namespace CRUDExample.StartupExtension
{
    public static class ConfiguredServiecExtension 
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection service,IConfiguration configuration)
        {
            service.AddHttpLogging(options => options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
| Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties);


            //it adds controllers and views as servicess
            service.AddControllersWithViews(options => {
                //options.Filters.Add<ResponseActiomFilter>(5);
                var logger = service.BuildServiceProvider().GetRequiredService<ILogger<ResponseActiomFilter>>();
                options.Filters.Add(new ResponseActiomFilter(logger, "My-Key-From-Global", "My-Value-FRom-Global", 2));
            });

            //Add services inside the Ioc Containers
            service.AddScoped<ICountryRepository, CountryRepository>();
            service.AddScoped<IPersonRepository, PersonRepository>();
            service.AddScoped<ICountriesService, CountriesServiceM>();
            service.AddScoped<IPersonGetterService, PersonsGetterServiceWIthFewChange>();
           // service.AddScoped<IPersonGetterService, PersonGetterServiceChild>();//LSP voilation
            service.AddScoped<PersonGetterService, PersonGetterService>();
            service.AddScoped<IPersonDeleteService, PersonDeleterService>();
            service.AddScoped<IPersonAdderService, PersonAdderService>();
            service.AddScoped<IPersonSortedService, PersonSorterService>();
            service.AddScoped<IPersonUpdaterService, PersonUpdaterService>();
            service.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));//scoped service
                //Enable identity in this project
            });
            
            service.AddIdentity<ApplicationUser, ApplicationRole>((options)=>{
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3;

            }).
                AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()//default token provider means email otp,sms otp etc
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()//this is for users tables
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();


            service.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//authorization filter for all the action methods 

            });
            service.ConfigureApplicationCookie(options =>
            {
               options.LoginPath = "/Account/Login";
            });
            return service;
        }
    }
}
