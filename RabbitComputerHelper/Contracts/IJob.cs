namespace RabbitComputerHelper.Jobs
{
    public interface IJob
    {
        public Task RunAsync();
        public string Name { get; }
    }
}
