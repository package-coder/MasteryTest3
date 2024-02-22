using MasteryTest3.Data;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;

namespace MasteryTest3.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISessionService _sessionService;
    private int? clientId => _sessionService.GetInt("userId");
    private int? visibilityLevel => _sessionService.GetInt("visibilityLevel");

    public OrderService(IOrderRepository orderRepository, ISessionService sessionService)
    {
        _orderRepository = orderRepository;
        _sessionService = sessionService;
    }

    public async Task<int?> RequestOrder(Order order, List<OrderItem>? deletedOrderItems)
    {
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
        return await _orderRepository.GetAllUserOrdersByStatus((int)clientId!, "DRAFT");
    }
    
    public async Task<IEnumerable<Order>> GetAllOrders(OrderStatus status, Role role)
    {
        // if (clientId == null)
        // {
        //     return new List<Order>();
        // }
        
        switch (role)
        {
            case Role.REQUESTOR:
                return await _orderRepository.GetAllUserOrdersByStatus((int)clientId!, status.ToString());
            case Role.APPROVER:
                return await _orderRepository.GetAllOrdersBy(new { visibilityLevel, status = status.ToString() });
        }

        throw new ArgumentException("Role does not exists.");
    }
}