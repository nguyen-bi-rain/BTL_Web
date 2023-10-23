using ShopQuanAo.Models;
using System.Collections.Generic;

namespace ShopQuanAo.Respository
{
    public interface ICategoryRepository
    {
        Category Add(Category category);
        Category Delete(int id  );
        Category Update(Category category);
        Category Get(int id);
        IEnumerable<Category> GetAll();
    }
}
