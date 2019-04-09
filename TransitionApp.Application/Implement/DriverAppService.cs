using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Driver;
using TransitionApp.Application.ResponseModel.Driver;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands.Driver;
using TransitionApp.Domain.ReadModel.Driver.DAO;
using TransitionApp.Domain.ReadModel.VehicleType;
using TransitionApp.Service.Interface;

namespace TransitionApp.Application.Implement
{
    public class DriverAppService : IDriverAppService
    {
        private readonly IDriverService _driverService;
        private readonly IMediatorHandler _bus;
        private readonly IVehicleTypeService _vehicleTypeService;

        public DriverAppService(IDriverService driverService, IMediatorHandler bus, IVehicleTypeService vehicleTypeService)
        {
            _driverService = driverService;
            _bus = bus;
            _vehicleTypeService = vehicleTypeService;
        }

        public Task<CreateDriverResponse> Create(CreateDriverRequest request)
        {
            var response = new CreateDriverResponse();
            try
            {
                CreateDriverCommand vehicleCommand = new CreateDriverCommand
                {
                    City = request.Driver.Address.City,
                    Country = request.Driver.Address.Country,
                    District = request.Driver.Address.District,
                    Street = request.Driver.Address.Street,
                    StreetNumber = request.Driver.Address.StreetNumber,
                    Code = request.Driver.Code,
                    Name = request.Driver.Name,
                    Note = request.Driver.Note,
                    PhoneNumber = request.Driver.PhoneNumber,
                    Sex = request.Driver.Sex,
                    Status = request.Driver.Status,
                    IDCardNumber = request.Driver.IDCardNumber,
                    DoB = DateTime.ParseExact(request.Driver.DoB, "dd/MM/yyyy", null),
                    StartDate = DateTime.ParseExact(request.Driver.StartDate, "dd/MM/yyyy", null),
                    VehicleTypeIDs = String.Join(',', request.Driver.VehicleTypeIDs)
                };

                var result = _bus.SendCommand(vehicleCommand);
                Task<object> status = result as Task<object>;
                var driver = (DriverModel)status.Result;
                return Task.FromResult(new CreateDriverResponse
                {
                    Data = driver.Id,
                    Message = "",
                    Success = true

                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CreateDriverResponse
                {
                    Message = ex.Message,
                    Success = false

                });

            }

        }

        public Task<GetDriverResponse> Get(int id)
        {
            var response = new GetDriverResponse();
            try
            {
                var driver = _driverService.Get(id);
                var ids = driver.VehicleTypeIDs != null ? driver.VehicleTypeIDs.Split(',').ToList() : null;
                var cityEmpty = (driver.City + driver.Country + driver.District + driver.Street + driver.StreetNumber).Length;
                var address = new
                {
                    City = driver.City,
                    Country = driver.Country,
                    District = driver.District,
                    Street = driver.Street,
                    StreetNumber = driver.StreetNumber
                };
                response.Data = new
                {
                    DriverInfo = new
                    {
                        Address = cityEmpty > 0 ? address : null,
                        driver.Code,
                        DoB = driver.DoB.HasValue ? driver.DoB.Value.ToString("dd/MM/yyyy", null) : "",
                        driver.IDCardNumber,
                        driver.Name,
                        driver.Note,
                        driver.PhoneNumber,
                        driver.Sex,
                        StartDate = driver.StartDate.HasValue ? driver.StartDate.Value.ToString("dd/MM/yyyy", null) : "",
                        driver.Status,
                        VehicleTypes = ids != null ? _vehicleTypeService.GetByIds(ids.Select(int.Parse).ToList()) : new List<VehicleTypeReadModel>()
                    },
                    ID = driver.Id,
                    UserID = driver.UserID
                };
                response.Message = "";
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return Task.FromResult(response);
        }

        public Task<DriverResponse> GetAll(SearchDriverRequest request)
        {

            var response = new DriverResponse();
            try
            {
                // thieu phuong tien co the lai dc
                var searchDriverModel = new SearchDriverModel
                {
                    Code = request.Code,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber
                };
                var driverAll = _driverService.GetAll(request.Page, request.PageSize, searchDriverModel);
                var result = from a in driverAll.Drivers
                             let ids = a.VehicleTypeIDs != null ? a.VehicleTypeIDs.Split(',').ToList() : null
                             let empty = String.Empty
                             let cityEmpty = (a.City + a.Country + a.District + a.Street + a.StreetNumber).Length
                             let address = new 
                             {
                                 City = a.City,
                                 Country = a.Country,
                                 District = a.District,
                                 Street = a.Street,
                                 StreetNumber = a.StreetNumber
                             }
                             select new
                             {
                                 DriverInfo = new
                                 {
                                     Address = cityEmpty > 0 ? address : null,
                                     a.Code,
                                     DoB = a.DoB.HasValue ? a.DoB.Value.ToString("dd/MM/yyyy", null) : empty,
                                     a.IDCardNumber,
                                     a.Name,
                                     a.Note,
                                     a.PhoneNumber,
                                     a.Sex,
                                     StartDate = a.StartDate.HasValue ? a.StartDate.Value.ToString("dd/MM/yyyy", null) : empty,
                                     a.Status,
                                     VehicleTypes = ids != null ? _vehicleTypeService.GetByIds(ids.Select(int.Parse).ToList()) : new List<VehicleTypeReadModel>()
                                 },
                                 ID = a.Id,
                                 UserID = a.UserID
                             };

                response.Data = result.ToList();
                response.Success = true;
                response.Metadata = new ResponseModel.PageResponse
                {
                    Page = driverAll.PageInfo.Page,
                    PageSize = driverAll.PageInfo.PageSize,
                    Total = driverAll.PageInfo.Total
                };
                response.Message = "";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

            }
            return Task.FromResult(response);
        }
    }
}
