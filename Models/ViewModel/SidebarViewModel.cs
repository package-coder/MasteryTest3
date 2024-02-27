namespace MasteryTest3.Models.ViewModel
{
    public class SidebarViewModel
    {
        public IEnumerable<User> users {  get; set; }
        public int pendingApprovalCount { get; set; } = 0;
        
    }
}
