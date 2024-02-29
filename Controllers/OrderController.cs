using System.Diagnostics;
using MasteryTest3.CustomAttributes;
using MasteryTest3.Data;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using MasteryTest3.Utilities;
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
        private readonly IFileEncoderUtility _fileEncoderUtility;
        private readonly IOrderApprovalRepository _orderApprovalRepository;

        public OrderController(IReceiptService receiptService, IOrderService orderService, IExcelService excelService, ISessionService sessionService, IFileEncoderUtility fileEncoderUtility, IOrderApprovalRepository orderApprovalRepository)
        {
            _receiptService = receiptService;
            _orderService = orderService;
            _excelService = excelService;
            _sessionService = sessionService;
            _fileEncoderUtility = fileEncoderUtility;
            _orderApprovalRepository = orderApprovalRepository;
        }

        public async Task<IActionResult> DownloadOrderReceipt(int id) {
            var order = await _orderService.GetOrderById(id);
            var approvals = await _orderApprovalRepository.GetAllApprovals(id);

            var orderPdf = _receiptService.GenerateOrderReceipt(order!, approvals);

            return File(orderPdf, "application/pdf", $"Order#{order!.Id}.pdf");
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(OrderStatus status, Role role)
        {
            if (status is OrderStatus.COMPLETED or OrderStatus.REQUESTED)
            {
                var orderLogs = await _orderService.GetAllOrderLogs(role);
                return View($"~/Views/Order/{role}/{status}.cshtml", orderLogs);
            }
            
            var orders = await _orderService.GetAllOrders(status, role);
            return View($"~/Views/Order/{role}/Index.cshtml", orders);
        }
        
        [HttpGet]
        public async Task<IActionResult> Detail(int id, Role role)
        {
            var session = _sessionService.GetSessionUser();
            var order = await _orderService.GetOrderById(id);
            
            if (order == null)
                return RedirectToAction("Error");
            if (role == Role.APPROVER && session!.role.name != Role.APPROVER.ToString() && order.status != OrderStatus.DRAFT.ToString())
                return RedirectToAction("Error");
            if (role == Role.REQUESTER && session!.id != order.user.Id)
                return RedirectToAction("Error");

            return View("~/Views/Order/Detail.cshtml", order);
        }
        
        [HttpGet]
        public async Task<IActionResult> Save(int? id)
        {
            if(id == null) return View("~/Views/Order/Save.cshtml");
            
            var session = _sessionService.GetSessionUser();
            var order = await _orderService.GetOrderById((int)id);
            
            if (order == null)
                return RedirectToAction("Error");
            if (session!.id != order.user.Id)
                return RedirectToAction("Error");

            return View("~/Views/Order/Save.cshtml", order);
        }

        [HttpPost] 
        public async Task<IActionResult> Save([FromBody] OrderViewModel orderViewModel)
        {
            var order = orderViewModel.ToOrder();

            if (!_fileEncoderUtility.VerifyEncodedPdf(order.attachment))
                return StatusCode(422);

            order.Id = await _orderService.SaveOrderRequest(order, orderViewModel.deletedOrderItems);

            if (orderViewModel.process)
                return RedirectToAction("process", "order",new { id = order.Id });
            
            return RedirectToAction("index", "order", new { order.status, role = Role.REQUESTER });
        }

        [HttpGet]
        public async Task<IActionResult> Process(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null) return RedirectToAction("Error");
            return View(order);
        }
        
        [HttpPost]
        public async Task<IActionResult> Complete(int id, OrderStatus status, string? remark)
        {
            await _orderService.CompleteOrderRequest(id, remark, status);
            
            return RedirectToAction("index", "order", new { status = OrderStatus.APPROVED, role = Role.APPROVER });
        }

        [HttpDelete]
        public async Task DeleteOrderRequest(int id)
        {
            await _orderService.DeleteOrderRequest(id);
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

        public async Task<IActionResult> DownloadOrderAttachment(int id) {
            var order = await _orderService.GetOrderById(id);
            byte[] pdfBytes = Convert.FromBase64String(order.attachment);

            return File(pdfBytes, "application/pdf", true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}