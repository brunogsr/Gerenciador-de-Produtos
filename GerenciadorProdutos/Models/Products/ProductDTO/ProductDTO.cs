using System.ComponentModel.DataAnnotations;

public class ProductDTO : IValidatableObject
{
    [Required(ErrorMessage = "O Nome é obrigatório.")]
    [StringLength(50, ErrorMessage = "O Nome pode ter no máximo 50 caracteres.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "A Descrição é obrigatória.")]
    [StringLength(255, ErrorMessage = "A Descrição pode ter no máximo 255 caracteres.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O Status é obrigatório.")]
    [RegularExpression("^(Em estoque|Indisponível)$", ErrorMessage = "O Status deve ser 'Em estoque' ou 'Indisponível'.")]
    public string? Status { get; set; }

    [Required(ErrorMessage = "O Valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "A Quantidade de Estoque é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A Quantidade de Estoque deve ser maior ou igual a 0.")]
    public int QuantidadeEstoque { get; set; }
    public int CategoryId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Status == "Em estoque" && QuantidadeEstoque == 0)
        {
            yield return new ValidationResult(
                "O Status não pode ser 'Em estoque' se a Quantidade de Estoque for 0.",
                new[] { nameof(Status), nameof(QuantidadeEstoque) });
        }
    }
}
