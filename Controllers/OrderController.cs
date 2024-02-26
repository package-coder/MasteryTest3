using System.Diagnostics;
using MasteryTest3.CustomAttributes;
using MasteryTest3.Data;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MasteryTest3.Controllers
{
    [RedirectSignedOut]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;  
        private readonly IReceiptService _receiptService;
        private readonly IExcelService _excelService;
        private readonly ISessionService _sessionService;

        public OrderController(IReceiptService receiptService, IOrderService orderService, IExcelService excelService, ISessionService sessionService)
        {
            _receiptService = receiptService;
            _orderService = orderService;
            _excelService = excelService;
            _sessionService = sessionService;
        }

        public async Task<IActionResult> DownloadOrderReceipt(int id) {
            var order = await _orderService.GetOrderById(id);

            var orderPdf = _receiptService.GenerateOrderReceipt(id, order!);

            return File(orderPdf, "application/pdf", $"Order#{order!.Id}.pdf");
        }
        
        
        [HttpGet] 
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            return Json(order);
        }

        [HttpGet]
        public async Task<IActionResult> Index(OrderStatus status, Role role)
        {
            var orders = await _orderService.GetAllOrders(status, role);
            return View(orders);
        }
        
        [HttpGet]
        public async Task<IActionResult> Process(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null) return RedirectToAction("Error");
            return View(order);
        }
        
        [HttpGet]
        public new async Task<IActionResult> Request(int id, Role role)
        {
            var session = _sessionService.GetSessionUser();
            var order = await _orderService.GetOrderById(id);
            
            if (order == null) return View();

            return role switch
            {
                Role.APPROVER when session!.role.id != (int)Role.APPROVER => RedirectToAction("Error"),
                Role.REQUESTER when session!.id != order.user.Id => RedirectToAction("Error"),
                _ => View(order)
            };
        }

        [HttpPost] 
        public new async Task<RedirectToActionResult> Request([FromBody] OrderViewModel orderViewModel)
        {
            var order = orderViewModel.ToOrder();
            order.Id = await _orderService.SaveOrderRequest(order, orderViewModel.deletedOrderItems);

            if (orderViewModel.process)
                return RedirectToAction("process", "order",new { id = order.Id });
            
            return RedirectToAction("index", "order", new { order.status, role = Role.REQUESTER });
        }

        [HttpDelete]
        public async Task DeleteOrderRequest([FromBody] Order? order)
        {
            await _orderService.DeleteOrderRequest(order);
        }

        [HttpPost]
        public IActionResult UploadExcelFile(IFormFile file) {

            if (_excelService.validateExcelFile(file))
            {
                var items = _excelService.ParseExcelFile(file);
                return Json(items);
            }
            else {
                return StatusCode(400);
            }
            
        }

        public IActionResult DownloadExcelTemplate() {

            var templateFile = _excelService.GetExcelTemplate();
            return File(templateFile, "application/vnd.ms-excel", "Product List Template.xlsx");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}