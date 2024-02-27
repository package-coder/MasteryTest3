using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface ICrcUtility
    {
        public int GenerateCRC(List<OrderItem> orderItems);
    }
}
