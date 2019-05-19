using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TransitionApp.Application.RequestModel.Driver;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Application.ResponseModel.Driver;

namespace TransitionApp.Application.Interface
{
    public interface IDriverAppService
    {
        #region Read
        Task<DriverResponse> GetAll(SearchDriverRequest request, List<int> VehicleTypeIDs);
        Task<GetDriverResponse> Get(int id);
        #endregion
        #region Write
        Task<CreateDriverResponse> Create(CreateDriverRequest request);
        Task<CreateDriverResponse> Update(int id, CreateDriverRequest request);
        Task<StatusResponse> ImportExcel(IFormFile file);
        Task<StatusResponse> Delete(int id);
        Task<StatusResponse> ResetPassword(AccountRequest request);
        #endregion
    }
}
