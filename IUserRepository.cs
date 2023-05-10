using Test_task.Models;

namespace Test_task.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> BlockUserAsync(int id);
        //Task<bool> CheckAdmin();
        Task<bool> CheckAdminUserLimitAsync();
        Task<bool> IsLoginUniqueAsync(string login);
    }
}