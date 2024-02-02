using MasteryTest3.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<IActionResult> Index()
        {
            var cartItems = await _cartRepository.GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCartItem(int Id) {

            await _cartRepository.RemoveOrderItem(Id);
            return StatusCode(200);

        }
    }
}
