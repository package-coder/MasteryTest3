﻿using Dapper;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using System.Data;

namespace MasteryTest3.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository (IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _connection.QueryAsync<User, UserRole, User>(
                "GetAllUsers", (user, userRole) => {
                    user.role = userRole;
                    return user;
                }, splitOn: "Id");
        }

        public async Task<User> GetUserById(int Id)
        {
            var result =  await _connection.QueryAsync<User, UserRole, User>(
                "GetUserById", 
                (user, userRole) => {
                    user.role = userRole;
                    return user;
                }, 
                param: new { userId = Id}, splitOn: "Id");

            return result.FirstOrDefault();
        }
    }
}
