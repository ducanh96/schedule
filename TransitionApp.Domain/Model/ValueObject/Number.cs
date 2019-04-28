using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Number
    {
        public int Value { get;}
        public Number(int value)
        {
            Value = value;
        }
    }
}
