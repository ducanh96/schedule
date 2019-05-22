using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Schedule;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Application.ResponseModel.Schedule;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands.Schedule;
using TransitionApp.Domain.ReadModel.Invoice;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;
using TransitionApp.Service.Interface;

namespace TransitionApp.Application.Implement
{
    public class ScheduleAppService : IScheduleAppService
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDriverService _driverService;
        private readonly IMediatorHandler _bus;
        private readonly IInvoiceService _invoiceService;
        public ScheduleAppService(IScheduleService scheduleService, IDriverService driverService, IInvoiceService invoiceService, IMediatorHandler bus)
        {
            _scheduleService = scheduleService;
            _driverService = driverService;
            _invoiceService = invoiceService;
            _bus = bus;
        }

        public Task<BaseResponse> Create(CreateScheduleRequest request)
        {
            var response = new BaseResponse();
            try
            {
                #region Tao dữ liệu routes

                var idsInvoice = new List<int>();
                
                List<ScheduleCommand.Route> lstRoute = new List<ScheduleCommand.Route>();
                foreach (var route in request.Routes)
                {
                    // lay du lieu customer ung voi 1 route
                    List<ScheduleCommand.CustomerInfo> customerInfos = new List<ScheduleCommand.CustomerInfo>();
                    route.Customers.ForEach(x =>
                    {

                        idsInvoice.AddRange(x.Invoices);
                        ScheduleCommand.CustomerInfo customerInfo = new ScheduleCommand.CustomerInfo()
                        {
                            CustomerID = x.CustomerID,
                            Invoices = string.Join(',', x.Invoices),
                            Lat = x.Address.Lat.Value.ToString(),
                            Lng = x.Address.Lng.Value.ToString(),
                            Status = x.Status,
                            Weight = x.Weight


                        };
                        customerInfos.Add(customerInfo);
                    });

                    ScheduleCommand.Route routeData = new ScheduleCommand.Route()
                    {
                       
                        EstimatedDistance = route.EstimatedDistance,
                        EstimatedDuration = route.EstimatedDuration,
                        DepotLat = route.Depot.Lat.Value.ToString(),
                        DepotAddress = route.Depot.Address,
                        DepotLng = route.Depot.Lng.Value.ToString(),
                        DriverID = route.DriverID,
                        WarehouseId = route.Depot.WarehouseId,
                        Weight = route.Weight.Value,
                        Status = route.Status.Value,
                        CustomerInfos = customerInfos

                    };

                    lstRoute.Add(routeData);
                }
                #endregion

                CreateScheduleCommand createCommand = new CreateScheduleCommand()
                {
                    EstimatedDistance = request.EstimatedDistance,
                    EstimatedDuration = request.EstimatedDuration,
                    Name = request.Name,
                    Note = request.Note,
                    NumberOfCustomers = request.NumberOfCustomers,
                    Weight = request.Weight,
                    Status = request.Status,
                    RouteManagerType = request.RouteManagerType,
                    DeliveredAt = DateTime.ParseExact(request.Name,"dd-MM-yyyy",null),
                    Routes = lstRoute
                };
                var result = _bus.SendCommand(createCommand);

                Task<object> status = result as Task<object>;
                bool isCreate = (bool)status.Result;
                if (isCreate)
                {
                    var ipValue = "192.168.43.51";
                    IBus rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(
                        rabbit =>
                        {
                            IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri($"rabbitmq://{ipValue}/"), settings =>
                            {
                                settings.Username("tuandv");
                                settings.Password("tuandv");
                            });
                        }
                    );

                    var invoices = _invoiceService.GetInvoices(idsInvoice);
                    foreach (var item in invoices)
                    {
                        rabbitBusControl.Publish<InvoiceReadModel>(item);
                    }
                    
                }

                return Task.FromResult(new BaseResponse
                {
                   

                });


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
           
            
        }

