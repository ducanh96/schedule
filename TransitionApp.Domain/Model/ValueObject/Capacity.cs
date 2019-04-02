using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Capacity : IEquatable<Capacity>
    {
        public double Size { get; }
        public Capacity(double size)
        {
            this.Size = size;
        }

        #region Equality
        public bool Equals(Capacity other)
            => other != null && Size.Equals(other.Size);

        #endregion

        #region Conversion
        public static implicit operator double(Capacity value)
        {
            return value.Size;
        }

        public static implicit operator Capacity(double value)
        {
            return new Capacity(value);
        }
        #endregion
    }
}
