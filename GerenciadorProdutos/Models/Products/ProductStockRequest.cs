using System.ComponentModel.DataAnnotations;

public class ProductStockRequest
{
    [Required(ErrorMessage = "O Status é obrigatório.")]
    [RegularExpression("^(Em estoque|Indisponível)$", ErrorMessage = "O Status deve ser 'Em estoque' ou 'Indisponível'.")]
    public string? Status { get; set; }

    [Required(ErrorMessage = "A Quantidade de Estoque é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A Quantidade de Estoque deve ser maior ou igual a 0.")]
    public int QuantidadeEstoque { get; set; }
}
