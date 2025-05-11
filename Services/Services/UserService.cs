using System.Text;
using System.Security.Claims;
// tokens - jwt
using Microsoft.IdentityModel.Tokens;
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
        public async Task<IEnumerable<User>> GetAll() 
        {
            //para ir viendo todos los q existen
            return await _unitOfWork.UserRepository.GetAllAsync();
        }
        //
        //
        public async Task<User> GetById(int id)
        {
            //search por id
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        //func Login
        public async Task <string> Login(User user)
        {
            //!*: deberiamos de usar los validators aqui tmb? or not?
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
            var tokenDescriptor = new SecurityTokenDescriptor 
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

        public async Task<User> SearchUser(string UserName, string Password){
            User userFound = new();
            var help = userFound.UserName;
            var help2 = userFound.Password;
            // ... working on this still
            // ojito mañana tempranito lo pruebo y exploto pq sin internet no furula, pero
            //al menos algo existe
            return userFound;
        } 
    }
}