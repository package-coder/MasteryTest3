using Dapper;
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
            return await _connection.QueryAsync<User>("GetAllUsers");
        }

        public async Task<User> GetUserById(int Id)
        {
            return await _connection.QuerySingleAsync<User>("GetUserById", new { userId = Id});
        }
    }
}
