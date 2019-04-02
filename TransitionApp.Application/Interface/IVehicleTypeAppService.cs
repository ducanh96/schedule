using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Application.RequestModel.VehicleType;
using TransitionApp.Application.ResponseModel.Vehicle;
using TransitionApp.Application.ResponseModel.VehicleType;
using TransitionApp.Domain.ReadModel.VehicleType;

namespace TransitionApp.Application.Interface
{
    public interface IVehicleTypeAppService
    {
        #region Read 
        Task<AllVehicleTypeResponse> GetAll();

        #endregion
        #region Write 
        Task<VehicleTypeResponse> Create(CreateVehicleTypeRequest request);
        Task<VehicleTypeResponse> Delete(int request);
        #endregion
    }
}
