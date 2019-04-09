using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Day
    {
        public DateTime Value { get; }
        public Day(DateTime date)
        {
            Value = date;
        }
    }
}
