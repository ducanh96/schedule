using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Price
    {
        public double Value { get; set; }
        public Price(double value)
        {
            Value = value;
        }
    }
}
