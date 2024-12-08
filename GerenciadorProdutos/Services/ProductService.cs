using GerenciadorProdutos.Models.Products.ProductDTO;
using GerenciadorProdutos.Repository;
using Microsoft.EntityFrameworkCore;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public List<ProductResponseDTO> GetAllProducts()
    {
        return _context.Products
            .Include(p => p.Category)
            .Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Status = p.Status,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque,
                CategoryId = p.CategoryId,
                Categoria = p.Category.Categoria
            })
            .ToList();
    }

    public List<ProductResponseDTO> GetProductsByCategory(int categoryId)
    {
        return _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Status = p.Status,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque,
                CategoryId = p.CategoryId,
                Categoria = p.Category.Categoria
            })
            .ToList();
    }

    public ProductResponseDTO GetProductById(int id)
    {
        var product = _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Status = p.Status,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque,
                CategoryId = p.CategoryId,
                Categoria = p.Category.Categoria
            })
            .FirstOrDefault();

        return product;
    }

    public List<ProductResponseDTO> GetProductsInStock()
    {
        return _context.Products
            .Where(p => p.Status == "Em estoque")
            .Include(p => p.Category)
            .Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Status = p.Status,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque,
                CategoryId = p.CategoryId,
                Categoria = p.Category.Categoria
            })
            .ToList();
    }

    public ProductResponseDTO CreateProduct(ProductDTO productDTO)
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
        };

        _context.Products.Add(product);
        _context.SaveChanges();

        var productResponse = new ProductResponseDTO
        {
            Id = product.Id,
            Nome = product.Nome,
            Descricao = product.Descricao,
            Status = product.Status,
            Preco = product.Preco,
            QuantidadeEstoque = product.QuantidadeEstoque,
            CategoryId = product.CategoryId,
            Categoria = category.Categoria
        };
        return productResponse;
    }

    public Product UpdateProduct(int id, ProductDTO productDTO)
    {
        var productExists = _context.Products.Find(id);

        if (productExists == null) return null;

        productExists.Nome = productDTO.Nome;
        productExists.Descricao = productDTO.Descricao;
        productExists.Status = productDTO.Status;
        productExists.Preco = productDTO.Preco;
        productExists.QuantidadeEstoque = productDTO.QuantidadeEstoque;

        _context.Products.Update(productExists);
        _context.SaveChanges();
        return productExists;
    }

    public ProductResponseDTO UpdateStock(int id, ProductStockRequest productRequest)
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

        var productResponse = GetProductById(id);

        return productResponse;
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
