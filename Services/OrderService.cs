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
        order.Id = await _orderRepository.SaveOrder(order);

        var unsavedItems = order.orderItems.Where(item => item.Id == null);
        await _orderRepository.SaveOrderItems((int)order.Id!, unsavedItems);
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