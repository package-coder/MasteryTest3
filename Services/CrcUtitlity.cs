using MasteryTest3.Interfaces;
using MasteryTest3.Models;

namespace MasteryTest3.Services
{
    public class CrcUtitlity : ICrcUtility
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
