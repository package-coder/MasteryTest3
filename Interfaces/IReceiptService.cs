using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IReceiptService
    {
        public byte[] GenerateOrderReceipt(Order order, IEnumerable<OrderApprovalLog> approvals);
    }
}
