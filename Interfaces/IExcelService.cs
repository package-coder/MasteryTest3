using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IExcelService
    {
        public List<OrderItem> ParseExcelFile(IFormFile file);
        public byte[] GetExcelTemplate();
        public byte[] GenerateExcelProductList(List<Product> products);
    }
}
