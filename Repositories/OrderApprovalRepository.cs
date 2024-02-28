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

    public async Task<IEnumerable<OrderApprovalLog>> GetAllApprovals(int orderId)
    {
        return await _connection.QueryAsync<OrderApprovalLog, User, OrderApprovalLog>(
            "GetAllApprovals",
                (orderApprovalLog, user) =>{ 
                    orderApprovalLog.user = user;
                    return orderApprovalLog;
                },
                new { orderId}, splitOn: "Id"
            );
    }

    private async Task<List<OrderApprovalLog>> QueryLogs(string sql, object? param = null)
    {
        var logs = await _connection.QueryAsync<OrderApprovalLog, Order, User, OrderApprovalLog>(
            sql,
            (approvalLog, order, user) =>
            {
                order.user = user;
                approvalLog.order = order;
                return approvalLog;
            },
            param,
            splitOn: "Id",
            commandType: CommandType.StoredProcedure
        );
        return logs.ToList();
    }

    public Task<List<OrderApprovalLog>> GetAllOrderLogsByApprover(int approverId) =>
        QueryLogs("GetAllOrderApprovalLogs", new { approverId });
    
    public Task<List<OrderApprovalLog>> GetAllOrderLogsByUser(int clientId) =>
        QueryLogs("GetAllOrderApprovalLogs", new { clientId });
}