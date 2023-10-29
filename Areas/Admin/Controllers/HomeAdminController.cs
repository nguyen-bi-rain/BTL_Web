using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("/admin/donhang")]
        public IActionResult OrderList()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }
        [Route("/admin/xoa_don_hang/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var product = await _context.Orders
                .Include(p => p.IdaccountNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        
        [HttpPost("/admin/xoa_don_hang/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'LTWEBContext.Products'  is null.");
            }
            var product = await _context.Orders.FindAsync(id);
            if (product != null)
            {
                _context.Orders.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(OrderList));
        }

    }
}
