using EPPlus.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel;
using TransitionApp.Application.RequestModel.Vehicle;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Application.ResponseModel.Vehicle;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands.Vehicle;
using TransitionApp.Domain.Notifications;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Driver;
using TransitionApp.Domain.ReadModel.Vehicle;
using TransitionApp.Service.Interface;
using static TransitionApp.Application.Shared.Common;

namespace TransitionApp.Application.Implement
{
    public class VehicleAppService : IVehicleAppService
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMediatorHandler _bus;
        private readonly DomainNotificationHandler _notifications;
        private readonly IVehicleTypeService _vehicleTypeService;
        private readonly IDriverService _driverService;

        public VehicleAppService(IVehicleService vehicleService, IMediatorHandler bus
            , INotificationHandler<DomainNotification> notifications
            , IDriverService driverService
            , IVehicleTypeService vehicleTypeService)
        {
            _vehicleService = vehicleService;
            _bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
            _vehicleTypeService = vehicleTypeService;
            _driverService = driverService;
        }

        #region Write
        //public BaseResponse Create(CreateVehicleRequest vehicleRequest)
        //{
        //    CreateVehicleCommand vehicleCommand = new CreateVehicleCommand
        //    {
        //        Volume = vehicleRequest.Volume,
        //        Capacity = vehicleRequest.Capacity,
        //        Image = vehicleRequest.Image,
        //        LicensePlate = vehicleRequest.LicensePlate,
        //        TypeVehicle = vehicleRequest.TypeVehicle
        //    };
        //    var result = _bus.SendCommand(vehicleCommand);
        //    Task<object> status = result as Task<object>;
        //    var isComplete = (bool)status.Result;
        //    if (isComplete)
        //    {
        //        return new BaseResponse(ResponseCode.OK.ToString(), code: ResponseCode.OK);
        //    }
        //    return new BaseResponse(ResponseCode.Fail.ToString(), code: ResponseCode.Fail);

        //    //if (_notifications.HasNotifications())
        //    //{

        //    //    Console.Write("ae oi");

        //    //    if(_notifications.GetNotifications().Any(x=>x.TypeNotification == TypeNotification.Success))
        //    //    {
        //    //        response.Code = (int)ResponseCode.OK;
        //    //        var message = _notifications.GetNotifications().FirstOrDefault().Value ?? "";
        //    //        response.Message = message as string;
        //    //    }
        //    //    if (_notifications.GetNotifications().Any(x => x.TypeNotification == TypeNotification.Fail))
        //    //    {
        //    //        response.Code = (int)ResponseCode.Fail;
        //    //        var message = _notifications.GetNotifications().FirstOrDefault().Value ?? "";
        //    //        response.Message = message as string;
        //    //    }

        //    //}

        //}

