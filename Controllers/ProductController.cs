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
        private readonly IExcelService _excelService;


        public ProductController(IOrderRepository orderRepository, IProductRepository productRepository, IExcelService excelService)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _excelService = excelService;
        }
        
        [HttpGet]
        public async Task<IActionResult> DraftOrder(int id)
        {
            var order = await _orderRepository.GetAllOrders();
            return Json(order);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRequest([FromBody] OrderViewModel orderViewModel)
        {
            var order = orderViewModel.ToOrder();
            
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

        public IActionResult DownloadProductList() { 

            var productList = _productRepository.GetAllProducts();
            var prouctListExcelFile = _excelService.GenerateExcelProductList(productList.ToList());

            return File(prouctListExcelFile, "application/vnd.ms-excel", "List of Products.xlsx");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}