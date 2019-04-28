using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel.Schedule
{
    public class CreateScheduleRequest
    {
        public double? EstimatedDistance { get; set; }
        public double? EstimatedDuration { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int? NumberOfCustomers { get; set; }
        public int? RouteManagerType { get; set; }
        public int? Status { get; set; }
        public double? Weight { get; set; }
        public List<Route> Routes { get; set; }
        public DateTime CreatedAt { get; set; }


    }
    public class Route
    {
        public double? DepartureTime { get; set; }
        public int? DriverID { get; set; }
        public double? EstimatedDistance { get; set; }
        public double? EstimatedDuration { get; set; }
        public int? Status { get; set; }
        public double? Weight { get; set; }
        public AddressRoute Depot { get; set; }
       
        public List<InfoCustomer> Customers { get; set; }
    }
    public class AddressRoute
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string WarehouseId { get; set; }
        public string Address { get; set; }
    }
    public class InfoCustomer
    {
        public AddressRoute Address { get; set; }
        public int CustomerID { get; set; }
        public int DriverRole { get; set; }
        public int Status { get; set; }
        public double Weight { get; set; }
        public List<int> Invoices { get; set; }

    }
}
