using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class IsDeliveried
    {
        public bool Value { get; set; }
        public IsDeliveried(bool value)
        {
            Value = value;
        }
    }
}
