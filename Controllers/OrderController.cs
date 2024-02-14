using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
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
        private readonly IOrderService _orderService;  
        private readonly IReceiptService _receiptService;

        public OrderController(IReceiptService receiptService, IOrderService orderService)
        {
            _receiptService = receiptService;
            _orderService = orderService;
        }

        public async Task<IActionResult> DownloadOrderReceipt(int id) {
            var order = await _orderService.GetOrderById(id);

            var orderPdf = _receiptService.GenerateOrderReceipt(id, order!);

            return File(orderPdf, "application/pdf", $"Order#{order!.Id}.pdf");
        }

        [HttpGet]
        public new IActionResult Request() => View();

        [HttpGet]
        public async Task<IActionResult> GetDraftOrderRequest()
        {
            var order = await _orderService.GetDraftOrderRequest();
            return Json(order);
        }
        
        [HttpPost]
        public new async Task Request([FromBody] OrderViewModel orderViewModel)
        {
            // if (!ModelState.IsValid)
            // {
            //     return View(orderViewModel);
            // }

            await _orderService.RequestOrder(orderViewModel.ToOrder(), orderViewModel.deletedOrderItems);
        }

        [HttpDelete]
        public async Task DeleteDraftOrderRequest([FromBody] Order? order)
        {
            await _orderService.DeleteDraftOrderRequest(order);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}