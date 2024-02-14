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

        [HttpGet]
        public async Task<IActionResult> DraftOrder()
        {
            var order = await _orderRepository.GetDraftOrder();
            return Json(order);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRequest([FromBody] OrderViewModel orderViewModel)
        {
            var order = orderViewModel.ToOrder();
            var id = await _orderRepository.SaveOrder(order);
            if (id > 0)
            {
                await _orderRepository.SaveOrderItem(orderId: id, order.orderItems);
            } 
            
            return StatusCode(200);
        }
        
        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] OrderViewModel orderViewModel)
        {
            var order = orderViewModel.ToOrder();
            if (await _orderRepository.SaveOrder(order) != 0) {
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