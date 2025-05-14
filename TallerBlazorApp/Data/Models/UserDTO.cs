using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerBlazorApp.Data.Models
{
    public class UserDTO 
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Debes ingresar un username")]
        [StringLength(250, ErrorMessage = "El user name no cumple con la min. longitud", MinimumLength = 4 )]
        public string  UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes ingresar una contraseña")]
        [StringLength(250, ErrorMessage = "La contraseña no cumple con la min. longitud", MinimumLength = 4 )]
        public string Password { get; set; } = string.Empty;
    }
}