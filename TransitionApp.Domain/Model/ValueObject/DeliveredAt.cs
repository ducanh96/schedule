using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class DeliveredAt
    {
        public DateTime Value { get; set; }
        public DeliveredAt(DateTime value)
        {
            Value = value;
        }
    }
}
