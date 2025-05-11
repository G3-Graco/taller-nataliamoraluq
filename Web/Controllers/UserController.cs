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

        [HttpPost("Login")]
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

        //[HttpPost("Register")]
        [HttpPost("User")] //  ---> nombre del endpoint segun el enunciado so aja, pero es el register
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
                    return Ok(new { message = "User created successfully!" });
                }
                else
                {
                    return BadRequest(new { message = "User registration failed :(" }); 
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

        // ---------- SEARCH ** !*? ** ---------------------------------
        /*
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            //
            try
            {
                var usuario = await _userService.GetById(id);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); //
            }
        }*/

        //AQUI VA EL REGISTER -> CREATE
        // POST api/<UserController>
        /*
        [HttpPost]
        public async Task<ActionResult<User>> Registrar([FromBody] User usuario)
        {
            try
            {
                var userCreated =
                    await _userService.Register(usuario);

                return Ok(userCreated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

    }
}