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
        private readonly ISessionService _sessionService;
        private readonly IUserRepository _userRepository;

        public HomeController(ISessionService sessionService, IUserRepository userRepository)
        {
            _sessionService = sessionService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (_sessionService.GetInt("userId") == null) {
                var user = await _userRepository.GetUserById(2);

                if (user != null) {
                    _sessionService.SetInt("userId", user.Id);
                    _sessionService.SetString("userName", user.name);
                    _sessionService.SetInt("roleId", user.role.id);
                    _sessionService.SetString("roleName", user.role.name);
                    _sessionService.SetInt("visibilityLevel", user.role.visibilityLevel);
                }
            }
            return Redirect("/order?status=DRAFT&role=1");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}