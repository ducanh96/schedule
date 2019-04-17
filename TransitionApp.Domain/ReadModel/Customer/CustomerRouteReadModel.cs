using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Customer
{
    public class CustomerRouteReadModel
    {
        public object Address { get; set; }
        public object DriverRole { get; set; }
        public int ID { get; set; }
        public bool IsServed { get; set; }
        public string Name { get; set; }
        public double ServerTime { get; set; }
        public object TimeWindows { get; set; }
    }
}
