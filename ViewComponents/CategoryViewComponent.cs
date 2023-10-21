using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Models;
using ShopQuanAo.Respository;
using SQLitePCL;

namespace ShopQuanAo.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _category;

        public CategoryViewComponent(ICategoryRepository category)
        {
            _category = category;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoties = _category.GetAll().OrderBy(c => c.Name);
            return View(categoties);
        }
    }
}
