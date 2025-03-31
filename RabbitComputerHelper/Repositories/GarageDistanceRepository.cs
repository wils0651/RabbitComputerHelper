using RabbitComputerHelper.Contracts;

namespace RabbitComputerHelper.Repositories
{
    public class GarageDistanceRepository : AbstractRepository, IGarageDistanceRepository
    {
        private readonly DatabaseContext _context;

        public GarageDistanceRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
