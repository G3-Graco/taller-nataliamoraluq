using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces.Services;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService) //
        {
            _userService = userService;
        }

        // REGISTER --- CREAR USUARIO ---
        [HttpPost] // (api/User) ---> nombre del endpoint segun el enunciado; que es el register/create
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Register(string UserName, string Password)
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                return BadRequest(new { message = "UserName and Password are required" });
            }
            try
            {
                var registrationResult = await _userService.Register(UserName, Password);
                if (registrationResult)
                {
                    //return Ok(new { message = "User created successfully!" });
                    return StatusCode(201, new { message = "Usuario creado correctamente!" });
                }
                else
                {
                    return BadRequest(new { message = "Ocurrió un error al registrar el usuario :(" }); 
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        //
        // LOGIN --- INICIAR SESION
        //[HttpPost("natalia/Login")] //**!!!?? es auth por author o solo auth?
        [HttpPost("auth/login")] //**!!!?? es auth por author o solo auth?
        //[HttpPost("Login")] ? o [HttpPost("/login")] ? o [HttpPost("/natalia/login")] ?
        [AllowAnonymous]
        //
        public async Task<ActionResult<User>> Login(User user) // *?!
        {
            var token = await _userService.Login(user);
            if(token == null || token == string.Empty)
            {
                return BadRequest(new { message = "UserName or Password is incorrect" });
            }
            return Ok(token);
        }

        // GET ALL --- PARA VER TODOS LOS REGISTROS EXISTENTES
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var usuariosExisting = await _userService.GetAll();
            return Ok(usuariosExisting);
        }

        //
        // GET -- BUSCAR --- CONSULTAR USUARIO POR ID ---

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var Userfound = await _userService.SearchUser(id);
                return Ok(Userfound);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}