using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetNonDraftOrders();
        public Task<IEnumerable<Order>> GetDraftOrders();
        public Task<Order?> GetOrderById(int id);
        public Task<int?> SaveOrder(Order order);
        public Task<int> SaveOrderItems(int orderId, IEnumerable<OrderItem> orderItems);
        public Task<Order?> GetDraftOrderRequestWithItems();
        public Task<int> DeleteOrderItems(IEnumerable<OrderItem> orderItems);
        public Task<int> DeleteDraftOrderRequest(Order order);
    }
}
    