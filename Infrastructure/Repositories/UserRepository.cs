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
        //aqui va el search especifico! del user --- o en un repo aparte


        public async ValueTask<User> GetUser(string UserName, string Password){
            //as is lit: UserName
            //get user: (de aqui partimos al login)
            return await base.dbSet.FirstAsync(usuario => usuario.UserName == UserName && usuario.Password == Password);
        }

        //!*: como en la base repo ya se tiene el crud basico, esto por ej
        //nos puede servir para el search por username -- en otro repo i think

        /*public override async ValueTask<User> GetByIdAsync(string UserName, string Password)
        {
            return await base.dbSet//.Include(x => x.UserName)
                                    //.Include(x => x.Password)

                                   .Include(x => x.User).FirstAsync(x => x.UserName == UserName && x.Password == Password);
        }*/
    }
}