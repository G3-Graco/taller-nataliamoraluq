using TallerBlazorApp.Components;
using Core.Entities;
using TallerBlazorApp.Data.Models;

namespace TallerBlazorApp.Data.Services
{
    public class PersonajeService
    {
        const string url = "Personaje";
        private readonly TokenContainer _tokenContainer;
        public PersonajeService(TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }
        public async Task<Response<Personaje>> CrearPersonaje(PersonajeDTO personaje)
        {
            Response<Personaje> response = new Response<Personaje>();

            try
            {
                response = await
                    Consumer
                    .Execute<Personaje, PersonajeDTO>(
                        url,
                        methodHttp.POST,
                        personaje)
                    ;

                return response;
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<Response<Personaje>> ActualizarPersonaje(PersonajeDTO personaje)
        {
            Response<Personaje> response = new Response<Personaje>();

            try
            {
                response = (await
                    Consumer
                    .Execute<Personaje, PersonajeDTO>(
                        url,
                        methodHttp.PUT,
                        personaje)
                    );

                return response;
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<Response<List<Personaje>>> GetAll()
        {
            Response<List<Personaje>> response = new Response<List<Personaje>>();
            List<Personaje> lstPersonajes = new List<Personaje>();
            try
            {
                string t = _tokenContainer.token;
                response = await Consumer
                    .Execute<List<Personaje>>(
                        url,
                        methodHttp.GET,
                        lstPersonajes,
                        t
                    );

            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<Response<Personaje>> Get(int id)
        {
            Response<Personaje> response = new Response<Personaje>();
            try
            {

                response = await Consumer
                    .Execute<Personaje>(
                        $"{url}/{id}",
                        methodHttp.GET,
                        null
                    );

            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<Response<Personaje>> Delete(int id)
        {
            Response<Personaje> response = new Response<Personaje>();
            try
            {

                response = await Consumer
                    .Execute<Personaje>(
                        $"{url}/{id}",
                        methodHttp.DELETE,
                        null
                    );

            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
