using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IUserService 
    {
        //: IBaseService<User> -> aqui nao, pq no haremos crud cm tal
        //esta base tiene precisamente las bases para hacer el CRUD
        //CON LAS DEMAS entities
        // modificacion aqui, preguntar al prof
        //string
        Task <string> Login(User user); //login hecho por el prof

        //the other endpoints
        //Task<string> Login(int idUser, string UserName, string Password); //por ej
        //Task<User> Register(int idUser, string UserName, string Password); 
        // **: Task<User> SearchUser(por id se pudiera); 
        //Task<User> SearchUser(string UserName, string Password); en el repo: sql here
    }
}