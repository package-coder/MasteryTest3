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
        
        public async Task<IEnumerable<Order>> GetAllOrders(int? clientId)
        {
            return await _connection.QueryAsync<Order>("GetAllOrders", new {clientId});
        }
        
        public async Task<int?> SaveOrder(Order order)
        {
            return await _connection.QuerySingleAsync<int?>("SaveOrder", new
            {
                clientId = _sessionRepository.GetInt("userId"),
                order.Id,
                order.status
            }, commandType: CommandType.StoredProcedure);
        }
        
        public async Task<int> SaveOrderItems(int orderId, IEnumerable<OrderItem> orderItems)
        {
            var data = orderItems.ToList()
                .Select(item => new {
                    orderId,
                    item.Id,
                    item.name,
                    item.quantity,
                    item.remark,
                    item.unit,
                    productId = item.product?.Id,
                });
            return await _connection.ExecuteAsync("SaveOrderItem", data);
        }

        public async Task<Order?> GetDraftOrderRequest()
        {
            var orders =  await _connection.QueryAsync<Order, OrderItem, Order>(
                "GetDraftOrderRequest",
                (order, orderItem) =>
                {
                    order.orderItems.Add(orderItem);
                    return order;
                },
                new  {  clientId = _sessionRepository.GetInt("userId") },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            );

            return orders.GroupBy(order => order.Id)
                    .Select(orders =>
                    {
                        var first = orders.First();
                        first.orderItems = orders.Select(order => order.orderItems.Single()).ToList();
                        return first;
                    }
            ).FirstOrDefault();
        }

        public Task<int> DeleteOrderItems(IEnumerable<OrderItem> orderItems)
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> GetOrderById(int id) {
            var result = await _connection.QueryAsync<Order, User, Order>(
                "GetOrderById",
                (order, user) =>
                {
                    order.user = user;
                    return order;
                }, 
                new { Id = id }, splitOn: "Id");

            return result.FirstOrDefault();
        }
    }
}
