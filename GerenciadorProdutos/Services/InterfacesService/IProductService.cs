using GerenciadorProdutos.Models.Products.ProductDTO;

public interface IProductService
{
    List<ProductResponseDTO> GetAllProducts();
    List<ProductResponseDTO> GetProductsByCategory(int categoryId);
    ProductResponseDTO GetProductById(int id);
    List<ProductResponseDTO> GetProductsInStock();
    ProductResponseDTO CreateProduct(ProductDTO productDTO);
    Product UpdateProduct(int id, ProductDTO productDTO);
    ProductResponseDTO UpdateStock(int id, ProductStockRequest productRequest);
    void DeleteProduct(int id);
}
