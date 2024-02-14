using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IOrderRepository
    {
        public Task<int> AddOrderItem(OrderItem item);
        public Task<IEnumerable<Order>> GetAllOrders(int? clientId);
        public Task<OrderItem> GetOrderItem(int Id);
        public Task<IEnumerable<OrderItem>> GetOrderAllOrderItems(int Id);
        public Task<Order> GetOrderById(int Id);
        public Task<int> SaveOrder(Order order);
        public Task<int> SaveOrderItem(int orderId, IEnumerable<OrderItem> orderItems);
        public Task<Order?> GetDraftOrder();
        public Task<int> UpdateOrderItem(OrderItem orderItem);
        public Task<int> UpdateOrderStatus();
    }
}
