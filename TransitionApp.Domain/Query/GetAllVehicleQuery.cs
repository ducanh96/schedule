using MediatR;
using System.Collections.Generic;
using TransitionApp.Domain.Query.ViewModel;

namespace TransitionApp.Domain.Query
{
    public class GetAllVehicleQuery : IRequest<IEnumerable<VehicleViewModel>>
    {

    }
}
