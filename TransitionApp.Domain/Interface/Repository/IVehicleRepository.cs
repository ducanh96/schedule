using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.Model.ValueObject;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Vehicle;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IVehicleRepository
    {
        #region Read
        Task<VehicleReadModel> Get();
        VehicleReadModel GetById(int id);
        bool checkExist(int id);
        bool checkExist(string code);
        SearchVehicleReadModel GetAll(int page, int pageSize, SearchVehicleModel vehicleModel);
        #endregion

        #region Write
        VehicleModel Add(Vehicle vehicle);
        Task Delete(Identity identity);
        VehicleModel Edit(Vehicle vehicle);
        bool ImportExcel(List<Vehicle> vehicles);
       
        #endregion

    }
}
