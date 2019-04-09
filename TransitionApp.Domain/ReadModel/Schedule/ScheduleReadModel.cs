using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Schedule
{
    public class ScheduleReadModel
    {
        public int Id { get; set; }
        public string CreatedById { get; set; }
        public DateTime DeliveredAt { get; set; }
        public double EstimatedDistance { get; set; }
        public double EstimatedDuration { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int NumberOfCustomers { get; set; }
        public int RouteManagerType { get; set; }
        public int Status { get; set; }
        public double Weight { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
