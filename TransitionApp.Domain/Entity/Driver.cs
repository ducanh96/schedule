using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Shared.ValueObject;

namespace TransitionApp.Domain.Entity
{
    public class Driver
    {
        public Identity Id { get; }
        public Name Name { get; }
        public License License { get; }
        public Identity VehicleId { get; }

        public Driver(Identity id, Name name, License license, Identity vehicleId)
        {
            Id = id;
            Name = name;
            License = license;
            VehicleId = vehicleId;
        }
    }
}
