using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPPlus.Core.Extensions;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Driver;
using TransitionApp.Application.ResponseModel;
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
                    VehicleTypeIDs = String.Join(',', request.Driver.VehicleTypeIDs.OrderBy(x => x))
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

        public Task<StatusResponse> Delete(int id)
        {
            try
            {
                DeleteDriverCommand deleteDriverCommand = new DeleteDriverCommand
                {
                    ID = id
                };
                var result = _bus.SendCommand(deleteDriverCommand);
                return Task.FromResult(new StatusResponse
                {
                    OK = true

                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new StatusResponse
                {
                    OK = false,
                    Content = ex.Message
                });

            }
        }

        public Task<GetDriverResponse> Get(int id)
        {
            var response = new GetDriverResponse();
            try
            {
                var driver = _driverService.Get(id);
                var ids = !string.IsNullOrEmpty(driver.VehicleTypeIDs) ? driver.VehicleTypeIDs.Split(',').ToList() : null;
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
                    ID = driver.ID,
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

        public Task<DriverResponse> GetAll(SearchDriverRequest request, List<int> vehicleTypeIDs)
        {
            var response = new DriverResponse();
            try
            {
                // thieu phuong tien co the lai dc
                var searchDriverModel = new SearchDriverModel
                {
                    Code = request.Code,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    VehicleTypeIDs = string.Join('%', vehicleTypeIDs.OrderBy(x => x)).Length > 0 ? string.Join('%', vehicleTypeIDs.OrderBy(x => x)) : null
                };
                var driverAll = _driverService.GetAll(request.Page, request.PageSize, searchDriverModel);
                var result = from a in driverAll.Drivers
                             let ids = !string.IsNullOrEmpty(a.VehicleTypeIDs) ? a.VehicleTypeIDs.Split(',').ToList() : null
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
                                 ID = a.ID,
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

        public Task<StatusResponse> ImportExcel(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Task.FromResult(new StatusResponse
                    {
                        Content = "Not file",
                        OK = false
                    });
                }
                //var path = Path.Combine(
                //            Directory.GetCurrentDirectory(), "wwwroot",
                //            file.FileName);

                ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream());
                // map du lieu tu excel thanh object
                var data = excelPackage.ToList<DriverExcelModel>();
                List<DataImportDriver> dataImportVehicles = new List<DataImportDriver>();

                data.ForEach(x =>
                {
                    dataImportVehicles.Add(new DataImportDriver
                    {
                        Code = x.Code,
                        DoB = x.DoB,
                        IDCardNumber = x.IDCardNumber,
                        Name = x.Name,
                        PhoneNumber = x.PhoneNumber,
                        Sex = x.Sex,
                        StartDate = DateTime.Now,
                        Status = 0
                    });
                });

                ImportDriverCommand vehicleCommand = new ImportDriverCommand
                {
                   Drivers = dataImportVehicles

                };

                var result = _bus.SendCommand(vehicleCommand);
                return Task.FromResult(new StatusResponse
                {
                    OK = true,
                    Content = ""
                });

            }
            catch (Exception ex)
            {
                return Task.FromResult(new StatusResponse
                {
                    Content = ex.Message,
                    OK = false
                });
            }
        }

        public Task<CreateDriverResponse> Update(int id, CreateDriverRequest request)
        {
            var response = new CreateDriverResponse();
            try
            {
                EditDriverCommand vehicleCommand = new EditDriverCommand
                {
                    Id = id,
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
                    VehicleTypeIDs = String.Join(',', request.Driver.VehicleTypeIDs.OrderBy(x => x))
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
    }
}
