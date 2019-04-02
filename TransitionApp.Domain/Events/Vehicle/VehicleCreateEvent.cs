using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Events.Vehicle
{
    public class VehicleCreateEvent:Event
    {
        public VehicleCreateEvent(int id, string licensePlate, double volume, double capacity)
        {
            Id = id;
            LicensePlate = licensePlate;
            Volume = volume;
            Capacity = capacity;
        } 

        public int Id { get; private set; }
        public string LicensePlate { get; private set; }
        public double Volume { get; private set; }
        public double Capacity { get; private set; }
    }
}
