using Azure.Core;
using Dapper;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using System.Data;

namespace MasteryTest3.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _connection;
        private readonly ISessionRepository _sessionRepository;

        public OrderRepository(IDbConnection connection, ISessionRepository sessionRepository)
        {
            _connection = connection;
            _sessionRepository = sessionRepository;
        }

        public async Task<int> AddOrderItem(OrderItem item)
        {
                return await _connection.ExecuteAsync("AddOrderItems", new
                {
                    clientId = _sessionRepository.GetInt("userId"),
                    productId = item.product.Id,
                    item.quantity,
                    item.name,
                    item.remark,
                    uomId = item.uom.Id,
                });
        }
    }
}
