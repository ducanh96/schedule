using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Schedule
{
    public class RouteInfoReadModel
    {
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public int RouterId { get; set; }
        public int DriverRole { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public bool IsServed { get; set; }
        public double ServerTime { get; set; }
    

    }
}
