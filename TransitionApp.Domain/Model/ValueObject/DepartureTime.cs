using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class DepartureTime
    {
        public DateTime Value { get; }
        public DepartureTime(DateTime value)
        {
            Value = value;
        }
    }
}
