using System.ComponentModel.DataAnnotations;

namespace GerenciadorProdutos.Models.Users.UserDto
{
    public class RoleRequestDTO
    {
        [Required(ErrorMessage = "O Role é obrigatório.")]
        [RegularExpression("^(Cliente | Funcionário | Gerente)$", ErrorMessage = "A Role deve ser 'Cliente', 'Funcionário' ou 'Gerente'.")]

        public string? Role { get; set; }
    }
}
