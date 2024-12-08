using GerenciadorProdutos.Models.Products;
using GerenciadorProdutos.Models.Products.ProductDTO;

public interface IProductService
{
    // Obtém todos os produtos
    List<ProductResponseDTO> GetAllProducts();

    // Obtém produtos por categoria
    List<ProductResponseDTO> GetProductsByCategory(int categoryId);

    // Obtém um produto pelo ID
    Product GetProductById(int id);

    // Obtém todos os produtos em estoque
    List<Product> GetProductsInStock();

    // Cria um novo produto
    Product CreateProduct(ProductDTO productDTO);

    // Atualiza um produto existente
    Product UpdateProduct(int id, ProductDTO productDTO);

    // Atualiza o estoque de um produto
    Product UpdateStock(int id, ProductStockRequest productRequest);

    // Exclui um produto pelo ID
    void DeleteProduct(int id);
}
