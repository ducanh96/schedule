using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class ArrivalTime
    {
        public DateTime Value { get; }
        public ArrivalTime(DateTime value)
        {
            Value = value;
        }
    }
}
