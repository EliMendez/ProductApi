using AutoMapper;
using ProductApi.Models;
using ProductApi.Models.Dto;

namespace ProductApi.Mapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap(); 
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
        }
    }
}
