using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.Schedule
{
    public abstract class ScheduleCommand : BaseCommand
    {
        public double? EstimatedDistance { get; set; }
        public double? EstimatedDuration { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int? NumberOfCustomers { get; set; }
        public int? RouteManagerType { get; set; }
        public int? Status { get; set; }
        public double? Weight { get; set; }
        public DateTime DeliveredAt { get; set; }
        public List<Route> Routes { get; set; }



        public class Route
        {
            public DateTime? DepartureTime { get; set; }
            public int? DriverID { get; set; }
            public double? EstimatedDistance { get; set; }
            public double? EstimatedDuration { get; set; }
            public int? Status { get; set; }
            public double? Weight { get; set; }
            public string DepotAddress { get; set; }
            public string DepotLat { get; set; }
            public string DepotLng { get; set; }
            public string WarehouseId { get; set; }
            public DateTime ArrivalTime { get; set; }
            public List<CustomerInfo> CustomerInfos { get; set; }
        }

        public class CustomerInfo
        {
            public int CustomerID { get; set; }
            public string Invoices { get; set; }
            public string Lat { get; set; }
            public string Lng { get; set; }
            public int Status { get; set; }
            public double Weight { get; set; }
        }
    }
    
  
}
