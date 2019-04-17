using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.Model.Entity
{
    public class Vehicle
    {
        public Identity Id { get; }
        public LicensePlate LicensePlate { get; }
        public Image Image { get; }
        public Volume Volume { get; }
        public Code Code { get; }
        public Driver Driver { get; }
        public MaxLoad MaxLoad { get; }
        public Name Name { get; }
        public VehicleType  VehicleType { get; }     
        public Note Note { get; }


        public Vehicle(
            Identity id, 
            LicensePlate licensePlate,
            Driver driver,
            MaxLoad maxLoad,
            Name name,
            VehicleType vehicleType,
            Note note,
            Code code,
            Volume volume
            )
        {
            Id = id;
            LicensePlate = licensePlate;
            Code = code;
            Driver = driver;
            MaxLoad = maxLoad;
            Name = name;
            VehicleType = vehicleType;
            Note = note;
            Volume = volume;
        }
    }
    public enum TypeVehicle : int
    {
        [Display(Name = "Xe máy")]
        Motobike = 1,
        [Display(Name = "Ô tô")]
        Car = 2
    }

}
