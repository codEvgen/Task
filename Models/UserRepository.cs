using Microsoft.EntityFrameworkCore;
using Test_task.Data;
using Test_task.Models;
using Test_task.Repositories;
using System.Linq;
using System.Collections.Generic;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddUserAsync(User user)
    {
        
        user.CreatedDate = DateTime.UtcNow;
        user.UserStateId = _context.UserStates.First(state => state.Code == "Active").Id;
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
 



     public async Task<User?> GetUserAsync(int id)
     {
         return await _context.Users
             .Include(u => u.UserGroup)
             .Include(u => u.UserState)
             .FirstOrDefaultAsync(u => u.Id == id);
     }


     public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.UserGroup)
            .Include(u => u.UserState)
            .ToListAsync();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> BlockUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            user.UserStateId = _context.UserStates.First(state => state.Code == "Blocked").Id;
            await _context.SaveChangesAsync();
        }

        return user;
    }

    public async Task<bool> CheckAdminUserLimitAsync()
    {
        var adminGroupId = _context.UserGroups.First(group => group.Code == "Admin").Id;
        return await _context.Users.CountAsync(u => u.UserGroupId == adminGroupId) < 1;
    }


    public async Task<bool> IsLoginUniqueAsync(string login)
    {
        return !await _context.Users.AnyAsync(u => u.Login == login);
    }
}