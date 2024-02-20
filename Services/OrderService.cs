using MasteryTest3.Interfaces;
using MasteryTest3.Models;

namespace MasteryTest3.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task RequestOrder(Order order, List<OrderItem>? deletedOrderItems)
    {
        order.Id = await _orderRepository.SaveOrder(order);

        if (deletedOrderItems is { Count: > 0 })
        {
            await _orderRepository.DeleteOrderItems(deletedOrderItems);
        }
        
        var unsavedItems = order.orderItems.Where(item => item.Id == null);
        await _orderRepository.SaveOrderItems((int)order.Id!, unsavedItems);
    }

    public async Task DeleteDraftOrderRequest(Order? order)
    {
        if (order == null && order?.Id == null) return;
        await _orderRepository.DeleteDraftOrderRequest(order);
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _orderRepository.GetOrderById(id);
    }

    public async Task<Order?> GetDraftOrderRequestWithItems()
    {
        return await _orderRepository.GetDraftOrderRequestWithItems();
    }

    public async Task<IEnumerable<Order>> GetDraftOrders()
    {
        return await _orderRepository.GetDraftOrders();
    }
}