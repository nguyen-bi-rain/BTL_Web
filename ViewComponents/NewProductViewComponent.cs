﻿using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Models;

namespace ShopQuanAo.ViewComponents
{
    public class NewProductViewComponent : ViewComponent
    {

        private readonly LTWEBContext _context;

        public NewProductViewComponent(LTWEBContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = _context.Products.Where(p => p.Newproduct == true).ToList();
            return View(products);
        }
    }
}