        public Task<DeleteScheduleResponse> Delete(int id)
        {
            DeleteScheduleResponse deleteScheduleResponse = new DeleteScheduleResponse();
            DeleteScheduleCommand deleteScheduleCommand = new DeleteScheduleCommand
            {
                ID = id
            };
            var result = _bus.SendCommand(deleteScheduleCommand);
            Task<object> status = result as Task<object>;
            bool isCreate = (bool)status.Result;
            if (isCreate)
            {
                deleteScheduleResponse.Success = true;
                deleteScheduleResponse.Data = "";
            }
            else
            {
                deleteScheduleResponse.Success = true;
                deleteScheduleResponse.Data = "";
            }
            return Task.FromResult(deleteScheduleResponse);
        }


            /// <summary>
            /// lấy lịch cụ thể
            /// </summary>
            /// <param name="id">mã lịch</param>
            /// <returns></returns>

            public Task<SearchScheduleResponse> Get(int id)
        {
            var response = new SearchScheduleResponse();
            try
            {
                var schedule = _scheduleService.Get(id);
                var routers = _scheduleService.GetRouteBySchedule(schedule.Id);

                // danh sach cac routeInfo vs key la Id cua Route
                Dictionary<int, List<RouteInfoReadModel>> infoRoutes = new Dictionary<int, List<RouteInfoReadModel>>();


                routers.ToList().ForEach(x => {
                    var item = _scheduleService.GetRouteInfo(x.Id);
                    infoRoutes.Add(x.Id, item.ToList());
                });

                var resultRoute = from a in routers
                                  join b in infoRoutes on a.Id equals b.Key
                                  let driverInfo = _driverService.Get(a.DriverID)
                                  let lstRouteInfo = _scheduleService.GetRouteInfo(a.Id)
                                  select new
                                  {
                                      a.ArrivalTime,
                                      Customers = _scheduleService.GetListCustomer(lstRouteInfo.ToList()),
                                      a.DepartureTime,
                                      Depot = new
                                      {
                                          Address = a.DepotAddress,
                                          Lat = a.DepotLat,
                                          Lng = a.DepotLng,
                                          WarehouseId = a.WarehouseId
                                      },
                                      DriverInfo = new
                                      {
                                          ID = driverInfo.ID,
                                          driverInfo.Name,
                                          driverInfo.PhoneNumber,
                                          Code = driverInfo.Code
                                          
                                      },
                                      EstimatedDistance = a.EstimatedDistance,
                                      EstimatedDuration = a.EstimatedDuration,
                                      RouteID = a.Id,
                                      RouteStatus = a.Status,
                                      Weight = a.Weight
                                  };

                response.Data = new
                {
                    schedule.CreatedAt,
                    schedule.CreatedById,
                    schedule.DeliveredAt,
                    schedule.EstimatedDistance,
                    schedule.EstimatedDuration,
                    ID = schedule.Id,
                    schedule.Name,
                    schedule.NumberOfCustomers,
                    schedule.RouteManagerType,
                    schedule.Status,
                    schedule.Weight,
                    Routes = resultRoute

                };
                response.Total = 0;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return Task.FromResult(response);
        }

        


        public Task<SearchScheduleResponse> GetAll(SearchScheduleRequest request)
        {
            var response = new SearchScheduleResponse();
            try
            {
                var searchScheduleModel = new SearchScheduleModel
                {
                    DeliveredAt = request.DeliveredAt,
                    Name = request.Name,
                    Type = request.Type
                };
                var result = _scheduleService.GetAll(request.Page, request.PageSize, searchScheduleModel);
                response.Data = from a in result.Schedules
                                select new
                                {
                                    a.CreatedAt,
                                    a.CreatedById,
                                    a.DeliveredAt,
                                    a.EstimatedDistance,
                                    a.EstimatedDuration,
                                    ID = a.Id,
                                    a.Name,
                                    a.NumberOfCustomers,
                                    a.RouteManagerType,
                                    a.Weight,
                                    a.Status
                                };

                response.Page = result.PageInfo.Page;
                response.PageSize = result.PageInfo.PageSize;
                response.Total = result.PageInfo.Total;
                response.Success = true;
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
