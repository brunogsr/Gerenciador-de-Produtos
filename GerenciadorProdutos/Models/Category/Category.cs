using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GerenciadorProdutos.Models.Category
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } // Chave primária da categoria

        [Required]
        [StringLength(255)]
        public string Categoria { get; set; }

        // Propriedade de navegação: uma categoria pode ter muitos produtos
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
