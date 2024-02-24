﻿using MasteryTest3.Data;
using MasteryTest3.Models;

namespace MasteryTest3.Interfaces;

public interface IOrderService
{
    Task<int?> SaveOrderRequest(Order order, List<OrderItem>? deletedOrderItems = null);
    Task DeleteOrderRequest(Order? order);
    Task<Order?> GetOrderById(int id);
    Task<IEnumerable<Order>> GetAllOrders(OrderStatus status, Role role);
}