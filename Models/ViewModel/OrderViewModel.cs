using System.ComponentModel.DataAnnotations;

namespace MasteryTest3.Models.ViewModel;

public class OrderViewModel
{
    public int? Id { get; set; }
    
    public List<OrderItem> orderItems { get; set; }

    public List<OrderItem>? deletedOrderItems { get; set; }

    public string? attachment { get; set; }
    
    public string? status { get; set; }

    public bool process { get; set; } = false;

    public Order ToOrder () => new(Id, orderItems, status, attachment);
}