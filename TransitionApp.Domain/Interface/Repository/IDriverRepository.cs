using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Driver;
using TransitionApp.Domain.ReadModel.Driver.DAO;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IDriverRepository
    {
        #region Read
        SearchDriverReadModel GetAll(int page, int pageSize, SearchDriverModel driverModel);
        DriverReadModel Get(int id);
        DriverReadModel GetByCode(string code);
        #endregion

        #region Write
        DriverModel Add(Driver driver);
        DriverModel Update(Driver driver);
        bool checkExist(string code);
        bool ImportExcel(List<Driver> drivers);
        bool Delete(int id);
        DriverReadModel GetByAccount(int userId);
        IEnumerable<CustomerReadModel> GetCustomers(DateTime date, int driverId);
        #endregion
    }
}
