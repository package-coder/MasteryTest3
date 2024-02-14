using MasteryTest3.Interfaces;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Fonts;
using PdfSharp.Charting;
using MasteryTest3.Models;

namespace MasteryTest3.Repositories
{
    public class ReceiptService : IReceiptService
    {
        public byte[] GenerateOrderReceipt(int id, Order order)
        {
            if (GlobalFontSettings.FontResolver == null) { 
                GlobalFontSettings.FontResolver = new FontResolverRepository();
            }

            var document = new PdfDocument();
            var page = document.AddPage();
            var graphics = XGraphics.FromPdfPage(page);

            XFont titleFont = new("Calibri", 20, XFontStyleEx.Bold);
            XFont textFont = new("Calibri", 14);

            graphics.DrawString("ORDER INFO", titleFont, XBrushes.Black, new XRect(30, 30, 0, 0));
            graphics.DrawString($"Order No: {order.Id}      CRC No: {order.crc}      Date Printed: {DateTime.Now.ToShortDateString()}", textFont, XBrushes.Black, new XRect(30, 50, 0, 0));
            graphics.DrawString($"Client Name: {order.user.name}", textFont, XBrushes.Black, new XRect(30, 90, 0, 0));
            graphics.DrawString($"Email Address: {order.user.email}", textFont, XBrushes.Black, new XRect(30, 110, 0, 0));
            graphics.DrawString($"Date of Order: {order.dateOrdered.ToShortDateString()}", textFont, XBrushes.Black, new XRect(30, 130, 0, 0));

            int xPosition = 30;
            
            //Table Headers
            graphics.DrawString("Qty", textFont, XBrushes.Black, new XRect(xPosition, 150, 50, 20), XStringFormats.TopLeft);
            graphics.DrawString("UOM", textFont, XBrushes.Black, new XRect(xPosition + 60, 150, 50, 20), XStringFormats.TopLeft);
            graphics.DrawString("Product", textFont, XBrushes.Black, new XRect(xPosition + 120, 150, 150, 20), XStringFormats.TopLeft);
            graphics.DrawString("Remark", textFont, XBrushes.Black, new XRect(xPosition + 350, 150, 150, 20), XStringFormats.TopLeft);

            //Table Body
            int yPosition = 170;
            foreach (var item in order.orderItems) {
                graphics.DrawString(item.quantity.ToString(), textFont, XBrushes.Black, new XRect(xPosition, yPosition, 50, 20), XStringFormats.TopLeft);
                graphics.DrawString(item.uom.unit, textFont, XBrushes.Black, new XRect(xPosition + 60, yPosition, 50, 20), XStringFormats.TopLeft);
                graphics.DrawString(item.name, textFont, XBrushes.Black, new XRect(xPosition + 120, yPosition, 150, 20), XStringFormats.TopLeft);
                if (item.remark != null) { 
                    graphics.DrawString(item.remark.ToString(), textFont, XBrushes.Black, new XRect(xPosition + 350, yPosition, 150, 20), XStringFormats.TopLeft);
                }

                yPosition += 20;
            }
            graphics.DrawString("************** nothing follows **************", textFont, XBrushes.Black, new XRect(150, yPosition + 20, 0, 0));
            graphics.DrawString($"Total Items: ***{order.orderItems.Count()} item(s)", textFont, XBrushes.Black, new XRect(xPosition, yPosition + 35, 0, 0));


            graphics.DrawString("For approval by:", textFont, XBrushes.Black, new XRect(400, yPosition+50, 0, 0));
            graphics.DrawString("______________________", textFont, XBrushes.Black, new XRect(400, yPosition+80, 0, 0));

           using var stream = new MemoryStream();
           document.Save(stream);

           return stream.ToArray();
        }
    }
}
