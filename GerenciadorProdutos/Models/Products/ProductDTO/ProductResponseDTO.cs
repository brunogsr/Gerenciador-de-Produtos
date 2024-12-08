namespace GerenciadorProdutos.Models.Products.ProductDTO
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string Status { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int? CategoryId { get; set; } // Pode ser nulo
        public string? Categoria { get; set; } // Nome da categoria, também pode ser nulo
    }
}
