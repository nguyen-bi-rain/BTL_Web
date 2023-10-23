using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopQuanAo.Models;

namespace ShopQuanAo.Respository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LTWEBContext _context;

        public CategoryRepository(LTWEBContext context)
        {
            _context = context;
        }

        public Category Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Category Get(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories;
        }

        public Category Update(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();
            return category;
        }
    }
}
