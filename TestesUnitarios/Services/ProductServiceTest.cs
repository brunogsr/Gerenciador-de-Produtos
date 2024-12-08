using Microsoft.EntityFrameworkCore;
using GerenciadorProdutos.Models.Category;
using GerenciadorProdutos.Models.Products;
using GerenciadorProdutos.Models.Products.ProductDTO;
using GerenciadorProdutos.Repository;
using GerenciadorProdutos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class ProductServiceTests
{
    private readonly ProductService _service;
    private readonly AppDbContext _context;

    public ProductServiceTests()
    {
        // Substitui Moq para InMemoryDatabase devido a erros de conexão -> Ids duplicados
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        // Limpando o banco de dados antes de cada execução de teste -> erros de Id duplicados
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        SeedDatabase();

        _service = new ProductService(_context);
    }

    private void SeedDatabase()
    {
        var categories = new List<Category>
        {
            new Category { Categoria = "Infantil" },
            new Category { Categoria = "Coleção" }
        };

        var products = new List<Product>
        {
            new Product
            {
                Nome = "Hot Wheels", Descricao = "Carrinho de brinquedo", Status = "Em estoque",
                Preco = 100.00m, QuantidadeEstoque = 10, CategoryId = 1, Category = categories[0]
            },
            new Product
            {
                Nome = "Goku", Descricao = "Action figure", Status = "Indisponível",
                Preco = 200.00m, QuantidadeEstoque = 0, CategoryId = 2, Category = categories[1]
            },
            new Product
            {
                Nome = "Seiya de Pegaso", Descricao = "Action Figure", Status = "Em estoque",
                Preco = 100.00m, QuantidadeEstoque = 10, CategoryId = 2, Category = categories[1]
            },
        };

        _context.Categories.AddRange(categories);
        _context.Products.AddRange(products);
        _context.SaveChanges();
    }

    [Fact]
    public void GetAllProducts_ShouldReturnAllProducts()
    {
        var result = _service.GetAllProducts();  // Chama o método que estamos testando

        Assert.Equal(3, result.Count);  // Verifica se retornou 3 produtos
        Assert.Equal("Hot Wheels", result[0].Nome);  // Verifica se o nome do primeiro produto é "Hot Wheels"
    }

    [Fact]
    public void GetProductById_ShouldReturnCorrectProduct()
    {
        var result = _service.GetProductById(1);

        Assert.NotNull(result);
        Assert.Equal("Hot Wheels", result.Nome);
    }

    [Fact]
    public void GetProductsByCategory_ShouldReturnFilteredProducts()
    {
        var result = _service.GetProductsByCategory(2);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Nome == "Goku");
        Assert.Contains(result, p => p.Nome == "Seiya de Pegaso");
    }

    [Fact]
    public void CreateProduct_ShouldAddNewProduct()
    {
        var newProduct = new ProductDTO
        {
            Nome = "Novo Produto",
            Descricao = "Nova Descrição",
            Status = "Em estoque",
            Preco = 300.00m,
            QuantidadeEstoque = 5,
            CategoryId = 1
        };

        var result = _service.CreateProduct(newProduct);

        Assert.NotNull(result);
        Assert.Equal("Novo Produto", result.Nome);
        Assert.Equal(4, _context.Products.Count());
    }

    [Fact]
    public void UpdateProduct_ShouldModifyExistingProduct()
    {
        var updatedProduct = new ProductDTO
        {
            Nome = "Produto Atualizado",
            Descricao = "Descrição Atualizada",
            Status = "Indisponível",
            Preco = 150.00m,
            QuantidadeEstoque = 0,
            CategoryId = 1
        };

        var result = _service.UpdateProduct(1, updatedProduct);

        Assert.NotNull(result);
        Assert.Equal("Produto Atualizado", result.Nome);
        Assert.Equal("Descrição Atualizada", result.Descricao);
        Assert.Equal("Indisponível", result.Status);
    }

    [Fact]
    public void DeleteProduct_ShouldRemoveProduct()
    {
        _service.DeleteProduct(1);

        Assert.Equal(2, _context.Products.Count());
    }

    [Fact]
    public void UpdateStock_ShouldUpdateStockAndStatus()
    {
        var stockRequest = new ProductStockRequest
        {
            Status = "Em estoque",
            QuantidadeEstoque = 5
        };

        var result = _service.UpdateStock(1, stockRequest);

        Assert.NotNull(result);
        Assert.Equal(5, result.QuantidadeEstoque);
        Assert.Equal("Em estoque", result.Status);
    }
}
