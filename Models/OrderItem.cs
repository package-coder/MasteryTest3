namespace MasteryTest3.Models
{
    public class OrderItem
    {
        public int? Id { get; set; }
        public Order order { get; set; }
        public Product? product { get; set; }
        public UOM uom { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string? remark { get; set; } 
    }
}
