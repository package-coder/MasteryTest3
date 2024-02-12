using System.Diagnostics;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf.Advanced;

namespace MasteryTest3.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPdfRepository _pdfRepository;

        public OrderController(IOrderRepository orderRepository, IPdfRepository pdfRepository)
        {
            _orderRepository = orderRepository;
            _pdfRepository = pdfRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(OrderItem orderItem)
        {
            if (await _orderRepository.AddOrderItem(orderItem) != 0) {
                return StatusCode(200);
            }

            return StatusCode(403);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder() {
            if (await _orderRepository.UpdateOrderStatus() != 0) {
                return StatusCode(200);
            }

            return StatusCode(403);
        }

        public async Task<IActionResult> DownloadOrderReceipt(int Id) {
            var order = await _orderRepository.GetOrderById(Id);
            var orderItems = await _orderRepository.GetOrderAllOrderItems(Id);

            var orderPdf = _pdfRepository.GenerateOrderReceipt(Id, order, orderItems);

            return File(orderPdf, "application/pdf", $"Order#{order.Id}.pdf");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}