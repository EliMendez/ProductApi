using ProductApi.Models;
using ProductApi.Models.Dto;

namespace ProductApi.Repository.Interface
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        bool IsUniqueUser(string usuario);
        Task<LoginResponseDto> Login(LoginDto loginDto);
        Task<User> Register(CreateUserDto createUserDto);
    }
}
