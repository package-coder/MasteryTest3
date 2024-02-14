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
        private readonly ICartRepository _cartRepository;

        public OrderRepository(IDbConnection connection, ISessionRepository sessionRepository, ICartRepository cartRepository)
        {
            _connection = connection;
            _sessionRepository = sessionRepository;
            _cartRepository = cartRepository;
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
            });
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
                    uomId = item.uom?.Id,
                    productId = item.product?.Id,
                });
            return await _connection.ExecuteAsync("SaveOrderItem", data);
        }

        public async Task<Order?> GetDraftOrderRequest()
        {
            var orders =  await _connection.QueryAsync<Order, OrderItem, UOM, Order>(
                "GetDraftOrderRequest",
                (order, orderItem, unit) =>
                {
                    orderItem.uom = unit;
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
        public async Task<int> UpdateOrderStatus() {
            var cartItems = await _cartRepository.GetCartItems();
            int crc = 0;

            foreach (var item in cartItems) {
                crc += item.name.Sum(ch => (int)ch) + item.quantity + item.uom.Id;
            }

            return await _connection.ExecuteAsync(
                "UpdateOrderStatus",
                new {
                    clientId = _sessionRepository.GetInt("userId"),
                    status = "FOR_APPROVAL",
                    crc
                });
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

        public async Task<IEnumerable<OrderItem>> GetOrderAllOrderItems(int id)
        {
            return await _connection.QueryAsync<OrderItem, UOM, OrderItem>(
                    "GetAllOrderItems",
                    (orderItem, uom) => { 
                        orderItem.uom = uom;
                        return orderItem;
                    },
                    new { Id = id}, splitOn: "Id");
        }
    }
}
