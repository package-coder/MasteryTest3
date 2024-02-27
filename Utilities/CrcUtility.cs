using MasteryTest3.Interfaces;
using MasteryTest3.Models;
using OfficeOpenXml.Style;

namespace MasteryTest3.Utilities
{
    public class CrcUtility : ICrcUtility
    {
        public int GenerateCRC(List<OrderItem> orderItems)
        {
            int crc = 0;

            foreach (var item in orderItems)
            {
                int productNameTotal = item.name.Sum(ch => ch);
                int unitTotal = item.unit.Sum(ch => ch);

                crc += productNameTotal + unitTotal + (int)item.quantity;
            }

            return crc;
        }
    }
}
