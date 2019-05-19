using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Role
    {
        public Role(int value)
        {
            Value = value;
        }

        public int Value { get;}
        
    }
}
