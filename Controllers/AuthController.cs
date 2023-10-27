
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using ShopQuanAo.Models;
using System.Security.Principal;

namespace ShopQuanAo.Controllers
{
    public class AuthController : Controller
    {
        private readonly LTWEBContext db;
        private readonly IToastNotification _toastNotification;

        public AuthController(LTWEBContext db, IToastNotification toastNotification)
        {
            this.db = db;
            _toastNotification = toastNotification;
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
                    HttpContext.Session.Set("UserId", data.Id);
                    if (data.Idrole == 1)
                    {
                        return Redirect("/admin/products_admin/index");

                    }   
                    else
                    {
                        ViewBag.success = 1;
                        _toastNotification.AddSuccessToastMessage("Login Success!");
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
        public async Task<IActionResult> Register(Account account)
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
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewBag.success = 0;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile()
        {
            var account = db.Accounts.FirstOrDefault(a => a.Username == HttpContext.Session.GetString("Username"));
            return View(account);
        }

    }
}
