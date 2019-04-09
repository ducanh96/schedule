using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.Entity;

namespace TransitionApp.Domain.ReadModel
{
    public class VehicleReadModel
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public double Volume { get; set; }
        public double Capacity { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int VehicleType { get; set; }
        public double MaxLoad { get; set; }
        public int Driver { get; set; }
        public string Note { get; set; }


    }
}
