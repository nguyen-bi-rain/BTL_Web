using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Models;

namespace ShopQuanAo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        private readonly LTWEBContext _context;

        public HomeAdminController(LTWEBContext context)
        {
            _context = context;
        }

        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrderList()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }
        
    }
}
