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
    }
}
