using System.ComponentModel.DataAnnotations;

namespace MasteryTest3.Models.ViewModel;

public class OrderViewModel
{
    public int? Id { get; set; }
    
    [Required]
    public List<OrderItem> orderItems { get; set; }

    public List<OrderItem>? deletedOrderItems { get; set; }
    
    public string? status { get; set; }

    public Order ToOrder () => new(Id, orderItems, status);
}