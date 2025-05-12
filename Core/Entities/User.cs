using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Entities;

namespace Core.Entities
{
    public class User : IUser
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int Id { get; set; }
    }
}