using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using ShopQuanAo.Models;
using X.PagedList;

namespace ShopQuanAo.Controllers
{
    public class ProductController : Controller
    {
        private readonly LTWEBContext _context;

        public ProductController(LTWEBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page, string? searchtext)
        {
            int pageSize = 9;
            int pageNum = page == null || page < 0 ? 1 : page.Value;
            var products = searchtext == null ? _context.Products.AsNoTracking().OrderBy(x => x.Name) : _context.Products.AsNoTracking().Where(p => p.Name.Contains(searchtext)).OrderBy(x => x.Name);
            PagedList<Product> list = new PagedList<Product>(products, pageNum, pageSize);
            ViewBag.searchtext = searchtext;
            if (HttpContext.Session.Get<List<Product>>("Cart") == null)
            {
                ViewBag.cartNumber = 0;
            }
            else
            {
                ViewBag.cartNumber = HttpContext.Session.Get<List<Product>>("Cart").Count;
            }
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.success = 1;
            }
            else
            {
                ViewBag.success = 0;
            }
            return View(list);
        }

        public IActionResult Details(int? id)
        {
            //if(id == null || _context.Products == null)
            //{
            //    return NotFound();
            //}
            var product = _context.Products.Include(p => p.IdcategoryNavigation).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return Content("Sản Phẩm không tồn tại");
            }
            if (HttpContext.Session.Get<List<Product>>("Cart") == null)
            {
                ViewBag.cartNumber = 0;
            }
            else
            {
                ViewBag.cartNumber = HttpContext.Session.Get<List<Product>>("Cart").Count;
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult SearchByFillterPrice(string? price)
        {


            if (HttpContext.Session.Get<List<Product>>("Cart") == null)
            {
                ViewBag.cartNumber = 0;
            }
            else
            {
                ViewBag.cartNumber = HttpContext.Session.Get<List<Product>>("Cart").Count;
            }

            List<Product> products = null;
           
            if (price == null)
            {
                products = _context.Products.AsNoTracking().OrderBy(p => p.Promationprice).ToList();
                return View(products);
            }
            else
            {
                switch (price)
                {
                    case "price-1":
                        products = _context.Products.AsNoTracking().Where(p => p.Promationprice >= 0 && p.Promationprice <= 1000000).OrderBy(p => p.Promationprice).ToList();
                        break;
                    case "price-2":
                        products = _context.Products.AsNoTracking().Where(p => p.Promationprice >= 1000000 && p.Promationprice <= 3000000).OrderBy(p => p.Promationprice).ToList();
                        break;
                    case "price-3":
                        products = _context.Products.AsNoTracking().Where(p => p.Promationprice >= 3000000 && p.Promationprice <= 5000000).OrderBy(p => p.Promationprice).ToList();
                        break;
                    case "price-4":
                        products = _context.Products.AsNoTracking().Where(p => p.Promationprice >= 5000000 && p.Promationprice <= 6000000).OrderBy(p => p.Promationprice).ToList();
                        break;
                    case "price-5":
                        products = _context.Products.AsNoTracking().Where(p => p.Promationprice > 6000000).OrderBy(p => p.Promationprice).ToList();
                        break;
                }
            }
            // Trả về PartialView
            return PartialView("SortByPriceTable", products);
        }
    }
}
