namespace RabbitComputerHelper.Contracts
{
    public interface IGarageEventLogRepository
    {
        public Task AddAsync<T>(T entity) where T : class;
        public Task SaveChangesAsync();
    }
}
