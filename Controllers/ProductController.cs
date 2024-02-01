using System.Diagnostics;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = _productRepository.GetAllProducts();
            return View(products);
        }

        public IActionResult AddItem(int Id) {
            return StatusCode(200);
        }

        public IActionResult Request()
        {
            return View();
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