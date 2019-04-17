using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.VehicleType;
using TransitionApp.Service.Interface;

namespace TransitionApp.Service.Implement
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        public VehicleTypeService(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public VehicleTypeReadModel Get(int id)
        {
            return _vehicleTypeRepository.Get(id);
        }

        public IEnumerable<VehicleTypeReadModel> GetAll()
        {
            return _vehicleTypeRepository.GetAll();
        }

        public VehicleTypeReadModel GetByCode(string codeDriver)
        {
            return _vehicleTypeRepository.GetByCode(codeDriver);
        }

        public IEnumerable<VehicleTypeReadModel> GetByIds(List<int> ids)
        {
            return _vehicleTypeRepository.GetByIds(ids);
        }
    }
}
