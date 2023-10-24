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

        public IActionResult Index(int? page)
        {
            int pageSize = 9;
            int pageNum = page == null || page < 0 ? 1 : page.Value;
            var products = _context.Products.AsNoTracking().OrderBy(x => x.Name);
            PagedList<Product> list = new PagedList<Product>(products, pageNum, pageSize);  
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
        
    }
}
