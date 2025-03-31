namespace RabbitComputerHelper.Contracts
{
    public interface IGarageDistanceRepository
    {
        public Task AddAsync<T>(T entity) where T : class;

        public Task SaveChangesAsync();
    }
}
