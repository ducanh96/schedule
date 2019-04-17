using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Volume : IEquatable<Volume>
    {
        public string Size { get; }
        public Volume(string size)
        {
            this.Size = size;
        }

        #region Equality
        public bool Equals(Volume other)
            => other != null && Size.Equals(other.Size);

        #endregion

        #region Conversion
        public static implicit operator string(Volume value)
        {
            return value.Size;
        }

        public static implicit operator Volume(string value)
        {
            return new Volume(value);
        }
        #endregion
    }
}
