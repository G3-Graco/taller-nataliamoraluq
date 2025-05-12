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

        //login?
        public async ValueTask<User> GetUser(string UserName){
            //*!
            //User user

            //
            //string UserName, string Password
            //as is lit: UserName
            //get user: 
            //return await base.dbSet.FirstAsync(usuario => usuario.UserName == UserName && usuario.Password == Password);
            return await base.dbSet.FirstOrDefaultAsync(usuario => usuario.UserName == UserName);
            //
        }

        //search by id -> el async ValueTask a usar es el del Base Repository

        //!*: como en la base repo ya se tiene el crud basico, esto por ej
        //nos puede servir para el search por username -- en otro repo i think

        /*public override async ValueTask<User> GetByIdAsync(string UserName, string Password)
        {
            return await base.dbSet.Include(x => x.UserName).FirstAsync(x => x.UserName == UserName && x.Password == Password);
        }*/
    }
}