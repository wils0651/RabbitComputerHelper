using Microsoft.EntityFrameworkCore;

namespace RabbitComputerHelper.Repositories
{
    public abstract class AbstractRepository
    {
        private readonly DbContext _context;

        protected AbstractRepository(DbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await _context.AddAsync(entity);
        }
    }
}
