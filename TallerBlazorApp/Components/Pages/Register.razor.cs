using TallerBlazorApp.Data.Models;
using TallerBlazorApp.Data.Services;
using Microsoft.AspNetCore.Components;

namespace TallerBlazorApp.Components.Pages
{
    public partial class Register
    {
        //
        public UserDTO user = new(); //
        //
        [Inject]
        public AuthService? service { get; set; }
        //
        public string mensaje { get; set; } = string.Empty;
        //public string claseMensaje { get; set; } = string.Empty;
        //
        public async void Register()
        {
            /*if(user.UserName.Length < 4 && user.Password.Length < 4)
            {
                
                mensaje = "Los Campos son Requeridos";
                claseMensaje = "alert alert-danger";
                return;
            }*/

            var response = await service.Register(user);

            if(response.Ok)
            {
                mensaje = "Usuario creado!";
                //claseMensaje = "alert alert-success";
            }
            else
            {
                mensaje = response.Message;
                //claseMensaje = "alert alert-danger";
            }
            CleanAll();
            StateHasChanged();

        }
        public void CleanAll()
        {
            user.Password = "";
            user.UserName = "";
        }

    }
}