using Dapper;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using System.Data;

namespace MasteryTest3.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDbConnection _connection;
        private readonly ISessionRepository _sessionRepository;

        public CartRepository(IDbConnection connection, ISessionRepository sessionRepository)
        {
            _connection = connection;
            _sessionRepository = sessionRepository;
        }
        public async Task<IEnumerable<OrderItem>> GetCartItems()
        {
            int? clientId = _sessionRepository.GetInt("userId");

            return await _connection.QueryAsync<OrderItem, UOM, Product, OrderItem>(
                "GetCartItems", 
                (orderItem, uom, product) => { 
                    orderItem.uom = uom;
                    orderItem.product = product;
                    return orderItem;
                },
                new { clientId }, splitOn: "Id");
        }

        public async Task<int> RemoveOrderItem(int Id) {
            return await _connection.ExecuteAsync("DeleteOrderItem", new { Id });
        }
    }
}
