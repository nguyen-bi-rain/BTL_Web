using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(int? page,string? searchtext) 
        {
            int pageSize = 9;
            int pageNum = page == null || page < 0 ? 1 : page.Value;
            var products = searchtext == null ? _context.Products.AsNoTracking().OrderBy(x => x.Name) : _context.Products.AsNoTracking().Where(p =>p.Name.Contains(searchtext)).OrderBy(x => x.Name);
            PagedList<Product> list = new PagedList<Product>(products, pageNum, pageSize);
            ViewBag.searchtext = searchtext;
            return View(list);
        }

        public IActionResult Details(int? id)
        {
            //if(id == null || _context.Products == null)
            //{
            //    return NotFound();
            //}
            var product = _context.Products.Include(p => p.IdcategoryNavigation).FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                return Content("Sản Phẩm không tồn tại");
            }
            return View(product);
        }
        public IActionResult SearchByFillterPrice(string[]? priceRange)
        {
            List<Product> products = _context.Products.ToList();

            if (priceRange == null || priceRange.Contains("all"))
            {
                return View(products);
            }

            var filteredProducts = new List<Product>();
            foreach (var range in priceRange)
            {
                var priceRangeArray = range.Split('-');
                int minPrice = int.Parse(priceRangeArray[0]);
                int maxPrice = int.Parse(priceRangeArray[1]);

                var tempFilteredProducts = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
                filteredProducts.AddRange(tempFilteredProducts);
            }

            return View(filteredProducts.Distinct());
        }
    }
}
