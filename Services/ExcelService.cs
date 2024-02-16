using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using OfficeOpenXml;

namespace MasteryTest3.Services
{
    public class ExcelService : IExcelService
    {
        
        public List<OrderItem> ParseExcelFile(IFormFile file)
        {
            List<OrderItem> items = new List<OrderItem>();

            using (var package = new ExcelPackage(file.OpenReadStream())) {
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                int rowCount = sheet.Dimension.Rows;
                
                for (int row = 2; row <= rowCount; row++) {
                    items.Add( new()
                    {
                        product = sheet.Cells[row, 1].Value == null ? null : new (){ Id = Convert.ToInt32(NullSafeString(sheet.Cells[row, 1].Value)) },
                        name = NullSafeString(sheet.Cells[row, 2].Value),
                        quantity = Convert.ToInt32(NullSafeString(sheet.Cells[row, 3].Value)),
                        unit = NullSafeString(sheet.Cells[row, 4].Value),
                        remark = NullSafeString(sheet.Cells[row, 5].Value),
                    }); 
                }
            }

            return items;
        }

        public byte[] GetExcelTemplate()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ExcelTemplate", "Product List Template.xlsx");
            byte[] excelTemplateFile = File.ReadAllBytes(filePath);

            return excelTemplateFile;
        }

        public byte[] GenerateExcelProductList(List<Product> products)
        {

            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Product List");

                //Headers
                worksheet.Cells[1, 1].RichText.Add("ID").Bold = true;
                worksheet.Cells[1, 2].RichText.Add("Product name").Bold = true;
                worksheet.Cells[1, 3].RichText.Add("Unit").Bold = true;

                //Populate row with data;
                int row = 2;
                foreach (var item in products)
                {
                    worksheet.Cells[row, 1].Value = item.Id;
                    worksheet.Cells[row, 2].Value = item.name;
                    worksheet.Cells[row, 3].Value = item.uom.unit.ToUpper();
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(10, 50);

                return package.GetAsByteArray();
            }
        }

        private string NullSafeString(object obj)
        {
            return (obj ?? string.Empty).ToString();
        }

        
    }
}
