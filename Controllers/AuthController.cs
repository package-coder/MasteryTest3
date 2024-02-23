using MasteryTest3.CustomAttributes;
using MasteryTest3.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    [RedirectSignedIn]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> SignIn()
        {
            var user = await _userRepository.GetAllUsers();
            return View(user);
        }
    }
}
