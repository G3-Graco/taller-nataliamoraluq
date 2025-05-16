using System.Text;
using System.Security.Claims;
// tokens - jwt
using Microsoft.IdentityModel.Tokens;

//
using Microsoft.AspNetCore.Identity; //para hashing

using System.IdentityModel.Tokens.Jwt;
// capas API
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Responses;
using Services.Validators;
using Services.Helpers;
//using System.IdentityModel.Tokens.Jwt;


namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasherService _passwordHasherService; 
        //prueba de hasher

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _passwordHasherService = new PasswordHasherService();
        }
        private List <User> _users = new List<User>
        { 
            new User{ UserName = "Admin", Password = "Password", Id = 1}
        };


        //private string _token { get; set; }

        //public UserService(string token)
        //{
        //    _token = token;
        //}
        //

        //lista de usuarios
        public async Task<IEnumerable<User>> GetAll() 
        {
            //para ir viendo todos los q existen
            return await _unitOfWork.UserRepository.GetAllAsync();
        }
        //
        
        // ----------- Iniciar sesion / login con verificacion de la password hashed ------------------------------------
        public async Task<string> Login(User user)
        {
            //User user
            //var LoginUser = _users.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            //var lstUsers = (await _unitOfWork.UserRepository.GetAllAsync()).ToList();
            //var Logged = lstUsers.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            var Logged = await _unitOfWork.UserRepository.GetUser(user.UserName); // **??!!
            //busca el usuario por username primero
            if (Logged == null)
            {
                return string.Empty;
            }

            // verifica la contraseña hasheada con la password recibida
            //prueba Logged
            //
            var passwordVerificationResult = _passwordHasherService.VerifyPassword(Logged.Password, user.Password);

            //ifso
            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                // --- Generamos el token JWT aqui ---
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("6f4d75aab32aef76b24c058d1bf7b979");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, Logged.UserName),
                        new Claim("id", Logged.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                string userToken = tokenHandler.WriteToken(token);
                return userToken;
            }

            return string.Empty; // Contraseña incorrecta
        }

        // ----------- REGISTER / CREAR USUARIO -------------------
        //hasheo con password hasher
        public async Task<bool> Register(string UserName, string Password)
        {
            // 
            User newUser = new User();

            // Para guardar el 'username' y el 'hashedPassword' en la db

            newUser.UserName = UserName;
            newUser.Password = Password; //le pasamos primero el mero texto, para
            //que verifique con loas validators el formato
            //user validators aqui
            UserValidators validator = new UserValidators();
            var validationResult = await validator.ValidateAsync(newUser);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage);
            }

            // HASHEO SIN VERIFICACION; LA VERIFIC LA HAREMOS LUEGO EN EL LOGIN
            string hashedPassword = _passwordHasherService.HashPassword(Password);
            //newUser.UserName = UserName;
            newUser.Password = hashedPassword; //aqui si reemplazamos
            //el texto sin hash por el texto con hasheo
            
            // add to DB
            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.CommitAsync();

            return true;
        }

        //also Search

        public async Task<User> SearchUser(int id){
            var userSearched = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (userSearched == null)
            {
                throw new Exception("Este usuario no existe!");
            }
            return userSearched;
        } 

        // ------ UPDATE -----------

        public async Task<User> Update(int id, User newUser)
        {
            //se busca por id el user
            var userSearched = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (userSearched == null)
            {
                throw new Exception("Este usuario no existe!!");
            }

            Console.WriteLine(userSearched);
            //se modifican los datos
            userSearched.UserName = newUser.UserName;
            userSearched.Password = _passwordHasherService.HashPassword(newUser.Password);

            /*
            string hashedPassword = _passwordHasherService.HashPassword(Password);
            //newUser.UserName = UserName;
            newUser.Password = hashedPassword; //aqui si reemplazamos
            //el texto sin hash por el texto con hasheo
            
            */

            Console.WriteLine(userSearched);
            
            //
            await _unitOfWork.CommitAsync();
            return userSearched;
        }
    }
}