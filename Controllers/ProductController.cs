using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopQuanAo.Models;

namespace ShopQuanAo.Controllers
{
    public class ProductController : Controller
    {
        private readonly LTWEBContext _context;

        public ProductController(LTWEBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
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
        
    }
}
