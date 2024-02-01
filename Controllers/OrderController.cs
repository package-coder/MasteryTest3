using System.Diagnostics;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class OrderController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}