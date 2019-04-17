using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Schedule;
using TransitionApp.Application.ResponseModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;
using TransitionApp.Service.Interface;

namespace TransitionApp.Application.Implement
{
    public class ScheduleAppService : IScheduleAppService
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDriverService _driverService;
        public ScheduleAppService(IScheduleService scheduleService, IDriverService driverService)
        {
            _scheduleService = scheduleService;
            _driverService = driverService;
        }

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
                                          driverInfo.PhoneNumber
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
