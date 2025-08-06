namespace RabbitComputerHelper.Contracts
{
    public interface IJob
    {
        public Task RunAsync(int delay);
        public string Name { get; }
    }
}
