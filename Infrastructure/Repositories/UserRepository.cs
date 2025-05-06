using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) {}
        // ***!!!! -> modif para el met especifico de buscar por user y password
        //aqui va el search especifico! del user

        public async ValueTask<User> GetUser(string UserName, string Password){
            //as is lit: UserName
            //get user: (de aqui partimos al login)
            return await base.dbSet.FirstAsync(usuario => usuario.UserName == UserName && usuario.Password == Password);
        }
    }
}