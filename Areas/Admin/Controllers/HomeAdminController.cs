using Microsoft.AspNetCore.Mvc;

namespace ShopQuanAo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
