using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel.VehicleType;

namespace TransitionApp.Service.Interface
{
    public interface IVehicleTypeService
    {
        IEnumerable<VehicleTypeReadModel> GetAll();
        Task<VehicleTypeReadModel> Get(int id);
    }
}
