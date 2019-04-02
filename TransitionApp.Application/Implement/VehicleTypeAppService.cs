using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.VehicleType;
using TransitionApp.Application.ResponseModel.VehicleType;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands.VehicleType;
using TransitionApp.Domain.Notifications;
using TransitionApp.Domain.ReadModel.VehicleType;
using TransitionApp.Service.Interface;

namespace TransitionApp.Application.Implement
{
    public class VehicleTypeAppService : IVehicleTypeAppService
    {
        //private readonly IVehicleService _vehicleService;
        private readonly IMediatorHandler _bus;
        private readonly DomainNotificationHandler _notifications;
        private readonly IVehicleTypeService _vehicleTypeService;

        public VehicleTypeAppService(IMediatorHandler bus
            , INotificationHandler<DomainNotification> notifications
            , IVehicleTypeService vehicleTypeService)
        {
            _bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
            _vehicleTypeService = vehicleTypeService;
        }


        public Task<AllVehicleTypeResponse> GetAll()
        {
            var result = new AllVehicleTypeResponse();
            try
            {
                var vehicleTypeAll = _vehicleTypeService.GetAll();
                result.Vehicletypes = vehicleTypeAll;
                result.Status = new ResponseModel.StatusResponse
                {
                    OK = true
                };
            }
            catch (Exception ex)
            {
                result.Status = new ResponseModel.StatusResponse
                {
                    Content = ex.Message,
                    OK = false
                };
            }
            return Task.FromResult(result);
        }

        #region Write
        public async Task<VehicleTypeResponse> Create(CreateVehicleTypeRequest request)
        {
            VehicleTypeResponse response = new VehicleTypeResponse();
            CreateVehicleTypeCommand vehicleTypeCommand = new CreateVehicleTypeCommand
            {
                Code = request.Code,
                Name = request.Name
            };
            try
            {
                var result = _bus.SendCommand(vehicleTypeCommand);
                Task<object> status = result as Task<object>;
                var isComplete = (bool)status.Result;
                if (isComplete)
                {
                    response.Status = new ResponseModel.StatusResponse
                    {
                        Content = "Sucess",
                        OK = true
                    };
                }
                else
                {
                    response.Status = new ResponseModel.StatusResponse
                    {
                        Content = "Error!",
                        OK = false
                    };
                }
            }
            catch (Exception ex)
            {
                response.Status = new ResponseModel.StatusResponse
                {
                    Content = ex.Message,
                    OK = false
                };
            }
            return await Task.FromResult(response);

        }

        public async Task<VehicleTypeResponse> Delete(int request)
        {
            VehicleTypeResponse response = new VehicleTypeResponse();
            DeleteVehicleTypeCommand vehicleTypeCommand = new DeleteVehicleTypeCommand
            {
                Id = request
            };
            try
            {
                var result = _bus.SendCommand(vehicleTypeCommand);
                Task<object> status = result as Task<object>;
                var isComplete = (bool)status.Result;
                if (isComplete)
                {
                    response.Status = new ResponseModel.StatusResponse
                    {
                        Content = "Sucess",
                        OK = true
                    };
                }
                else
                {
                    response.Status = new ResponseModel.StatusResponse
                    {
                        Content = "Error!",
                        OK = false
                    };
                }
            }
            catch (Exception ex)
            {
                response.Status = new ResponseModel.StatusResponse
                {
                    Content = ex.Message,
                    OK = false
                };
            }
            return await Task.FromResult(response);

        }
        #endregion


        private VehicleTypeResponse ProcessCommand(VehicleTypeCommand vehicleTypeCommand)
        {
            VehicleTypeResponse response = new VehicleTypeResponse();
            try
            {
                var result = _bus.SendCommand(vehicleTypeCommand);
                Task<object> status = result as Task<object>;
                var isComplete = (bool)status.Result;
                if (isComplete)
                {
                    response.Status = new ResponseModel.StatusResponse
                    {
                        Content = "Sucess",
                        OK = true
                    };
                }
                else
                {
                    response.Status = new ResponseModel.StatusResponse
                    {
                        Content = "Error!",
                        OK = false
                    };
                }
            }
            catch (Exception ex)
            {
                response.Status = new ResponseModel.StatusResponse
                {
                    Content = ex.Message,
                    OK = false
                };
            }
            return response;
        }
    }
}
