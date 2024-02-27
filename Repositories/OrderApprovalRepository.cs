using System.Data;
using Dapper;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;

namespace MasteryTest3.Repositories;

public class OrderApprovalRepository : IOrderApprovalRepository
{
    private readonly IDbConnection _connection;

    public OrderApprovalRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int?> SaveLog(Order order, User user, string? remark)
    {
        return await _connection.QuerySingleAsync<int?>("SaveOrderApprovalLog", new
        {
            approverId = user.Id,
            orderId = order.Id,
            order.status,
            order.visibilityLevel,
            remark
        }, commandType: CommandType.StoredProcedure); ;
    }
}