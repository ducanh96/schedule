using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Weight
    {
        public double Value { get; }
       
        public Weight(double value)
        {
            Value = value;
        }

        public Weight(decimal valueDecimal)
        {
            Value = Convert.ToDouble(valueDecimal);
        }
    }
}
