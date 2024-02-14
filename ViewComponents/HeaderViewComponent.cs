using MasteryTest3.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IUserRepository _userRepository;

        public HeaderViewComponent(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IViewComponentResult Invoke() {
            var user = Task.Run(()=>_userRepository.GetAllUsers()).Result;
            return View("Default", user);
        }
    }
}
