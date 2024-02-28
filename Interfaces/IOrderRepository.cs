using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IOrderRepository
    {
        public Task<int?> SaveOrder(Order order);
        public Task<int> SaveOrderItems(int orderId, IEnumerable<OrderItem> orderItems);
        public Task<Order?> GetOrderById(int id);
        public Task<List<Order>> GetAllOrders();
        public Task<List<Order>> GetAllOrdersBy(object param);
        public Task<List<Order>> GetAllUserOrdersByStatus(int clientId, string status);
        public Task<int> DeleteOrderItems(IEnumerable<OrderItem> orderItems);
        public Task<int> DeleteDraftOrderRequest(int id);
    }
}
    