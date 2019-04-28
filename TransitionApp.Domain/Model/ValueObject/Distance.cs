using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Distance
    {
        public double Value { get; }
        public Distance(double value)
        {
            Value = value;
        }
    }
}
