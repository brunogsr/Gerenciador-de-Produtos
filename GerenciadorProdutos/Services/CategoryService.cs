// Services/CategoryService.cs
using GerenciadorProdutos.Models.Category;
using GerenciadorProdutos.Repository;

namespace GerenciadorProdutos.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category CreateCategory(string name)
        {
            var category = new Category { Categoria = name };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }
    }
}
