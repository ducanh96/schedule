using System.Collections.Generic;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;
using TransitionApp.Service.Interface;

namespace TransitionApp.Service.Implement
{
    public class ScheduleService : IScheduleService
    {
        public readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public ScheduleReadModel Get(int id)
        {
            return _scheduleRepository.Get(id);
        }

        public SearchScheduleReadModel GetAll(int page, int pageSize, SearchScheduleModel scheduleModel)
        {
            return _scheduleRepository.GetAll(page, pageSize, scheduleModel);
        }

        public CustomerDetailReadModel GetInforCustomerOfRoute(int customerId)
        {
            return _scheduleRepository.GetInforCustomerOfRoute(customerId);
        }

        public IEnumerable<RouteReadModel> GetRouteBySchedule(int scheduleId)
        {
            return _scheduleRepository.GetRouteBySchedule(scheduleId);
        }

        public IEnumerable<RouteInfoReadModel> GetRouteInfo(int routeId)
        {
            return _scheduleRepository.GetRouteInfo(routeId);
        }

        /// <summary>
        /// lay danh sach Customer
        /// </summary>
        /// <param name="routeInfoReadModels"></param>
        /// <returns></returns>
        public IEnumerable<CustomerRouteReadModel> GetListCustomer(List<RouteInfoReadModel> routeInfoReadModels)
        {
            var customerRoutes = new List<CustomerRouteReadModel>();
            foreach (var item in routeInfoReadModels)
            {
                var customer = GetInforCustomerOfRoute(item.CustomerId);
                var temp = new CustomerRouteReadModel
                {
                    Address = new
                    {
                        customer.Address.City,
                        customer.Address.Country,
                        customer.Address.District,
                        Lat = double.Parse(customer.Address.Lat),
                        Lng = double.Parse(customer.Address.Lng),
                        customer.Address.Street,
                        customer.Address.StreetNumber
                    },
                    DriverRole = new
                    {
                        GiaoHang = (item.DriverRole == 1 || item.DriverRole == 3),
                        VanChuyen = (item.DriverRole == 2 || item.DriverRole == 3)
                    },
                    ID = customer.Customer.Id,
                    IsServed = item.IsServed,
                    Name = customer.Customer.Name,
                    ServerTime = item.ServerTime,
                    TimeWindows = new
                    {
                        FromTime = item.FromTime,
                        ToTime = item.ToTime
                    }
                };
                customerRoutes.Add(temp);

            }
            return customerRoutes;
        }
    }
}
