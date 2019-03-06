using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Entity;

namespace TransitionApp.Domains
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAll();
    }
}
