using ContactsManager.Core.Domain.IndentityEntities;
using ContactsManager.Core.Dto;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContactManager.UI.Controllers
{
    [AllowAnonymous]//all the action methods inside this controller are accessible without authentication
    //[Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid == false)
            {
                //if model state is not valid then return the same view with error messages
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage);
                return View(registerDTO);
            }
            ApplicationUser user = new ApplicationUser//creating the obj of newly created user
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email,//UserName is mandatory field in IdentityUser
                PersonName = registerDTO.PersonName
            };
            IdentityResult result = await _userManager.CreateAsync(user,registerDTO.Password);//creating the user in Identity Database it will store the password in the hashcode
            

            if (result.Succeeded)
            {
                //Sign in
                await _signInManager.SignInAsync(user,isPersistent:false);

                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return View(registerDTO);
            }
            //store user registration details into Identity Database
        }
        [HttpGet]
        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto,string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage);
                return View(loginDto);
            }
            var result=await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false,lockoutOnFailure:false);
            if (result.Succeeded)
            {
                //if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                //{
                //    return LocalRedirect(ReturnUrl);
                //}
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            ModelState.AddModelError("Login","Invalid Email or Password");
            return View(loginDto);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }
        public async Task<IActionResult> IsEmailAlreadyRegister(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
