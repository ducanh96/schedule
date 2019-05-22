using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Driver;
using TransitionApp.Domain.ReadModel.Driver.DAO;

namespace TransitionApp.Service.Interface
{
    public interface IDriverService
    {
        SearchDriverReadModel GetAll(int page, int pageSize, SearchDriverModel driverModel);
        DriverReadModel Get(int id);
        DriverReadModel GetByCode(string code);
        DriverReadModel GetByAccount(int userId);
        IEnumerable<CustomerReadModel> GetCustomers(DateTime date, int driverId);
        bool IsServedCustomer(int customerId, DateTime date, int driverId);
        bool checkExist(string code);

    }
}
