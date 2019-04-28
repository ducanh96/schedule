using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class IsServed
    {
        public bool Value { get; set; }
        public IsServed(bool value)
        {
            Value = value;
        }
    }
}
