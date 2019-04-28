using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.Model.Entity
{
    public class Route
    {
        public Route(DepartureTime departureTime, Identity driverID, Distance estimatedDistance,
            Duration estimatedDuration, Status status, Weight weight, Address addressDepot,
            ArrivalTime arrivalTime, List<Customer> customers)
        {
            DepartureTime = departureTime;
            DriverID = driverID;
            EstimatedDistance = estimatedDistance;
            EstimatedDuration = estimatedDuration;
            Status = status;
            Weight = weight;
            AddressDepot = addressDepot;
            Customers = customers;
            ArrivalTime = arrivalTime;

        }

        public DepartureTime DepartureTime { get; set; }
        public Identity DriverID { get; set; }
        public Distance EstimatedDistance { get; set; }
        public Duration EstimatedDuration { get; set; }
        public Status Status { get; set; }
        public Weight Weight { get; set; }
        public Address AddressDepot { get; set; }
        public ArrivalTime ArrivalTime { get; set; }


        public List<Customer> Customers { get; set; }
    }
}
