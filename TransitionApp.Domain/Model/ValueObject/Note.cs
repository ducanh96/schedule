using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Note
    {
        public string Value { get; }
        public Note(string value)
        {
            Value = value;
        }
    }
}
