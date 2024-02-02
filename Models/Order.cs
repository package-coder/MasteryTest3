namespace MasteryTest3.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User user { get; set; }
        public string crc { get; set; }
        public string status { get; set; }
        public int totalItems { get; set; }
        public DateTime dateOrdered { get; set; }
        public DateTime datePrinted { get; set; }
    }
}
