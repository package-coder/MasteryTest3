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
        private readonly IOrderRepository _orderRepository;
       
        public HomeController( IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllOrders();
            return View(orders);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}