using Dapper;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using System.Data;

namespace MasteryTest3.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _connection;
        private readonly ISessionService _sessionService;

        public OrderRepository(IDbConnection connection, ISessionService sessionService)
        {
            _connection = connection;
            _sessionService = sessionService;
        }

        public async Task<int?> SaveOrder(Order order)
        {

            // if (order.status == "FOR_APPROVAL") {
            //     var items = await GetDraftOrderRequest();
            //     
            //     foreach (var item in items.orderItems) {
            //        int productNameTotal = item.name.Sum(ch => ch);
            //        int unitTotal = item.unit.Sum(ch => ch);
            //
            //        order.crc += productNameTotal + unitTotal + (int)item.quantity;
            //     }
            // }

            return await _connection.QuerySingleAsync<int?>("SaveOrder", new
            {
                clientId = order.user.Id,
                order.Id,
                order.status,
            }, commandType: CommandType.StoredProcedure); ;
        }

        public async Task<int> SaveOrderItems(int orderId, IEnumerable<OrderItem> orderItems)
        {
            var data = orderItems.ToList()
                .Select(item => new
                {
                    orderId,
                    item.Id,
                    item.name,
                    item.quantity,
                    item.remark,
                    item.unit,
                    productId = item.product?.Id,
                });
            return await _connection.ExecuteAsync("SaveOrderItem", data, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteOrderItems(IEnumerable<OrderItem> orderItems)
        {
            var ids = orderItems.Select(item => new { item.Id });
            return await _connection.ExecuteAsync("DeleteOrderItem", ids, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteDraftOrderRequest(Order order)
        {
            return await _connection.ExecuteAsync("DeleteDraftOrderRequest", new { order.Id }, commandType: CommandType.StoredProcedure);
        }


        private async Task<List<Order>> QueryOrders(string sql, object? param = null)
        {
                var orders = await _connection.QueryAsync<Order, OrderItem, Product, Order>(
                    sql,
                    (order, orderItem, product) =>
                    {
                        orderItem.product = product;
                        order.orderItems.Add(orderItem);
                        return order;
                    },
                    param,
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                );

                return orders.GroupBy(item => item.Id)
                    .Select(groupOrder =>
                        {
                            var first = groupOrder.First();
                            first.orderItems = groupOrder.Select(order => order.orderItems.Single()).ToList();
                            return first;
                        }
                    ).ToList();
        }

        public Task<List<Order>> GetAllOrders() => QueryOrders("GetAllOrders");
        public Task<List<Order>> GetAllOrdersBy(object param) => QueryOrders("GetAllOrders", param);
        public Task<List<Order>> GetAllOrdersByStatus(string status) => QueryOrders("GetAllOrders", new { status });
        public Task<List<Order>> GetAllUserOrdersByStatus(int clientId, string status) => QueryOrders("GetAllOrders", new { status, clientId });

        public async Task<Order?> GetOrderById(int id)
        {
            var orders = await QueryOrders("GetOrderById", new { id });
            return orders.SingleOrDefault();
        }
    }
}
