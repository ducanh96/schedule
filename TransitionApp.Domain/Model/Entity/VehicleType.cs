using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.Model.Entity
{
    public class VehicleType
    {
        public Code Code { get; }
        public Name Name { get; }
        public Identity Id { get; set; }
        public VehicleType(Code code, Name name)
        {
            Code = code;
            Name = name;
        }
        public VehicleType(Identity id)
        {
            Id = id;
        }
    }
}
