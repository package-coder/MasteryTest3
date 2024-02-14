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

        public async Task<int> AddOrderItem(OrderItem item)
        {
                return await _connection.ExecuteAsync("AddOrderItems", new
                {
                    clientId = _sessionRepository.GetInt("userId"),
                    productId = item.product?.Id,
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

        public async Task<int> SaveOrder(Order order)
        {
            if (order.Id != null) return (int)order.Id;
            
            return await _connection.ExecuteAsync("SaveOrder", new
            {
                clientId = _sessionRepository.GetInt("userId"),
                order.Id,
                order.status
            });
        }
        
        public async Task<int> SaveOrderItem(int orderId, IEnumerable<OrderItem> orderItems)
        {
            var data = orderItems.ToList()
                .Where(item => item.Id == null)
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

        public async Task<Order?> GetDraftOrder()
        {
            var orders =  await _connection.QueryAsync<Order, OrderItem, UOM, Order>(
                "GetDraftOrderWithItems",
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

        public async Task<Order> GetOrderById(int Id) {
            var result = await _connection.QueryAsync<Order, User, Order>(
                "GetOrderById",
                (order, user) =>
                {
                    order.user = user;
                    return order;
                }, 
                new { Id }, splitOn: "Id");

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderAllOrderItems(int Id)
        {
            return await _connection.QueryAsync<OrderItem, UOM, OrderItem>(
                    "GetAllOrderItems",
                    (orderItem, uom) => { 
                        orderItem.uom = uom;
                        return orderItem;
                    },
                    new { Id}, splitOn: "Id");
        }
    }
}
