using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Models;
using System.Diagnostics;

namespace ShopQuanAo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LTWEBContext _context;

        public HomeController(ILogger<HomeController> logger, LTWEBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult ProductCategory(int? id)
        {
            if(id == null ||  _context.Products == null)
            {
                return NotFound();
            }
            var lstProducts = _context.Products.Where(p => p.Idcategory == id).OrderBy(p => p.Id).ToList();
            return View(lstProducts);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}