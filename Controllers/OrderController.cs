using System.Diagnostics;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISessionRepository _sessionRepository;

        public OrderController(IOrderRepository orderRepository, ISessionRepository sessionRepository) {
            _orderRepository = orderRepository;
            _sessionRepository = sessionRepository;
        }
        public async Task<IActionResult> Index()
        {
            int? Id = _sessionRepository.GetInt("userId");
            var orders = await _orderRepository.GetAllOrders(Id);
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(OrderItem orderItem)
        {
            if (await _orderRepository.AddOrderItem(orderItem) != 0) {
                return StatusCode(200);
            }

            return StatusCode(403);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder() {
            if (await _orderRepository.UpdateOrderStatus() != 0) {
                return StatusCode(200);
            }

            return StatusCode(403);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}