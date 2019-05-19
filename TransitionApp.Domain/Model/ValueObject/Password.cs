using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Password
    {
        public Password(string value)
        {
            Value = value;
        }

        public string Value { get;}
    }
}
