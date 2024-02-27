using MasteryTest3.Data;
using MasteryTest3.Interfaces;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderService _orderService;
        private readonly ISessionService _sessionService;

        public SidebarViewComponent(IUserRepository userRepository, IOrderService orderService, ISessionService sessionService)
        {
            _userRepository = userRepository;
            _orderService = orderService;
            _sessionService = sessionService;
        }

        public async Task<IViewComponentResult> InvokeAsync() {

            var role = Request.Query["role"].ToString();
            var pendingApprovalCount = 0;
            var sessionUser = _sessionService.GetSessionUser();

            if (sessionUser.role.name != "requester") {
                var orders = await _orderService.GetAllOrders(OrderStatus.FOR_APPROVAL, Role.APPROVER);
                pendingApprovalCount = orders.Count();
            }

            var ViewModel = new SidebarViewModel()
            {
                users = await _userRepository.GetAllUsers(),
                pendingApprovalCount = pendingApprovalCount
            };

            return View("Default", ViewModel);
        }
    }
}
