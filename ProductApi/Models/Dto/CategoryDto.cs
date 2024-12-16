using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models.Dto
{
    public class CategoryDto
    {
        public int Id;

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(50, ErrorMessage = "El número máximo de caracteres es de 50.")]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
