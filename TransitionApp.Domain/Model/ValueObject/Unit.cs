using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Unit
    {
        public string Value { get; set; }
        public Unit(string value)
        {
            Value = value;
        }
    }
}
