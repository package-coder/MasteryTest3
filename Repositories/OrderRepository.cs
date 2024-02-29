using Dapper;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using System.Data;
using MasteryTest3.Data;

namespace MasteryTest3.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _connection;
        private readonly ICrcUtility _crcUtility;

        public OrderRepository(IDbConnection connection, ICrcUtility crcUtility)
        {
            _connection = connection;
            _crcUtility = crcUtility;
        }

        public async Task<int?> SaveOrder(Order order)
        {
            if (order.status == "FOR_APPROVAL") {
                var items = await GetOrderById((int)order.Id);
                order.crc = _crcUtility.GenerateCRC(items.orderItems);
                order.totalItems = items.orderItems.Count();
            }

            return await _connection.QuerySingleAsync<int?>("SaveOrder", new
            {
                clientId = order.user.Id,
                order.Id,
                order.status,
                order.attachment,
                order.totalItems,
                order.crc,
                order.visibilityLevel,
            }, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteDraftOrderRequest(int id)
        {
            return await _connection.ExecuteAsync("DeleteDraftOrderRequest", new { id }, commandType: CommandType.StoredProcedure);
        }


        private async Task<List<Order>> QueryOrders(string sql, object? param = null)
        {
                var orders = await _connection.QueryAsync<Order, User, OrderItem, Product, Order>(
                    sql,
                    (order, user, orderItem, product) =>
                    {
                        order.user = user;
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
        public Task<List<Order>> GetAllUserOrdersByStatus(int clientId, string status) => QueryOrders("GetAllOrders", new { status, clientId });
        public async Task<Order?> GetOrderById(int id)
        {
            var orders = await QueryOrders("GetOrderById", new { id });
            return orders.SingleOrDefault();
        }
    }
}
