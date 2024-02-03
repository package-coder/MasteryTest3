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

        public async Task<IEnumerable<Order>> GetAllOrders(int? clientId)
        {
            return await _connection.QueryAsync<Order>("GetAllOrders", new {clientId});
        }

        public async Task<OrderItem> GetOrderItem(int Id) {
            var result = await _connection.QueryAsync<OrderItem, UOM, OrderItem>(
                "GetOrderItemById",
                (orderItem, uom) => {
                    orderItem.uom = uom;
                    return orderItem;
                },
                new { Id }, splitOn: "Id");

            return result.FirstOrDefault();
        }

        public async Task<int> UpdateOrderItem(OrderItem orderItem)
        {
            return await _connection.ExecuteAsync(
                "UpdateOrderItem",
                new {
                    orderItem.Id,
                    orderItem.name,
                    orderItem.remark,
                    orderItem.quantity,
                    uomId = orderItem.uom.Id,
                });
        }
    }
}
