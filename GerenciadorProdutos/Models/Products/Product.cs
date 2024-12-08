using System.ComponentModel.DataAnnotations;
using GerenciadorProdutos.Models.Category;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nome { get; set; }

    [StringLength(255)]
    public string? Descricao { get; set; }

    [Required]
    [StringLength(20)]
    [RegularExpression("^(Em estoque|Indisponível)$")]
    public string Status { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantidadeEstoque { get; set; }

    [Required]
    public int CategoryId { get; set; } // Chave estrangeira obrigatória

    // Propriedade de navegação para a entidade Category
    public Category Category { get; set; }
}
