using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopQuanAo.Models;


namespace ShopQuanAo.ViewComponents
{
    public class SameProductViewComponent : ViewComponent
    {
        private readonly LTWEBContext _context;

        public SameProductViewComponent(LTWEBContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = _context.Products.Include(p => p.IdcategoryNavigation).Where(p => p.Idcategory == p.IdcategoryNavigation.Id ).Take(7).ToList();
            return View(products);
        }

    }
}
