using MasteryTest3.Models;

namespace MasteryTest3.Interfaces;

public interface IOrderApprovalRepository
{
    Task<int?> SaveLog(Order order, User user, string? remark);
    Task<IEnumerable<OrderApprovalLog>> GetAllApprovals(int orderId);
}