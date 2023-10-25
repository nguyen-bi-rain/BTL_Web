using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ShopQuanAo.Models;
using System.Security.Principal;

namespace ShopQuanAo.Controllers
{
    public class CartController : Controller
    {
        private readonly LTWEBContext db;
        List<Product> cartItems;
        public CartController(LTWEBContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
           
            if (HttpContext.Session.Get<List<Product>>("Cart") == null)
            {
                cartItems = new List<Product>();
                var product = db.Products.FirstOrDefault(p => p.Id == 1);
                cartItems.Add(product);
            }
            else
            {
                cartItems = HttpContext.Session.Get<List<Product>>("Cart");
            }

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            if (HttpContext.Session.Get<List<Product>>("Cart") == null)
            {
                cartItems = new List<Product>();
            }
            else
            {
                cartItems = HttpContext.Session.Get<List<Product>>("Cart");
            }

            // Thêm sản phẩm vào danh sách giỏ hàng
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                cartItems.Add(product);
            }

            // Lưu danh sách sản phẩm vào session
            HttpContext.Session.Set("Cart", cartItems);

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
