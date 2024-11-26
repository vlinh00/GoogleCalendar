using AT.Server.Data;
using Microsoft.EntityFrameworkCore;
namespace AT.Server.Services.DbModel
{
    public class DbModelService : IDbModelService
    {
        private readonly ApplicationDbContext _context;
        public DbModelService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool ModelExists<T>(int id) where T : class
        {
            var dbSet = _context.Set<T>();
            return dbSet.Any(e => EF.Property<int>(e, "Id") == id);
        }
    }
}
