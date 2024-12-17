using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Models.Dto;
using ProductApi.Repository.Interface;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _ctRepo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategories()
        {
            var categoryList = _ctRepo.GetCategories();

            var categoryDtoList = new List<CategoryDto>();
            foreach (var list in categoryList) 
            {
                categoryDtoList.Add(_mapper.Map<CategoryDto>(list));
            }

            return Ok(categoryDtoList);

        }

        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _ctRepo.GetCategory(categoryId);

            if (category == null)
            {
                return NotFound($"No se encontró la categoría con el ID: {categoryId}");
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createCategoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ctRepo.CategoryExists(createCategoryDto.Name))
            {
                ModelState.AddModelError("", "La categoría ya existe.");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);

            if (!_ctRepo.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Algo sálio mal guardando la categoría con el nombre {category.Name}.");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCategory", new {categoryId = category.Id}, category);
        }

        /* Partially update */
        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryDto == null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var categoryExists = _ctRepo.GetCategory(categoryId);

            if (categoryExists == null)
            {
                return NotFound($"No se encontró la categoría con el ID: {categoryId}");
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_ctRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Algo sálio mal actualizando el registro con el nombre {category.Name}.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{categoryId:int}", Name = "UpdatePutCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePutCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryDto == null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var categoryExists = _ctRepo.GetCategory(categoryId);

            if (categoryExists == null)
            {
                return NotFound($"No se encontró la categoría con el ID: {categoryId}");
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_ctRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Algo sálio mal actualizando el registro con el nombre {category.Name}.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_ctRepo.CategoryExists(categoryId))
            {
                return NotFound($"No se encontró la categoría con el ID: {categoryId}");
            }

            var category = _ctRepo.GetCategory(categoryId);

            if (!_ctRepo.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Algo sálio mal eliminando el registro con el nombre {category.Name}.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
