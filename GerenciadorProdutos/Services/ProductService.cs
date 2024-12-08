//using GerenciadorProdutos.Models.Products;
using GerenciadorProdutos.Models.Products.ProductDTO;
using GerenciadorProdutos.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorProdutos.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public List<ProductResponseDTO> GetAllProducts()
        {
            var products = _context.Products
                .Include(p => p.Category) // Inclui os dados da categoria
                .Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    Status = p.Status,
                    Preco = p.Preco,
                    QuantidadeEstoque = p.QuantidadeEstoque,
                    CategoryId = p.CategoryId,
                    Categoria = p.Category != null ? p.Category.Categoria : null // Nome da categoria
                })
                .ToList();

            return products;
        }

        public List<ProductResponseDTO> GetProductsByCategory(int categoryId)
        {
            var allProducts = GetAllProducts();
            var productsByCategory = allProducts.Where(p => p.CategoryId == categoryId).ToList();
            return productsByCategory;
        }

        public Product GetProductById(int id) => _context.Products.Find(id);

        public List<Product> GetProductsInStock()
        {
            return _context.Products.Where(p => p.Status == "Em estoque").ToList();
        }

        // Services/ProductService.cs
        public Product CreateProduct(ProductDTO productDTO)
        {
            var category = _context.Categories.Find(productDTO.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("Categoria não encontrada.");
            }

            var product = new Product
            {
                Nome = productDTO.Nome,
                Descricao = productDTO.Descricao,
                Status = productDTO.Status,
                Preco = productDTO.Preco,
                QuantidadeEstoque = productDTO.QuantidadeEstoque,
                CategoryId = productDTO.CategoryId,
                Category = category
            };

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product UpdateProduct(int id, ProductDTO productDTO)
        {
            var productExists = _context.Products.Find(id);

            if (productExists == null) return null;

            // Atualização com base nos dados do DTO
            productExists.Nome = productDTO.Nome;
            productExists.Descricao = productDTO.Descricao;
            productExists.Status = productDTO.Status;
            productExists.Preco = productDTO.Preco;
            productExists.QuantidadeEstoque = productDTO.QuantidadeEstoque;

            _context.Products.Update(productExists);
            _context.SaveChanges();
            return productExists;
        }

        public Product UpdateStock(int id, ProductStockRequest productRequest)
        {
            var productExists = _context.Products.Find(id);

            if (productExists == null) return null;

            if (!string.IsNullOrEmpty(productRequest.Status))
            {
                if (productRequest.Status != "Em estoque" && productRequest.Status != "Indisponível")
                {
                    throw new ArgumentException("Status inválido. Deve ser 'Em estoque' ou 'Indisponível'.");
                }
                productExists.Status = productRequest.Status;
            }

            if (productRequest.QuantidadeEstoque >= 0)
            {
                productExists.QuantidadeEstoque = productRequest.QuantidadeEstoque;

                if (productExists.QuantidadeEstoque == 0)
                {
                    productExists.Status = "Indisponível";
                }
                else if (productExists.Status != "Em estoque" && productExists.QuantidadeEstoque > 0)
                {
                    productExists.Status = "Em estoque";
                }
            }

            _context.Products.Update(productExists);
            _context.SaveChanges();

            return productExists;
        }


        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
