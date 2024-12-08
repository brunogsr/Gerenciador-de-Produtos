using GerenciadorProdutos.Models.Products.ProductDTO;
using GerenciadorProdutos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet] // Busca todos os produtos
    public ActionResult<List<ProductResponseDTO>> GetAllProducts() => Ok(_productService.GetAllProducts());

    [HttpGet("{id}")] // Busca um produto por ID
    public ActionResult<Product> GetProductById([FromRoute] int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }
        return Ok(product);
    }

    [HttpGet("GetProductsByCategory")]
    public ActionResult<List<ProductResponseDTO>> GetProductsByCategory([FromQuery] int categoryId)
    {
        var products = _productService.GetProductsByCategory(categoryId);
        if (products == null)
        {
            return NotFound(new { message = "Nenhum produto encontrado." });
        }
        return Ok(products);
    }

    [HttpGet("GetStock")] // Busca todos os produtos em estoque
    public ActionResult<IEnumerable<Product>> GetProductsInStock()
    {
        var productsInStock = _productService.GetProductsInStock();
        if (productsInStock == null || !productsInStock.Any())
        {
            return NotFound(new { message = "Nenhum produto em estoque encontrado." });
        }

        return Ok(productsInStock);
    }

    [HttpPost]
    [Authorize(Policy = "GerenteFuncionario")]
    public ActionResult<Product> CreateProduct([FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var createdProduct = _productService.CreateProduct(productDTO);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")] // Atualiza um produto
    [Authorize(Policy = "GerenteFuncionario")]
    public ActionResult<Product> UpdateProduct([FromRoute] int id, [FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedProduct = _productService.UpdateProduct(id, productDTO);
        if (updatedProduct == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }
        return Ok(updatedProduct);
    }

    [HttpPatch("{id}")] // Atualiza o Status e Quantidade em estoque de um produto
    [Authorize(Policy = "GerenteFuncionario")]
    public ActionResult<Product> UpdateStock([FromRoute] int id, [FromBody] ProductStockRequest productRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedProduct = _productService.UpdateStock(id, productRequest);
            if (updatedProduct == null)
            {
                return NotFound(new { message = "Produto não encontrado." });
            }

            return Ok(updatedProduct);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")] // Deleta um produto
    [Authorize(Policy = "GerenteFuncionario")]
    public ActionResult<Product> DeleteProduct([FromRoute] int id)
    {
        var productExists = _productService.GetProductById(id);
        if (productExists == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        _productService.DeleteProduct(id);
        return Ok(new { message = "Produto deletado." });
    }
}
