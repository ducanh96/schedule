using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Vehicle
{
    public class SearchVehicleReadModel
    {
        public IEnumerable<VehicleReadModel> Vehicles { get; set; }
        public PagingReadModel PageInfo { get; set; }
    }
    
}
