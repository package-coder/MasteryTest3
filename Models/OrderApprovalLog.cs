using System.Security.Cryptography.Xml;

namespace MasteryTest3.Models
{
    public class OrderApprovalLog
    {
        public int Id { get; set; }
        public User user { get; set; }
        public string status { get; set; }
        public DateTime? approvalDate { get; set; }
        public string? remark { get; set; }
    }
}
