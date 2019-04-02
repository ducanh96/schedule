using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.Driver;
using TransitionApp.Service.Interface;

namespace TransitionApp.Service.Implement
{
    public class DriverService:IDriverService
    {
        private readonly IDriverRepository _driverRepository;

        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }
        public async Task<IEnumerable<DriverReadModel>> GetAll()
        {
            return  await _driverRepository.GetAll();
        }
    }
}
