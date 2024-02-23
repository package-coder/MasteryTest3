using MasteryTest3.Data;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;

namespace MasteryTest3.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISessionService _sessionService;
    
    private SessionUser? session => _sessionService.GetSessionUser();
    public OrderService(IOrderRepository orderRepository, ISessionService sessionService)
    {
        _orderRepository = orderRepository;
        _sessionService = sessionService;
    }

    public async Task<int?> RequestOrder(Order order, List<OrderItem>? deletedOrderItems)
    {
        order.user = new User(id: session!.id);
        order.Id = await _orderRepository.SaveOrder(order);

        if (deletedOrderItems is { Count: > 0 })
        {
            await _orderRepository.DeleteOrderItems(deletedOrderItems);
        }
        
        var unsavedItems = order.orderItems.Where(item => item.Id == null);
        await _orderRepository.SaveOrderItems((int)order.Id!, unsavedItems);

        return order.Id;
    }

    public async Task DeleteOrderRequest(Order? order)
    {
        if (order == null && order?.Id == null) return;
        await _orderRepository.DeleteDraftOrderRequest(order);
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _orderRepository.GetOrderById(id);
    }

    public async Task<IEnumerable<Order>> GetAllDraftOrders()
    {
        return await _orderRepository.GetAllUserOrdersByStatus(session!.id, "DRAFT");
    }
    
    public async Task<IEnumerable<Order>> GetAllOrders(OrderStatus status, Role role)
    {
        return role switch
        {
            Role.REQUESTOR => await _orderRepository.GetAllUserOrdersByStatus(session!.id, status.ToString()),
            Role.APPROVER => await _orderRepository.GetAllOrdersBy(new { session!.role.visibilityLevel, status = status.ToString() }),
            _ => throw new ArgumentException("Role does not exists.")
        };
    }
}