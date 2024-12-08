using Microsoft.EntityFrameworkCore;
using GerenciadorProdutos.Models.Category;
using GerenciadorProdutos.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GerenciadorProdutos.Repository;

namespace GerenciadorProdutos.Tests
{
    public class CategoryServiceTests
    {
        private readonly CategoryService _service;
        private readonly AppDbContext _context;

        public CategoryServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryTestDatabase")
                .Options;

            _context = new AppDbContext(options);


            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            SeedDatabase();

            _service = new CategoryService(_context);
        }

        private void SeedDatabase()
        {
            var categories = new List<Category>
            {
                new Category { Categoria = "Infantil" },
                new Category { Categoria = "Coleção" }
            };

            _context.Categories.AddRange(categories);
            _context.SaveChanges();
        }

        [Fact]
        public void GetAllCategories_ShouldReturnAllCategories()
        {
            var result = _service.GetAllCategories();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Categoria == "Infantil");
            Assert.Contains(result, c => c.Categoria == "Coleção");
        }

        [Fact]
        public void CreateCategory_ShouldAddNewCategory()
        {
            var newCategoryName = "Novidades";

            var result = _service.CreateCategory(newCategoryName);

            Assert.NotNull(result);
            Assert.Equal(newCategoryName, result.Categoria);
            Assert.Equal(3, _context.Categories.Count());
        }

        [Fact]
        public void GetCategoryById_ShouldReturnCorrectCategory()
        {
            var result = _service.GetCategoryById(1);

            Assert.NotNull(result);
            Assert.Equal("Infantil", result.Categoria);
        }

        [Fact]
        public void GetCategoryById_ShouldReturnNullWhenCategoryNotFound()
        {
            var result = _service.GetCategoryById(999);

            Assert.Null(result);
        }
    }
}
