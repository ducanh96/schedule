using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class PhoneNumber
    {
        public string Value { get; }
        public PhoneNumber(string value)
        {
            Value = value;
        }
    }
}
