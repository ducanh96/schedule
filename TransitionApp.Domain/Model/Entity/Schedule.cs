using System.Collections.Generic;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.Model.Entity
{
    public class Schedule
    {
        public Schedule(Distance estimatedDistance, Duration estimatedDuration, Name name,
            Note note, Number numberOfCustomers, Status routeManagerType,
            Status status, Weight weight, DeliveredAt deliveredAt, List<Route> routes)
        {
            EstimatedDistance = estimatedDistance;
            EstimatedDuration = estimatedDuration;
            Name = name;
            Note = note;
            NumberOfCustomers = numberOfCustomers;
            RouteManagerType = routeManagerType;
            Status = status;
            Weight = weight;
            Routes = routes;
            DeliveredAt = deliveredAt;
        }

        public Distance EstimatedDistance { get;  }
        public Duration EstimatedDuration { get; }
        public Name Name { get; }
        public Note Note { get;}
        public Number NumberOfCustomers { get; }
        public Status RouteManagerType { get; }
        public Status Status { get; }
        public Weight Weight { get; }
        public List<Route> Routes { get; set; }
        public DeliveredAt DeliveredAt { get; set; }

    }
}
