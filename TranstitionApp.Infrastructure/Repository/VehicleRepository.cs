using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Entity;
using TransitionApp.Domains;

namespace TranstitionApp.Infrastructure.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        public Task<List<Vehicle>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
