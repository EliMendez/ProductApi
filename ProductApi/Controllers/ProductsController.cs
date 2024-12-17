using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ICategoryRepository _ctRepo;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository ptRepo, ICategoryRepository ctRepo, IMapper mapper)
        {
            _ptRepo = ptRepo;
            _ctRepo = ctRepo;
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
        public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto) 
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

            // Additional validation: check if the category exists
            if (!_ctRepo.CategoryExists(createProductDto.CategoryId))
            {
                return NotFound($"No se encontró la categoría con el ID: {createProductDto.CategoryId}.");
            }

            var product = _mapper.Map<Product>(createProductDto);

            if (!_ptRepo.CreateProduct(product))
            {
                ModelState.AddModelError("", $"Algo sálio mal guardando el producto con el título: {product.Title}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetProduct", new {productId = product.Id}, product);
        }

        [HttpPatch("{productId:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productDto == null || productId != productDto.Id)
            {
                return BadRequest(ModelState);
            }

            var productExists = _ptRepo.GetProduct(productId);

            if (productExists == null)
            {
                return NotFound($"No se encontró el producto con el ID: {productId}");
            }

            // Additional validation: check if the category exists
            if (!_ctRepo.CategoryExists(productDto.CategoryId))
            {
                return NotFound($"No se encontró la categoría con el ID: {productDto.CategoryId}.");
            }

            var product = _mapper.Map<Product>(productDto);

            if (!_ptRepo.UpdateProduct(product))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el producto con el título: {product.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{productId:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_ptRepo.ProductExists(productId))
            {
                return NotFound($"No se encontró el producto con el ID: {productId}");
            }

            var product = _ptRepo.GetProduct(productId);

            if (!_ptRepo.DeleteProduct(product))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el producto con el título: {product.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("GetProductsByCategory/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var productList = _ptRepo.GetProductsByCategory(categoryId);

            if (productList.IsNullOrEmpty())
            {
                return NotFound($"No se encontró productos relacionados con la con categoría con ID: {categoryId}");
            }

            var productDtoList = new List<ProductDto>();

            foreach (var product in productList) 
            {
                productDtoList.Add(_mapper.Map<ProductDto>(product));
            }

            return Ok(productDtoList);
        }

        [HttpGet("SearchProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SearchProducts(string title)
        {
            try
            {
                var result = _ptRepo.SearchProducts(title);
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No se encontró productos que estén relacionados con el título: {title}");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado al recuperar los datos.");
            }
        }
    }
}
