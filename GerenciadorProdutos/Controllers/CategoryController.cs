// Controllers/CategoryController.cs
using GerenciadorProdutos.Services;
using GerenciadorProdutos.Models;
using Microsoft.AspNetCore.Mvc;
using GerenciadorProdutos.Models.Category;
using Microsoft.AspNetCore.Authorization;

namespace GerenciadorProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<List<Category>> GetCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost]
        [Authorize(Policy = "GerenteFuncionario")]
        public ActionResult<Category> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (string.IsNullOrEmpty(categoryDTO.Categoria))
            {
                return BadRequest("O nome da categoria é obrigatório.");
            }

            var category = _categoryService.CreateCategory(categoryDTO.Categoria);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, category);
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetCategoryById([FromRoute] int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
    }
}

