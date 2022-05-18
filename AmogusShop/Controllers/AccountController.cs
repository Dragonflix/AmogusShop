using AmogusShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BCrypt.Net;

namespace AmogusShop.Controllers
{
    public class AccountController : Controller
    {
        ApplicationContext db;
        public AccountController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login(string? alert)
        {
            ViewBag.Title = "Log In";
            ViewBag.Alert = alert;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl)
        {
            User? user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user.EmailConfirmed!=1)
            {
                return RedirectToRoute("default", new { controller = "Account", action = "Login", alert = "Email wasnt confirmed!" });
            }
            if (user == null || BCrypt.Net.BCrypt.Verify(password, user.Password) == false)
            {
                return RedirectToRoute("default", new { controller = "Account", action = "Login", alert = "Incorrect username or password!" });
            }
            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, user.Email) 
            };
           
            var claimsIdentity = new ClaimsIdentity(claims, "Login");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/Home/Index");
        }

        public async Task<IActionResult> Register(string? alert)
        {
            ViewBag.Title = "Register";
            ViewBag.Alert = alert;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string cpassword, string username)
        {
            if (db.Users.FirstOrDefault(u => u.Email == email) == null)
            {
                if (password.Length<8)
                    return RedirectToRoute("default", new { controller = "Account", action = "Register", alert = "Password should contain more than 7 symbols!" });
                if(password!=cpassword)
                    return RedirectToRoute("default", new { controller = "Account", action = "Register", alert = "Passwords do not match!" });
                User user = new User(username, email);
                string hash = BCrypt.Net.BCrypt.HashPassword(password);
                user.Password = hash;
                Random rand = new Random();
                user.EmailConfirmed = rand.Next(2, 2000000000);
                db.Users.Add(user);
                await db.SaveChangesAsync();
                MailSender.SendVerificationEmail($"https://localhost:7114/Account/ConfirmEmail?id={user.Id}&token={user.EmailConfirmed}", user.Email);
                return Redirect("~/Account/Login");
            }
            else
            {
                return RedirectToRoute("default", new { controller = "Account", action = "Register", alert = "User with this email already exists!" });
            }
        }

        public async Task<IActionResult> ConfirmEmail(int id, int token)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == id&&u.EmailConfirmed==token);
            if(user == null)
            {
                ViewBag.Result = "Verification Unsuccessful";
            }
            else
            {
                user.EmailConfirmed = 1;
                db.Update(user);
                await db.SaveChangesAsync();
                ViewBag.Result = "Verification Successful";
            }
            ViewBag.Title = "Email confirmation";
            return View();
        }
    }
}
