using System.Collections.Generic;
using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.VehicleType;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IVehicleTypeRepository
    {
        #region Write
        Task<bool> Add(VehicleType vehicleType);
        Task<bool> Delete(int vehicleTypeId);
        #endregion
        #region Read
        Task<VehicleTypeReadModel> Get(int id);
        IEnumerable<VehicleTypeReadModel> GetAll();
        IEnumerable<VehicleTypeReadModel> GetByIds(List<int> ids);

        #endregion
    }
}
