using AutoMapper;
using ProductApi.Models;
using ProductApi.Models.Dto;

namespace ProductApi.Mapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            /* Category */
            CreateMap<Category, CategoryDto>().ReverseMap(); 
            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            /* Product */
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();

            /* User */
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<User, LoginResponseDto>().ReverseMap();
            CreateMap<User, UserDtoData>().ReverseMap();
        }
    }
}
