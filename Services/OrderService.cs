using MasteryTest3.Data;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;

namespace MasteryTest3.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISessionService _sessionService;
    private readonly IOrderApprovalRepository _approvalRepository;
    
    private SessionUser? session => _sessionService.GetSessionUser();
    public OrderService(IOrderRepository orderRepository, ISessionService sessionService, IOrderApprovalRepository approvalRepository)
    {
        _orderRepository = orderRepository;
        _sessionService = sessionService;
        _approvalRepository = approvalRepository;
    }

    public async Task<int?> SaveOrderRequest(Order order, List<OrderItem>? deletedOrderItems = null)
    {
        order.user = new User(id: session!.id);
        order.Id = await _orderRepository.SaveOrder(order);

        if (deletedOrderItems is { Count: > 0 })
        {
            await _orderRepository.DeleteOrderItems(deletedOrderItems);
        }
        
        await _orderRepository.SaveOrderItems((int)order.Id!, order.orderItems);
        
        return order.Id;
    }

    public async Task DeleteOrderRequest(int id)
    {
        await _orderRepository.DeleteDraftOrderRequest(id);
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _orderRepository.GetOrderById(id);
    }

    public async Task CompleteOrderRequest(int id, string? remark, OrderStatus status)
    {
        var order = await GetOrderById(id);
        if (order == null) throw new ArgumentException("Order request should be existed");
        if (order.status != OrderStatus.FOR_APPROVAL.ToString()) throw new ArgumentException("Order status should be for approval status");
        if(order.status == OrderStatus.DISAPPROVED.ToString() && remark == null) throw new ArgumentException("Remark should not be null when disapproving order");

        order.status = status.ToString();
        var approver = new User(id: session!.id);
        
        await _orderRepository.SaveOrder(order);
        await _approvalRepository.SaveLog(order, approver, remark);
    }
    
    public async Task<List<OrderApprovalLog>> GetAllOrderLogs(Role role)
    {
        switch(role)
        {
            case Role.REQUESTER:
            {
                var logs = await _approvalRepository.GetAllOrderLogsByUser(session!.id);
                return logs.GroupBy(item => item.order.Id)
                    .Select(item =>
                    {
                        return item.First(log => log.dateLogged < DateTime.Now);
                    })
                    .ToList();
            }
            case Role.APPROVER:
            {
                return await _approvalRepository.GetAllOrderLogsByApprover(session!.id);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(role), role, null);
        }
    }
    
    public async Task<List<Order>> GetAllOrders(OrderStatus status, Role role)
    {
        return role switch
        {
            Role.REQUESTER => await _orderRepository.GetAllUserOrdersByStatus(session!.id, status.ToString()),
            Role.APPROVER => await _orderRepository.GetAllOrdersBy(new { session!.role.visibilityLevel, status = status.ToString() }),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}