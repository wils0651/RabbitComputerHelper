using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories
{
    public class GarageEventLogRepository : AbstractRepository, IGarageEventLogRepository
    {
        public GarageEventLogRepository(GarageSensorContext context) : base(context)
        {
        }
    }
}
