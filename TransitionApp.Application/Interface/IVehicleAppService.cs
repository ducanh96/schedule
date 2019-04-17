using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransitionApp.Application.RequestModel;
using TransitionApp.Application.RequestModel.Vehicle;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Application.ResponseModel.Vehicle;
using TransitionApp.Domain.ReadModel;

namespace TransitionApp.Application.Interface
{
    public interface IVehicleAppService
    {
        

        #region Read
        Task<VehicleResponse> GetAsync();
        //   Task<VehicleResponse> GetById(GetVehicleRequest request);
        Task<SearchVehicleResponse> GetAll(SearchVehicleRequest request);
        Task<GetVehicleResponse> Get(int id);
        
        #endregion

        #region Write
        //BaseResponse Create(CreateVehicleRequest vehicleRequest);
        Task<StatusResponse> Create(CreateVehicleRequests vehicleRequest);

        Task<StatusResponse> Edit(CreateVehicleRequests vehicleEditRequest);
        Task<StatusResponse> Delete(DeleteVehicleRequest vehicleRequest);
        Task<StatusResponse> ImportExcel(IFormFile file);

        #endregion

    }
}
