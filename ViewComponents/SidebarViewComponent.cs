using MasteryTest3.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IUserRepository _userRepository;

        public SidebarViewComponent(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IViewComponentResult Invoke() {
            var user = Task.Run(()=>_userRepository.GetAllUsers()).Result;
            return View("Default", user);
        }
    }
}
