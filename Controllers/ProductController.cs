using System.Diagnostics;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            
            string? userId = HttpContext.Request.Query["userId"];
            
            Console.WriteLine($"User id {userId}");

            if(userId != null) {
                ViewBag.userId = userId;
            }
            
            var products = _productRepository.GetAllProducts();
            
            return View(products);
        }

        public IActionResult Request()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Upload()
        {
            return View();
        }
    }
}