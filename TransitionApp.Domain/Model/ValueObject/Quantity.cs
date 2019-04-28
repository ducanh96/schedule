using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Quantity
    {
        public int Value { get; set; }
        public Quantity(int value)
        {
            Value = value;
        }
    }
}
