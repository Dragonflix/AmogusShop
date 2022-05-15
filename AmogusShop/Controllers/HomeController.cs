using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmogusShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace AmogusShop.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        [Authorize]
        public IActionResult Huindex(string? id)
        {
            var user = db.Users.FirstOrDefault(i => i.Email == User.Identity.Name);
            var item = db.CartItems.FirstOrDefault(i => i.CartId == user.Id && i.Amogus.Id == int.Parse(id));
            if (item == null)
            {
                CartItem cartItem = new CartItem();
                cartItem.Amogus = db.Amogs.FirstOrDefault(i => i.Id == int.Parse(id));
                cartItem.CartId = user.Id;
                cartItem.Amount = 1;
                db.CartItems.Add(cartItem);
                db.SaveChanges();
            }
            else
            {
                item.Amount++;
                db.CartItems.Update(item);
                db.SaveChanges();
            }
            return Redirect("~/Home/Shop");
        }

        public IActionResult Shop()
        {
            ViewBag.Title = "Shop";
            if(!db.Amogs.Any())
            {
                db.Amogs.Add(new AMOGUS(1, "Susser", 0.55f, "SUS.png", "Regular amogus", 55f, "What?", 5));
                db.Amogs.Add(new AMOGUS(2, "Sus Master", 0.95f, "SusMaster.png", "Regular amogus", 55f, "What?", 5));
                db.Amogs.Add(new AMOGUS(3, "Boss of the Sus", 0.96f, "SusRemaster.png", "Regular amogus", 55, "What?", 5));
                db.Amogs.Add(new AMOGUS(4, "Mega Sus", 0.97f, "SUS.png", "Regular amogus", 55f, "What?", 5));
                db.Amogs.Add(new AMOGUS(5, "Ultra Impostor", 0.98f, "SUS.png", "Regular amogus", 55f, "What?", 5));
                db.Amogs.Add(new AMOGUS(6, "A M O G U S", 0.99f, "SUS.png", "Regular amogus", 55f, "What?", 5));
                db.SaveChanges();
            }
            if(!db.Users.Any())
            {
                User a = new User("admin", "saikon1976@gmail.com");
                a.Password = "admin";
                db.Users.Add(a);
                db.SaveChanges();
            }
            return View(db.Amogs.ToList());
        }

        [Authorize]
        public async Task<IActionResult> ShopCart()
        {
            ViewBag.Title = "Cart";
            var user = db.Users.FirstOrDefault(i => i.Email == User.Identity.Name);
            var IncTable = db.CartItems.Include(i => i.Amogus);
            var Result = IncTable.Where(i => i.CartId == user.Id).ToList();
            return View(Result);
        }

        [Authorize]
        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            var item = db.CartItems.FirstOrDefault(i => i.Id == id);
            if(item.Amount==1)
            {
                db.CartItems.Remove(item);
                await db.SaveChangesAsync();
            }
            else
            {
                item.Amount--;
                db.Update(item);
                await db.SaveChangesAsync();
            }
            return Redirect("~/Home/ShopCart");
        }
    }
}
