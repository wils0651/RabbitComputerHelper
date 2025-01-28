namespace RabbitComputerHelper.Contracts;

public interface IProbeDataRepository
{
    public Task AddAsync<T>(T entity) where T : class;

    public Task SaveChangesAsync();
}