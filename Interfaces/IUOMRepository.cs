using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IUOMRepository
    {
        public Task<IEnumerable<UOM>> GetAllUOM();
    }
}
