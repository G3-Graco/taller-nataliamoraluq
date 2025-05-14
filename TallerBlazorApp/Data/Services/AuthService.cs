using Core.Entities;
//using Data.Models;
namespace TallerBlazorApp.Data.Services
{
    public class AuthService
    {
        const string url = "User"; //

        // --- LOGIN / INICIAR SESION ---
        //
        public async Task<Response<string>> Login(UserDTO usuario)
        {
            //public async Task<Response<User>> Login(User usuario)
            //User pq el task en services y controller (api) son User
            Response<string> response = new Response<string>();
            try
            {
                response = await
                    Consumer.Execute<string, UserDTO>(
                        $"{url}/auth/login",
                        //$"{url}/Login",
                        methodHttp.POST,
                        usuario)
                    ;
                return response;
            }
            catch (Exception ex)
            {
                //
            }
            return response;
        }

        // Prueba *! : para el register 
        // --- REGISTER / CREAR USUARIO ---
        //
        public async Task<Response<User>> Register(UserDTO user)
        {
            
            Response<User> response = new Response<User>();
            try
            {
                response = await 
                    Consumer
                    .Execute<User, UserDTO>(
                        url,
                        //$"{url}/", ? o url? -> pq es solo User
                        //!* detalle aqui: en la api -> api/User so if $"{url} = User es solo url o url/ ?
                        //if aint working, colocarle a la api /User/user/ if
                        methodHttp.POST,
                        user
                    );
                //return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }


        /*public async Task<Response<User>> Register(User user)
        {
            //string UserName, string Password
            //public async Task<Response<string>> Register(User usuario)
            //bool pq el task en services y controller (api) es bool
            Response<string> response = new Response<string>();
            try
            {
                response = await
                    Consumer.Execute<string, User>(
                        $"{url}",
                        //$"{url}/",
                        //!* detalle aqui: en la api -> api/User so if $"{url} = User es solo url o url/ ?
                        //if aint working, colocarle a la api /User/user/ if
                        methodHttp.POST,
                        usuario)
                    ;
                //return response;
            }
            catch (Exception ex)
            {
                //
            }
            return response;
        }
        */
    }

}
