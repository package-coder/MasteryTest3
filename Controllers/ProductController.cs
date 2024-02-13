using System.Diagnostics;
using System.Text;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace MasteryTest3.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProductController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;


        public ProductController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public IActionResult Request()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Request(OrderItem item) {
            if (await _orderRepository.AddOrderItem(item) != 0) {
                return StatusCode(200);
            }

            return StatusCode(403);
           
        }

        public IActionResult GetAllProducts() {
            var products = _productRepository.GetAllProducts();
            return Json(products);
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