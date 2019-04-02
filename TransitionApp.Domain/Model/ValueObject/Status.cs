using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Status
    {
        public int Value { get; }
        public Status(int value)
        {
            Value = value;
        }
    }
}
