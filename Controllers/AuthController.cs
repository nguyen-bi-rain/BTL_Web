
using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Models;

namespace ShopQuanAo.Controllers
{
    public class AuthController : Controller
    {
        private readonly LTWEBContext db;

        public AuthController(LTWEBContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Account account)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                var data = db.Accounts.Where(a => a.Email.Equals(account.Email) && a.Password.Equals(account.Password)).FirstOrDefault();
                if (data != null)
                {
                    HttpContext.Session.SetString("Username", data.Username.ToString());
                    if (data.Idrole == 1)
                    {
                        return Redirect("/admin/products_admin/index");

                    }   
                    else
                    {
                        ViewBag.success = 1;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.error = "Invalid login ";
                    ViewBag.success = 0;
                    return RedirectToAction("Login", "Auth");
                }

            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Account account,string comfirmPassword)
        {
            if (ModelState.IsValid)
            {
                var check = db.Accounts.FirstOrDefault(a => a.Username == account.Username);
                if (check == null)
                {
                    account.Idrole = 2;
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    ViewBag.error = "User already exists";
                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            ViewBag.success = 0;
            return RedirectToAction("Login", "Auth");
        }

    }
}
