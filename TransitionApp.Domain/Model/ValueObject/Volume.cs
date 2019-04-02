using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Volume : IEquatable<Volume>
    {
        public double Size { get; }
        public Volume(double size)
        {
            this.Size = size;
        }

        #region Equality
        public bool Equals(Volume other)
            => other != null && Size.Equals(other.Size);

        #endregion

        #region Conversion
        public static implicit operator double(Volume value)
        {
            return value.Size;
        }

        public static implicit operator Volume(double value)
        {
            return new Volume(value);
        }
        #endregion
    }
}
