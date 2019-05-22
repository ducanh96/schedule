using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Invoice;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IScheduleRepository
    {
        SearchScheduleReadModel GetAll(int page, int pageSize, SearchScheduleModel scheduleModel);
        ScheduleReadModel Get(int id);
        bool Create(Schedule schedule);
        IEnumerable<RouteReadModel> GetRouteBySchedule(int scheduleId);
        IEnumerable<RouteInfoReadModel> GetRouteInfo(int routeId);
        CustomerDetailReadModel GetInforCustomerOfRoute(int customerId);
        AddressReadModel GetInformationCustomerOfRoute(int routerInfo);
        bool UpdateIsServedRouteInfo(int customerId, DateTime deliveredAt, int driverId, bool isServed);
        bool Delete(int id);
    }
}
