using Dapper;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using Microsoft.Identity.Client;
using System.Data;

namespace MasteryTest3.Repositories
{
    public class UOMRepository : IUOMRepository
    {
        private readonly IDbConnection _connection;

        public UOMRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<UOM>> GetAllUOM()
        {
            return await _connection.QueryAsync<UOM>("GetAllUOM");
        }
    }
}
