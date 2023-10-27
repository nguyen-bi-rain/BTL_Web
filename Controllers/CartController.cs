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
            ViewBag.cartNumber = viewModel.CartItems.Count;
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

            // chech trùng sản phẩm
            for(int i=0; i< viewModel.CartItems.Count; i++)
            {
                if (viewModel.CartItems[i].Id == id)
                {
                    _toastNotification.AddAlertToastMessage("Product existed in Cart!");
                    return RedirectToAction("Index", "Home");
                }
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

            return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int change)
        {
            // Retrieve cart items and quantities from the session
            var cartItems = HttpContext.Session.Get<List<Product>>("Cart");
            var quantities = HttpContext.Session.Get<List<int>>("Quantity");

            // Find the product in the cart
            var product = cartItems.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                // Find the index of the product in the cart
                var index = cartItems.IndexOf(product);

                // Update the quantity
                if(quantities[index] >= 1)
                {
                    quantities[index] += change;
                }
                
                // Save the updated quantities to the session
                HttpContext.Session.Set("Quantity", quantities);
            }
            // update total
            decimal sum = 0;
            for(int i = 0; i < cartItems.Count; i++)
            {
                sum += (decimal)cartItems[i].Price * quantities[i];
            }
            Console.WriteLine(sum);
            // Return a success response
            return Ok(sum);
        }
    }
}
