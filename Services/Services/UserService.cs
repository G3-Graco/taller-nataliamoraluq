using System.Text;
using System.Security.Claims;
// tokens - jwt
using Microsoft.IdentityModel.Tokens;

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
        
        //login con verificacion de la password hashed
        public async Task<string> Login(User user)
        {
            //User user
            //var LoginUser = _users.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            
            var lstUsers = (await _unitOfWork.UserRepository.GetAllAsync()).ToList();

            //var Logged = lstUsers.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            var Logged = await _unitOfWork.UserRepository.GetUser(user.UserName); // **??!!
            //prueba (ver user repo modif.!!!!!!)
            //var LoginUser = await _unitOfWork.UserRepository.GetUser(user.UserName);

            
            //var LoginUser = await _unitOfWork.UserRepository.GetUser(user.UserName);
            //busca el usuario por username primero
            if (Logged == null)
            {
                return string.Empty;
            }

            /*
            
            if (LoginUser == null)
            {
                return string.Empty;
            }
            */

            // verifica la contraseña hasheada con la password recibida
            //var passwordVerificationResult = _passwordHasherService.VerifyPassword(LoginUser.Password, user.Password);
            
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
        
        

        //prueba hasheo con password hasher
        public async Task<bool> Register(string UserName, string Password)
        {
            // ... validaciones de usuario
            //mi user validator right hereee
            User newUser = new User();

            // Para guardar el 'username' y el 'hashedPassword' en la base de datos
            // ...
            //el nuevo usuario tendra los datos ingresados (username por string y password 
            //una vez q ha sido hasheada)

            newUser.UserName = UserName;
            newUser.Password = Password; //le pasamos primero el mero texto, para
            //que verifique con loas validators el formato

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

            //... como el validator lo usamos siempre al inicio
            // pq recibimos por params la instancia nueva de la Entity
            // pero aqui recibimos son 2 strings, 1 cm viene y el otro q se hashea
            //deberiamos de validar despues? o antes?
            //duh despues sino va a validar algo vacio y explota XDD
            
            // add to DB
            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.CommitAsync();

            return true;
        }

        //public async Task<User> Register(User newUsuario)

        /*
        Task<User> Register(string UserName, string Password); 
        // **
        Task<User> SearchById(int id){} -> esto ya esta en el base so
        Task<User> SearchUser(string UserName, string Password){}
        */

        /*public async Task<User> Register(User newUsuario)
        {
            UserValidators validator = new UserValidators(); //user validators
            var validationResult = await validator.ValidateAsync(newUsuario);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage);
            }
            /*
            //investigadno esto y viendo como hacerlo andamos, pq aqui va la cuestion
            //al crear un nuevo user se deberia de encriptar la 
            //contraseña para hacerla segura
            //una de las sugs- del profe es invertirla y guardarla invertida
            //ahora, requerimos igual el token para eso?
            //pq if token == key == firma digital == clave segura
            //right?

            //inv tema del Hash
            //ver tema del manejo del token bien, probar y preg al prof
            //base de create normal + el tema del Hasheo


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
            
            
            //
            await _unitOfWork.UserRepository.AddAsync(newUsuario); //del baserepository
            await _unitOfWork.CommitAsync();
            //
            return newUsuario; //the insert/add/create of the new one
        }*/



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

        public async Task<User> SearchUser(int id){
            var userSearched = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (userSearched == null)
            {
                throw new Exception("Este usuario no existe!");
            }
            return userSearched;
        } 
    }
}