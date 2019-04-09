using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class VehicleTypeID
    {
        public string Value { get; }
        public VehicleTypeID(string value)
        {
            Value = value;
        }
    }
}
