using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class CardNumber
    {
        public string Value { get; }
        public CardNumber(string value)
        {
            Value = value;
        }
    }
}
