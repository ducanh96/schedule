using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class MaxLoad
    {
        public double Value { get; }
        public MaxLoad(double value)
        {
            Value = value;
        }
    }
}
