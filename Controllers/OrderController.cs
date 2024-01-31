using System.Diagnostics;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
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