using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Driver;
using TransitionApp.Application.ResponseModel.Driver;
using TransitionApp.Service.Interface;

namespace TransitionApp.Application.Implement
{
    public class DriverAppService : IDriverAppService
    {
        private readonly IDriverService _driverService;
        public DriverAppService(IDriverService driverService)
        {
            _driverService = driverService;
        }
        public async Task<DriverResponse> GetAll(SearchDriverRequest request)
        {

            var response = new DriverResponse();
            try
            {
                var driverAll = await _driverService.GetAll();
                var result = from a in driverAll
                         select new
                         {
                             DriverInfo = new
                             {
                                 Code = a.Code,
                                 Name = a.Name,
                                 PhoneNumber = a.PhoneNumber,
                                 Status = a.Status
                             },
                             ID = a.Id,
                             UserID = a.Id

                         };
                response.Data = result.ToList();
                response.Success = true;
                response.Metadata = new ResponseModel.PageResponse
                {
                    Page = request.Page,
                    PageSize = request.PageSize
                };
                response.Message = "";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                
            }
            return response;
        }
    }
}
