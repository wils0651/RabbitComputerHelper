﻿using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories
{
    public class GarageDistanceRepository : AbstractRepository, IGarageDistanceRepository
    {
        private readonly GarageSensorContext _context;

        public GarageDistanceRepository(GarageSensorContext context) : base(context)
        {
            _context = context;
        }
    }
}
