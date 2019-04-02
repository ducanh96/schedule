using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Identity: IEquatable<Identity>
    {
        public int Value { get; }
        public Identity(int id)
        {
            this.Value = id;
        }

        #region Equality
        public bool Equals(Identity other)
            => other != null && Value.Equals(other.Value);

        #endregion

        #region Conversion
        public static implicit operator int(Identity value)
        {
            return value.Value;
        }

        public static implicit operator Identity(int value)
        {
            return new Identity(value);
        }
        #endregion
    }
}
