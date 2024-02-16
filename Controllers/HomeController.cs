using System.Diagnostics;
using MasteryTest3.Interfaces;
using MasteryTest3.Models.ViewModel;
using MasteryTest3.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly ISessionRepository _sessionRepository;
       
        public HomeController(ILogger<HomeController> logger, IOrderRepository orderRepository, ISessionRepository sessionRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllOrders();
            return View(orders);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}