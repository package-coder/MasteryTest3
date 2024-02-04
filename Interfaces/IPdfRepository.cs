using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IPdfRepository
    {
        public byte[] GenerateOrderReceipt(int Id, Order order, IEnumerable<OrderItem> orderItems);
    }
}
