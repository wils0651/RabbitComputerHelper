namespace RabbitComputerHelper.Repositories
{
    public abstract class AbstractRepository
    {
        private readonly DatabaseContext _context;

        protected AbstractRepository(DatabaseContext context)
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
