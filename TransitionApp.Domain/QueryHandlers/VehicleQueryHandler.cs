using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TransitionApp.Domain.Query;
using TransitionApp.Domain.Query.ViewModel;

namespace TransitionApp.Domain.QueryHandlers
{
    public class VehicleQueryHandler : IRequestHandler<GetAllVehicleQuery, IEnumerable<VehicleViewModel>>
    {
        public Task<IEnumerable<VehicleViewModel>> Handle(GetAllVehicleQuery request, CancellationToken cancellationToken)
        {
            Console.WriteLine("123456");
            IEnumerable<VehicleViewModel> vehicleAll = new List<VehicleViewModel>
            {
                new VehicleViewModel(1,"abc",12,Model.Entity.TypeVehicle.Motobike),
                new VehicleViewModel(2,"def",13,Model.Entity.TypeVehicle.Car)
            };
            return Task.FromResult(vehicleAll);
        }
    }
}
