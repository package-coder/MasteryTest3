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
            var clientId = _sessionRepository.GetInt("userId");

            return await _connection.QueryAsync<OrderItem, Product, UOM, OrderItem>(
                "GetCartItems", 
                (orderItem, product, uom) => { 
                    orderItem.product = product;
                    orderItem.uom = uom;
                    return orderItem;
                },
                new { clientId }, splitOn: "Id");
        }

        public async Task<int> RemoveOrderItem(int Id) {
            return await _connection.ExecuteAsync("DeleteOrderItem", new { Id });
        }

        public async Task<Order?> GetCardOrder()
        {
            var clientId = _sessionRepository.GetInt("userId");
            var orders = await _connection.QueryAsync<Order>("GetCartOrder", new { clientId }, commandType: CommandType.StoredProcedure);
            return orders.FirstOrDefault();
        }
    }
}
