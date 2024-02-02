using System.Diagnostics;
using System.Text;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProductController : Controller
    {
        private readonly IUOMRepository _uomRepository;
        private readonly IOrderRepository _orderRepository;

        public ProductController(IUOMRepository uomRepository, IOrderRepository orderRepository)
        {
            _uomRepository = uomRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Request()
        {
            var uom = await _uomRepository.GetAllUOM();
            return View(uom);
        }

        [HttpPost]
        public async Task<IActionResult> Request(OrderItem item) {
            if (await _orderRepository.AddOrderItem(item) != 0) {
                return StatusCode(200);
            }

            return StatusCode(403);
           
        }

        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}