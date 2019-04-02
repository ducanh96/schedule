using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Code
    {
        public string Value { get;}
        public Code(string value)
        {
            Value = value;
        }
    }
}
