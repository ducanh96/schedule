using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TransitionApp.Application.Queries
{
    public interface IVehicleQueries
    {
        Task<List<Vehicle>> GetAll();
    }
}
