using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IOrderRepository
    {
        public Task<int> AddOrderItem(OrderItem item);
        public Task<IEnumerable<Order>> GetAllOrders(int? clientId);
        public Task<OrderItem> GetOrderItem(int Id);
        public Task<int> UpdateOrderItem(OrderItem orderItem);
        public Task<int> UpdateOrderStatus();
    }
}
