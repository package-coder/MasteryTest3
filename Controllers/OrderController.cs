﻿using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using MasteryTest3.Data;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using MasteryTest3.Models.ViewModel;
using MasteryTest3.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PdfSharp.Pdf.Advanced;

namespace MasteryTest3.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;  
        private readonly IReceiptService _receiptService;
        private readonly IExcelService _excelService;

        public OrderController(IReceiptService receiptService, IOrderService orderService, IExcelService excelService)
        {
            _receiptService = receiptService;
            _orderService = orderService;
            _excelService = excelService;
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
        public async Task<IActionResult> Index(OrderStatus status = OrderStatus.DRAFT, Role role = Role.REQUESTOR)
        {
            var orders = await _orderService.GetAllOrders(status, role);
            return View(orders);
        }
        
        [HttpGet] 
        public new IActionResult Request() => View();
        
        [HttpPost] 
        public new async Task Request([FromBody] OrderViewModel orderViewModel)
        {
           await _orderService.RequestOrder(orderViewModel.ToOrder(), orderViewModel.deletedOrderItems);
        }

        [HttpDelete]
        public async Task DeleteOrderRequest([FromBody] Order? order)
        {
            await _orderService.DeleteOrderRequest(order);
        }

        [HttpPost]
        public IActionResult UploadExcelFile(IFormFile productList) {

            var items = _excelService.ParseExcelFile(productList);
            return Json(items);
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