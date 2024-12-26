using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Models.Dto;
using ProductApi.Repository.Interface;
using System.Net;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("CORSApi")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _usRepo;
        private readonly IMapper _mapper;
        protected ResponseApi _responseApi;

        public UsersController(IUserRepository usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            this._responseApi = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUsers()
        {
            var listUser = _usRepo.GetUsers();

            var userDtoList = new List<UserDto>();
            foreach (var user in listUser)
            {
                userDtoList.Add(_mapper.Map<UserDto>(user));
            }

            return Ok(userDtoList);
        }

        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int userId) 
        {
            var user = _usRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound($"No se ha encontrado un usuario con el ID: {userId}");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> createUser([FromBody] CreateUserDto createUserDto) 
        {
            bool userUnique = _usRepo.IsUniqueUser(createUserDto.Name);
            if (!userUnique)
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.isSuccess = false;
                _responseApi.ErrorMessages.Add("El usuario ya existe en los registros");
                return BadRequest(_responseApi);
            }

            var user = await _usRepo.Register(createUserDto);

            if (user == null)
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.isSuccess = false;
                _responseApi.ErrorMessages.Add("Error en el registro");
                return BadRequest(_responseApi);
            }

            _responseApi.StatusCode = HttpStatusCode.OK;
            _responseApi.isSuccess = true;
            return Ok(_responseApi);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _usRepo.Login(loginDto);
            if (response.User == null || string.IsNullOrEmpty(response.Token))
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.isSuccess = false;
                _responseApi.ErrorMessages.Add("El nombre de usuario o contraseña son incorrectos");
                return BadRequest(_responseApi);
            }

            _responseApi.StatusCode = HttpStatusCode.OK;
            _responseApi.isSuccess = true;
            _responseApi.Result = response;
            return Ok(_responseApi);
        }
    }
}
