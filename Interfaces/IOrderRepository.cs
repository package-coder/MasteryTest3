using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IOrderRepository
    {
        public Task<int?> SaveOrder(Order order);
        public Task<int> SaveOrderItems(int orderId, IEnumerable<OrderItem> orderItems);
        public Task<Order?> GetOrderById(int id);
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<IEnumerable<Order>> GetAllOrdersBy(object param);
        public Task<IEnumerable<Order>> GetAllUserOrdersByStatus(int clientId, string status);
        public Task<int> DeleteOrderItems(IEnumerable<OrderItem> orderItems);
        public Task<int> DeleteDraftOrderRequest(Order order);
    }
}
    