using Microsoft.EntityFrameworkCore;
using Test_task.Models;

namespace Test_task.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
      

      //public DbSet<User> Users { get; set; }
      //public DbSet<UserGroup> UserGroups { get; set; }
      //public DbSet<UserState> UserStates { get; set; }
      
      public DbSet<User> Users => Set<User>();
      public DbSet<UserGroup> UserGroups => Set<UserGroup>();
      public DbSet<UserState> UserStates => Set<UserState>();
    }
    
}