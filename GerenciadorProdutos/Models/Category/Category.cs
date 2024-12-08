using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GerenciadorProdutos.Models.Category
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Categoria { get; set; }

        // Propriedade de navegação para produtos associados a esta categoria
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }

}
