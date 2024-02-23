using System.Diagnostics;
using MasteryTest3.CustomAttributes;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    [RedirectSignedOut]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {
            return Redirect("/order?status=DRAFT&role=1");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}