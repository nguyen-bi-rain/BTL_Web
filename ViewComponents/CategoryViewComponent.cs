using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Respository;


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
