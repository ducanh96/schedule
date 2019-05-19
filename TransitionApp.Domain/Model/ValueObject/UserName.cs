using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class UserName
    {
        public UserName(string value)
        {
            Value = value;
        }

        public string Value { get; }

    }
}
