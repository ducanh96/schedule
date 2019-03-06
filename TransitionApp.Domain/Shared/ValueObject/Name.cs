using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Shared.ValueObject
{
    public class Name
    {
        public string First { get; }
        public string Last { get; }
        public string Full { get; }

        public Name(string first, string last, string full)
        {
            this.First = first;
            this.Last = last;
            this.Full = full;
        }
        public Name(string full)
        {
            this.Full = full;
        }

    }
}
