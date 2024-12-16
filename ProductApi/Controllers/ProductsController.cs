using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Models.Dto;
using ProductApi.Repository.Interface;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _ptRepo;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository ptRepo, IMapper mapper)
        {
            _ptRepo = ptRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetProducts()
        {
            var productList = _ptRepo.GetProducts();

            var productDtoList = new List<ProductDto>();
            foreach (var list in productList)
            {
                productDtoList.Add(_mapper.Map<ProductDto>(list));
            }

            return Ok(productDtoList);
        }

        [HttpGet("{productId:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProduct(int productId)
        {
            var product = _ptRepo.GetProduct(productId);

            if (product == null) 
            { 
                return NotFound($"No se encontró el producto con el ID: {productId}");
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult createProduct([FromBody] CreateProductDto createProductDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createProductDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ptRepo.ProductExists(createProductDto.Title))
            {
                ModelState.AddModelError("", "El producto ya existe.");
                return StatusCode(404, ModelState);
            }

            var product = _mapper.Map<Product>(createProductDto);

            if (!_ptRepo.CreateProduct(product))
            {
                ModelState.AddModelError("", $"Algo sálio mal guardando el registro con el nombre {product.Title}.");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetProduct", new {productId = product.Id}, product);
        }
    }
}
