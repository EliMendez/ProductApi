using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDtoData User { get; set; }

        public string Role { get; set; }
        public string Token { get; set; }
    }
}
