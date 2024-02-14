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

    public async Task RequestOrder(Order order)
    {
        var orderId = await _orderRepository.SaveOrder(order);
        if (orderId != null)
        {
            await _orderRepository.SaveOrderItems((int)orderId, order.orderItems);
        }
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _orderRepository.GetOrderById(id);
    }

    public async Task<Order?> GetDraftOrderRequest()
    {
        return await _orderRepository.GetDraftOrderRequest();
    }
}