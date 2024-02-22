using MasteryTest3.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MasteryTest3.Controllers
{
    public class UserController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IUserRepository _userRepository;

        public UserController(ISessionService sessionService, IUserRepository userRepository)
        {
            _sessionService = sessionService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> ChangeClient()
        {
            var Id = Convert.ToInt32(HttpContext.Request.Query["userId"]);
            var user = await _userRepository.GetUserById(Id);

            if (user != null) {
                _sessionService.SetInt("userId", user.Id);
                _sessionService.SetString("userName", user.name);
                _sessionService.SetString("roleName", user.role.name);
                _sessionService.SetInt("roleId", user.role.id);
                _sessionService.SetInt("visibilityLevel", user.role.visibilityLevel);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