        public Task<StatusResponse> Create(CreateVehicleRequests vehicleRequest)
        {
            try
            {
                var existCode = _vehicleService.checkExist(vehicleRequest.Code);
                if (existCode)
                {
                    return Task.FromResult(new StatusResponse
                    {
                        OK = false,
                        Content = "Trùng code"
                    });
                }

                CreateVehicleCommand vehicleCommand = new CreateVehicleCommand
                {
                    LicensePlate = vehicleRequest.LicensePlate,
                    Code = vehicleRequest.Code,
                    MaxLoad = vehicleRequest.MaxLoad,
                    Name = vehicleRequest.Name,
                    Note = vehicleRequest.Note,
                    TypeID = vehicleRequest.TypeID,
                    DriverID = vehicleRequest.DriverID

                };
                var result = _bus.SendCommand(vehicleCommand);
                Task<object> status = result as Task<object>;
                var id = (VehicleModel)status.Result;
                return Task.FromResult(new StatusResponse
                {
                    OK = true,
                    Content = id.Id.ToString()
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


        public Task<StatusResponse> Delete(DeleteVehicleRequest vehicleRequest)
        {
            try
            {
                DeleteVehicleCommand deleteVehicleCommand = new DeleteVehicleCommand
                {
                    ID = vehicleRequest.Id
                };
                var result = _bus.SendCommand(deleteVehicleCommand);
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

        public Task<StatusResponse> Edit(CreateVehicleRequests vehicleRequest)
        {
            try
            {
                EditVehicleCommand vehicleCommand = new EditVehicleCommand
                {
                    Code = vehicleRequest.Code,
                    DriverID = vehicleRequest.DriverID,
                    ID = int.Parse(vehicleRequest.ID),
                    LicensePlate = vehicleRequest.LicensePlate,
                    MaxLoad = vehicleRequest.MaxLoad,
                    Name = vehicleRequest.Name,
                    Note = vehicleRequest.Note,
                    TypeID = vehicleRequest.TypeID,
                    Volume = string.Join('-', vehicleRequest.MaxVolume.Select(x=> x.HasValue? x:0))
                };
                var result = _bus.SendCommand(vehicleCommand);
                Task<object> status = result as Task<object>;
                var id = (VehicleModel)status.Result;
                return Task.FromResult(new StatusResponse
                {
                    OK = true,
                    Content = id.Id.ToString()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.FromResult(new StatusResponse
                {
                    OK = false,
                    Content = ex.Message
                });
            }
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
                var data = excelPackage.ToList<VehicleExcelModel>();
                List<DataImportVehicle> dataImportVehicles = new List<DataImportVehicle>();

                data.ForEach(x =>
                {
                    dataImportVehicles.Add(new DataImportVehicle
                    {
                        Code = x.Code,
                        DriverID = _driverService.GetByCode(x.CodeDriver)?.ID,
                        LicensePlate = x.LicensePlate,
                        MaxLoad = x.MaxLoad,
                        Name = x.Name,
                        Note = x.Note,
                        TypeVehicleID = _vehicleTypeService.GetByCode(x.TypeVehicle)?.ID
                        
                    });
                });

                ImportVehicleCommand vehicleCommand = new ImportVehicleCommand
                {
                    Vehicles = dataImportVehicles
                   
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

        #endregion

        #region Read

        public Task<GetVehicleResponse> Get(int id)
        {
            try
            {
                var vehicle = _vehicleService.GetById(id);
                var result = new
                {
                    Code = vehicle.Code,
                    Driver = _driverService.Get(vehicle.Driver),
                    ID = vehicle.Id,
                    LicensePlate = vehicle.LicensePlate,
                    MaxLoad = vehicle.MaxLoad,
                    Type = _vehicleTypeService.Get(vehicle.VehicleType),
                    Name = vehicle.Name,
                    Note = vehicle.Note,
                    MaxVolume = vehicle.Volume?.Split('-').Select(int.Parse)
                };

                return Task.FromResult(new GetVehicleResponse
                {
                    Status = new StatusResponse
                    {
                        Content = "",
                        OK = true
                    },
                    Vehicles = new List<object>
                    {
                        result
                    }
                });
            }
            catch (Exception ex)
            {

                return Task.FromResult(new GetVehicleResponse
                {
                    Status = new StatusResponse
                    {
                        Content = ex.Message,
                        OK = false
                    },

                });
            }

        }


        public Task<SearchVehicleResponse> GetAll(SearchVehicleRequest request)
        {
            try
            {
                var vehicleModel = new SearchVehicleModel
                {
                    Code = request.code,
                    LicensePlate = request.licensePlate,
                    Name = request.name,
                    VehicleType = request.vehicleTypeID
                };
                var infoSearchVehicle = _vehicleService.GetAll(request.Page, request.PageSize, vehicleModel);
                var allVehicle = from a in infoSearchVehicle.Vehicles
                                 select new
                                 {
                                     Code = a.Code,
                                     Type = _vehicleTypeService.Get(a.VehicleType),
                                     Name = a.Name,
                                     LicensePlate = a.LicensePlate,
                                     ID = a.Id,
                                     MaxLoad = a.MaxLoad,
                                     Driver = _driverService.Get(a.Driver),
                                     MaxVolume = a.Volume?.Split('-').Select(int.Parse),
                                     Note = a.Note
                                 };
                var result = new SearchVehicleResponse
                {
                    Page = infoSearchVehicle.PageInfo.Page,
                    PageSize = infoSearchVehicle.PageInfo.PageSize,
                    Total = infoSearchVehicle.PageInfo.Total,
                    Status = new StatusResponse
                    {
                        Content = "",
                        OK = true
                    },
                    Vehicles = allVehicle.ToList()
                };
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {

                var result = new SearchVehicleResponse
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = 0,
                    Status = new StatusResponse
                    {
                        Content = ex.Message,
                        OK = false
                    },
                    Vehicles = null
                };
                return Task.FromResult(result);
            }
          
        }

        public async Task<VehicleResponse> GetAsync()
        {
            VehicleReadModel readModel = await _vehicleService.Get();
            return new VehicleResponse
            {
                Vehicle = readModel,
                Driver = new List<DriverReadModel>
                {
                    new DriverReadModel
                    {
                        ID = 1

                    },
                    new DriverReadModel
                    {
                        ID = 2

                    }
                },
                Status = 1
            };
        }

        #endregion


        //public Task<VehicleResponse> GetById(GetVehicleRequest request)
        //{
        //    VehicleReadModel readModel = await _vehicleService.Get();
        //}
    }
}
