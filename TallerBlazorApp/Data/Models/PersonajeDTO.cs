using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerBlazorApp.Data.Models
{
    public class PersonajeDTO 
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Por favor indica nombre de personaje")]
        [StringLength(10, ErrorMessage = "No cumple longitud", MinimumLength = 2 )]
        public string nombre { get; set; } = string.Empty;
        [Range(10,100, ErrorMessage = "La salud debe estar entre 10 y 100")]
        public int salud { get; set; }
        public int energia { get; set; }
        public int fuerza { get; set; }
        public int inteligencia { get; set; }
        public int agilidad { get; set; }
        public int nivel { get; set; }
        public int defensa { get; set; }
        [Required(ErrorMessage = "Debe ingresar un id de personaje")]
        public int? tipoId { get; set; }
    }
}