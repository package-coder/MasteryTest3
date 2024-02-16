using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<Order?> GetOrderById(int id);
        public Task<int?> SaveOrder(Order order);
        public Task<int> SaveOrderItems(int orderId, IEnumerable<OrderItem> orderItems);
        public Task<Order?> GetDraftOrderRequest();
        public Task<int> DeleteOrderItems(IEnumerable<OrderItem> orderItems);
        public Task<int> DeleteDraftOrderRequest(Order order);
    }
}
    