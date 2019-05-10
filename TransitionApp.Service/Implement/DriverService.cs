using System;
using System.Collections;
using System.Collections.Generic;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Driver;
using TransitionApp.Domain.ReadModel.Driver.DAO;
using TransitionApp.Service.Interface;

namespace TransitionApp.Service.Implement
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;

        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public DriverReadModel Get(int id)
        {
            if (id == 0)
                return null;
            return _driverRepository.Get(id);
        }

        public SearchDriverReadModel GetAll(int page, int pageSize, SearchDriverModel driverModel)
        {
            return _driverRepository.GetAll(page, pageSize, driverModel);
        }

        public DriverReadModel GetByAccount(int userId)
        {
            return _driverRepository.GetByAccount(userId);
        }

        public DriverReadModel GetByCode(string code)
        {
            return _driverRepository.GetByCode(code);
        }

        public IEnumerable<CustomerReadModel> GetCustomers(DateTime date, int driverId)
        {
            return _driverRepository.GetCustomers(date, driverId);

        }
    }
}
