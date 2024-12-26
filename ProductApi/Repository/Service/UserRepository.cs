using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Data;
using ProductApi.Models;
using ProductApi.Models.Dto;
using ProductApi.Repository.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ProductApi.Repository.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSetting:Secret");
        }

        public User GetUser(int userId)
        {
            return _db.Users.FirstOrDefault(u => u.Id == userId);
        }

        public ICollection<User> GetUsers()
        {
            return _db.Users.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == usuario);

            if (user == null)
            {
                return true; 
            }

            return false;
        }

        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var password = getMd5(loginDto.Password);

            var user = _db.Users.FirstOrDefault(
                u => u.UserName.ToLower() == loginDto.UserName.ToLower() && u.Password == password
            );

            // Validate if the user does not exist with the correct username and password combination
            if (user == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToLower()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };

            return loginResponseDto;
        }

        public async Task<User> Register(CreateUserDto createUserDto)
        {
            var password = getMd5(createUserDto.Password);

            User user = new User()
            {
                UserName = createUserDto.UserName,
                Name = createUserDto.Name,
                Password = password,
                Role = createUserDto.Role,
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            user.Password = password;
            return user;
        }

        public static string getMd5(string valor)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = md5Hasher.ComputeHash(data);

            string result = "";
            for (int i = 0; i < data.Length; i++)
            {
                result += data[i].ToString("x2").ToLower();
            }

            return result;

        }
    }
}
