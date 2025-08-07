namespace RabbitComputerHelper.Contracts
{
    public interface IJob
    {
        public Task RunAsync();
        public string Name { get; }
    }
}
