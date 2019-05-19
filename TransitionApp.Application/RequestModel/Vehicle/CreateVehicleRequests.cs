using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel.Vehicle
{
    public class CreateVehicleRequests
    {
        public string Code { get; set; }
        public int DriverID { get; set; }
        public double MaxLoad { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public string Note { get; set; }
        public string LicensePlate { get; set; }
        public List<int?> MaxVolume { get; set; }
        public string ID { get; set; }

    }
}
