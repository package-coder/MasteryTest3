using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface ICartRepository
    {
        public Task<IEnumerable<OrderItem>> GetCartItems();
        public Task<int> RemoveOrderItem(int Id);

        public Task<Order?> GetCardOrder();
    }
}
