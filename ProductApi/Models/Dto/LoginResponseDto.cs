using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models.Dto
{
    public class LoginResponseDto
    {
        public User User { get; set; }

        public string Role { get; set; }
        public string Token { get; set; }
    }
}
