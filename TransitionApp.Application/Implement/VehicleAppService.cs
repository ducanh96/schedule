using MediatR;
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
                CreateVehicleCommand vehicleCommand = new CreateVehicleCommand
                {
                    LicensePlate = vehicleRequest.LicensePlate,
                    Code = vehicleRequest.Code,
                    MaxLoad = vehicleRequest.MaxLoad,
                    Name = vehicleRequest.Name,
                    Note = vehicleRequest.Note,
                    TypeID = vehicleRequest.TypeID,
                    DriverID = vehicleRequest.TypeID

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


        public BaseResponse Delete(DeleteVehicleRequest vehicleRequest)
        {
            DeleteVehicleCommand deleteVehicleCommand = new DeleteVehicleCommand
            {
                //Id = vehicleRequest.Id
            };
            var result = _bus.SendCommand(deleteVehicleCommand);
            Task<object> status = result as Task<object>;
            if ((bool)status.Result)
            {
                return new BaseResponse(ResponseCode.OK.ToString(), code: ResponseCode.OK);
            }
            return new BaseResponse(ResponseCode.Fail.ToString(), code: ResponseCode.Fail);
        }

        public BaseResponse Edit(CreateVehicleRequest vehicleRequest)
        {
            EditVehicleCommand vehicleCommand = new EditVehicleCommand
            {

                //Volume = vehicleRequest.Volume,
                //Capacity = vehicleRequest.Capacity,
                //Image = vehicleRequest.Image,
                //LicensePlate = vehicleRequest.LicensePlate,
                //TypeVehicle = vehicleRequest.TypeVehicle,
                //Id = vehicleRequest.Id
            };
            var result = _bus.SendCommand(vehicleCommand);
            Task<object> status = result as Task<object>;
            if ((bool)status.Result)
            {
                return new BaseResponse(ResponseCode.OK.ToString(), code: ResponseCode.OK);
            }
            BaseResponse response = new BaseResponse();
            if (_notifications.HasNotifications())
            {
                var message = _notifications.GetNotifications().FirstOrDefault().Value ?? "";
                response.Message = message as string;
                response.Code = (int)ResponseCode.Fail;
            }
            return response;
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
                    Type = _vehicleTypeService.Get(vehicle.VehicleType).Result,
                    Name = vehicle.Name,
                    Note = vehicle.Note
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
                                 Type = _vehicleTypeService.Get(a.VehicleType).Result,
                                 Name = a.Name,
                                 LicensePlate = a.LicensePlate,
                                 ID = a.Id,
                                 MaxLoad = a.MaxLoad
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
                        Id = 1

                    },
                    new DriverReadModel
                    {
                        Id = 2

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
