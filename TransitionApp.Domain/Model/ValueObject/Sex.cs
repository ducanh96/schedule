using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Sex
    {
        public string Value { get; }
        public Sex(string value)
        {
            Value = value;
        }
    }
}
