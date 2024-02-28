using MasteryTest3.Data;

namespace MasteryTest3.Models
{
    public class OrderApprovalLog
    {
        public int Id { get; set; }
        public User user { get; set; }
        public OrderStatus status { get; set; }
        public Order order { get; set; }
        public DateTime? dateLogged { get; set; }
        public string? remark { get; set; }
    }
}
