using System.Collections;
using System.Collections.Generic;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Invoice;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;

namespace TransitionApp.Service.Interface
{
    public interface IScheduleService
    {
        SearchScheduleReadModel GetAll(int page, int pageSize, SearchScheduleModel scheduleModel);
        ScheduleReadModel Get(int id);
        IEnumerable<RouteReadModel> GetRouteBySchedule(int scheduleId);
        IEnumerable<RouteInfoReadModel> GetRouteInfo(int routeId);
        CustomerDetailReadModel GetInforCustomerOfRoute(int customerId);
        AddressReadModel GetInformationCustomerOfRoute(int routerInfo);
        IEnumerable<CustomerRouteReadModel> GetListCustomer(List<RouteInfoReadModel> routeInfoReadModels);
    }
}
