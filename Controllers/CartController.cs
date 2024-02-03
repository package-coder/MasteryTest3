using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUOMRepository _uomRepository;
        private readonly IOrderRepository _orderRepository;

        public CartController(ICartRepository cartRepository, IUOMRepository uomRepository, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _uomRepository = uomRepository;
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
            var cartItems = await _cartRepository.GetCartItems();
            return View(cartItems);
        }

        public async Task<IActionResult> Update(int Id) {
            var viewModel = new UpdateItemViewModel
            {
                uomList = await _uomRepository.GetAllUOM(),
                orderItem = await _orderRepository.GetOrderItem(Id)
            };

            return View(viewModel);    
        }

        [HttpPost]
        public async Task<IActionResult> Update(OrderItem orderItem) {

            await _orderRepository.UpdateOrderItem(orderItem);
            return StatusCode(200);
        
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCartItem(int Id) {

            await _cartRepository.RemoveOrderItem(Id);
            return StatusCode(200);

        }
    }
}
