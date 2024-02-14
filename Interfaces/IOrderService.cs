using MasteryTest3.Models;

namespace MasteryTest3.Interfaces;

public interface IOrderService
{
    Task RequestOrder(Order order);
    
    Task<Order?> GetOrderById(int id);
    Task<Order?> GetDraftOrderRequest();
}