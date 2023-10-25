using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;
using ShopQuanAo.Models;
using System.Security.Principal;

namespace ShopQuanAo.Controllers
{
    public class CartController : Controller
    {
        private readonly LTWEBContext db;
        List<Product> cartItems;
        List<int> quantity;
        CartViewModel viewModel;
        private readonly IToastNotification _toastNotification;
        public CartController(LTWEBContext db, IToastNotification toastNotification)
        {
            this.db = db;
            viewModel = new CartViewModel();
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
           
            if (HttpContext.Session.Get<List<Product>>("Cart") == null)
            {
                viewModel.CartItems = new List<Product>();
                viewModel.Quantity = new List<int>();

            }
            else
            {
                viewModel.CartItems = HttpContext.Session.Get<List<Product>>("Cart");
                viewModel.Quantity = HttpContext.Session.Get<List<int>>("Quantity");
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            if(HttpContext.Session.GetString("Username") == null)
            {
                _toastNotification.AddWarningToastMessage("Please Login To Using Cart!");
                return RedirectToAction("Index", "Home");
            }
            if (HttpContext.Session.Get<List<Product>>("Cart") == null)
            {
                viewModel.CartItems = new List<Product>();
                viewModel.Quantity = new List<int>();
            }
            else
            {
                viewModel.CartItems = HttpContext.Session.Get<List<Product>>("Cart");
                viewModel.Quantity = HttpContext.Session.Get<List<int>>("Quantity");
            }

            // Thêm sản phẩm vào danh sách giỏ hàng
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                viewModel.CartItems.Add(product);
                viewModel.Quantity.Add(1);
            }

            // Lưu danh sách sản phẩm vào session
            HttpContext.Session.Set("Cart", viewModel.CartItems);
            HttpContext.Session.Set("Quantity", viewModel.Quantity);

            _toastNotification.AddSuccessToastMessage("Add To Cart Success!");

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Index", "Cart");
            }
            viewModel.CartItems = HttpContext.Session.Get<List<Product>>("Cart");
            viewModel.Quantity = HttpContext.Session.Get<List<int>>("Quantity");
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Increase(int id)
        {
            viewModel.CartItems = HttpContext.Session.Get<List<Product>>("Cart");
            viewModel.Quantity = HttpContext.Session.Get<List<int>>("Quantity");
            Console.WriteLine(viewModel.CartItems[0].Id);
            Console.WriteLine(id);

            for(int i=0; i < viewModel.CartItems.Count ; i++)
            {
                if (viewModel.CartItems[i].Id == id - 1) viewModel.Quantity[i] = viewModel.Quantity[i] + 1;
            }
            HttpContext.Session.Set("Cart", viewModel.CartItems);
            HttpContext.Session.Set("Quantity", viewModel.Quantity);
            Console.WriteLine(viewModel.Quantity[0]);
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult Decrease(int id)
        {
            viewModel.CartItems = HttpContext.Session.Get<List<Product>>("Cart");
            viewModel.Quantity = HttpContext.Session.Get<List<int>>("Quantity");
            Console.WriteLine(viewModel.CartItems[0].Id);
            Console.WriteLine(id);

            for (int i = 0; i < viewModel.CartItems.Count; i++)
            {
                if (viewModel.CartItems[i].Id == id + 1 && viewModel.Quantity[i] > 1) viewModel.Quantity[i] = viewModel.Quantity[i] - 1;
            }
            HttpContext.Session.Set("Cart", viewModel.CartItems);
            HttpContext.Session.Set("Quantity", viewModel.Quantity);
            Console.WriteLine(viewModel.Quantity[0]);
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult Order(string FullName, string Email, string Phone, string Address, decimal Total)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            // Lấy thông tin giỏ hàng từ Session
            viewModel.CartItems = HttpContext.Session.Get<List<Product>>("Cart");
            viewModel.Quantity = HttpContext.Session.Get<List<int>>("Quantity");

            // Tạo một đối tượng đơn hàng và lưu thông tin
            var order = new Order
            {
                Idaccount = HttpContext.Session.Get<int>("UserId"), // Thay thế bằng Id tài khoản của người dùng (nếu có hệ thống tài khoản)
                Createdate = DateTime.Now,
                Fullname = FullName,
                Email = Email,
                Phone = Phone,
                Address = Address,
                Total = CalculateTotal(viewModel.CartItems, viewModel.Quantity) // Tính tổng số tiền
            };

            // Lưu đơn hàng vào cơ sở dữ liệu
            db.Orders.Add(order);
            db.SaveChanges();

            // Lưu chi tiết đơn hàng (sản phẩm trong đơn hàng)
            for (int i = 0; i < viewModel.CartItems.Count; i++)
            {
                var orderDetail = new Orderdetail
                {
                    Idorder = order.Id,
                    Idproduct = viewModel.CartItems[i].Id,
                    Price = viewModel.CartItems[i].Price,
                    Quantity = viewModel.Quantity[i],
                    Subtotal = viewModel.CartItems[i].Price * viewModel.Quantity[i]
                };

                db.Orderdetails.Add(orderDetail);
            }
            db.SaveChanges();

            // Xóa giỏ hàng sau khi đặt hàng
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("Quantity");
            _toastNotification.AddSuccessToastMessage("Order Success!");
            return RedirectToAction("Index", "Cart");
        }
        // Hàm tính tổng tiền đơn hàng
        private decimal CalculateTotal(List<Product> products, List<int> quantities)
        {
            decimal total = 0;
            for (int i = 0; i < products.Count; i++)
            {
                total += (decimal)products[i].Price * quantities[i];
            }
            return total;
        }
        //[HttpPost]
        //public IActionResult teat(Bind[jfhjhvjhdj] product, IFormFile image)
        //{
        //    // tạo product

        //    // lấy tên ảnh gán vào product

        //    // lưu ảnh vào foler chỉ định;

        //    // wwwroot/image/upload/ + product.image;

        //    return RedirectToAction("Index", "Cart");
        //}
    }
}
