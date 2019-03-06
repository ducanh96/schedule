using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TransitionApp.Domain.Shared.ValueObject;

namespace TransitionApp.Domain.Entity
{
    public class Vehicle
    {
        public Identity Id { get; }
        public LicensePlate LicensePlate { get; }
        public Capacity Capacity { get; }
        public Image Image { get; }
        public TypeVehicle Type { get; }
        public Volume Volume { get; }

        public Vehicle(Identity id, LicensePlate licensePlate, Capacity capacity,
            Image image, TypeVehicle type, Volume volume)
        {
            Id = id;
            LicensePlate = licensePlate;
            Capacity = capacity;
            Image = image;
            Type = type;
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
