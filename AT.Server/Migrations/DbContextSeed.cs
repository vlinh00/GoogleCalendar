using AT.Server.Data;
using AT.Share.Model;
using System.Data;

namespace AT.Server.Migrations
{
    public static class DbContextSeed
    {
        public static async Task SeedData(ApplicationDbContext context)
        {
            if (!context.GroupUsers.Any())
            {
                context.GroupUsers.AddRange(
                    new GroupUser { Id = 1, GroupName = "Admin", Description = "Full control", CreatedBy = "System", CreatedDate = DateTime.Now },
                    new GroupUser { Id = 2, GroupName = "Manager", Description = "Can manage staff", CreatedBy = "System", CreatedDate = DateTime.Now },
                    new GroupUser { Id = 3, GroupName = "Staff", Description = "Can view task and add progress", CreatedBy = "System", CreatedDate = DateTime.Now }
                );
            }
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Id = 1, Name = "Sales" },
                    new Department { Id = 2, Name = "Lab" },
                    new Department { Id = 3, Name = "Xưởng" }
                ); 
            }
            await context.SaveChangesAsync();

        }
    }
}
