using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models.Dto
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El número máximo de caracteres es de 50.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El nombre completo del usuario es obligatorio.")]
        [StringLength(100, ErrorMessage = "El número máximo de caracteres es de 100.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
