using MasteryTest3.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    public class UserController : Controller
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        public UserController(ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> ChangeClient()
        {
            var Id = Convert.ToInt32(HttpContext.Request.Query["userId"]);
            var user = await _userRepository.GetUserById(Id);

            if (user != null) { 
                _sessionRepository.SetInt("userId", user.Id);
                _sessionRepository.SetString("userName", user.name);
                _sessionRepository.SetInt("role", user.role.Id);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
