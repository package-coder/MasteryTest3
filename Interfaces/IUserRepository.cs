using MasteryTest3.Models;

namespace MasteryTest3.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUser();
        public Task<User> GetUserById(int id);
    }
}
