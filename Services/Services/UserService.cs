using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Responses;
using Services.Validators;


namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        //func Login
        public async Task <string> Login(User user)
        {
            //al iniciar sesion --- buscamos el usuario
            var LoginUser = await _unitOfWork.UserRepository.GetUser(user.UserName, user.Password);
            //
            //var LoginUser = _users.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);

            if(LoginUser == null)
            {
                return string.Empty;
            }
            //verifico los datos del usuario; realmente deberia ser con encriptamiento y asi
            //pero a efectos de este taller lo haremos asi, una vez verificados los datos del user
            //con la clase handler del token (Jwt Bearer)
            //hago la soperaciones necesairas para poder crear el propio token
            var tokenHandler = new JwtSecurityTokenHandler();
            //<PackageReference Include="Microsoft.IdentityModel" Version="7.0.0" />
            var key = Encoding.ASCII.GetBytes("6f4d75aab32aef76b24c058d1bf7b979");
            var tokenDescriptor = new SecurityTokenDescriptor //error de vers.
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, LoginUser.UserName),
                    new Claim("id", LoginUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string userToken = tokenHandler.WriteToken(token);
            return userToken;
        }



        /* //Register/Create
        public async Task<Uset> Register(User newUser)
        {
            //aqui sera cm? db or anything else? 
            //inv tema del Hash
            //ver tema del manejo del token bien, probar y preg al prof
            //base de create normal de Ubicacion:

            UbicacionValidators validator = new UbicacionValidators();
            var validationResult = await validator.ValidateAsync(newUbicacion);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage);
            }

            await _unitOfWork.UbicacionRepository.AddAsync(newUbicacion);
            await _unitOfWork.CommitAsync();

            return newUbicacion;
        }
        */

        //also Search
    }
}