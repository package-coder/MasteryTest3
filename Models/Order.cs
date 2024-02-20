namespace MasteryTest3.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public List<OrderItem> orderItems { get; set; } = new();
        public User user { get; set; }
        public int crc { get; set; }
        public string status { get; set; }
        public int totalItems { get; set; }
        public double totalAmount { get; set; }
        public string attachment { get; set; }
        public DateTime dateOrdered { get; set; }
        public DateTime datePrinted { get; set; }

        public Order(int? id, List<OrderItem> orderItems, string status)
        {
            Id = id;
            this.orderItems = orderItems;
            this.status = status;
        }

        public Order()
        {
        }
    }
}
