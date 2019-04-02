using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Model.Entity;

namespace TransitionApp.Domain.Commands.Vehicle
{
    public abstract class VehicleCommand : BaseCommand
    {
        //public int Id { get; set; }
        //public string LicensePlate { get; set; }
        //public double Capacity { get; set; }
        //public string Image { get; set; }
        //public TypeVehicle TypeVehicle { get; set; }
        //public double Volume { get; set; }

        public string Code { get; set; }
        public int DriverID { get; set; }
        public int ID { get; set; }
        public double MaxLoad { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public string Note { get; set; }
        public string LicensePlate { get; set; }


    }
}
