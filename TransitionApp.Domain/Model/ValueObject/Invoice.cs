using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Invoice
    {
        public int Value { get; }
        public Invoice(int value)
        {
            Value = value;
        }
    }
}
