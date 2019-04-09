using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Schedule.DAO
{
    public class RouteReadModel
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int Id { get; set; }
        public double Distance { get; set; }
        public double EstimatedDistance { get; set; }
        public double EstimatedDuration { get; set; }
        public int Status { get; set; }
        public double Weight { get; set; }
        public int DriverID { get; set; } // thong tin driver info
        public string DepotAddress { get; set; }
        public string DepotLng { get; set; }
        public string DepotLat { get; set; }
        public string WarehouseId { get; set; }
        public int ScheduleId { get; set; }
    }
}
