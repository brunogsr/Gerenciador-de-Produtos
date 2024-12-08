using GerenciadorProdutos.Models.Category;

namespace GerenciadorProdutos.Services
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        Category CreateCategory(string categoryName);
        Category GetCategoryById(int id);
    }
}
