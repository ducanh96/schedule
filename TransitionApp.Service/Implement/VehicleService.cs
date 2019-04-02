using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Vehicle;
using TransitionApp.Service.Interface;

namespace TransitionApp.Service.Implement
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public bool checkExist(int id)
        {
            return _vehicleRepository.checkExist(id);
        }

        public async Task<VehicleReadModel> Get()
        {
            return await _vehicleRepository.Get();
        }

        public SearchVehicleReadModel GetAll(int page, int pageSize, SearchVehicleModel vehicleModel)
        {
            return _vehicleRepository.GetAll(page, pageSize, vehicleModel);
        }

    
    }
}
