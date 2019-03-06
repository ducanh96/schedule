using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Shared.ValueObject
{
    public class Identity: IEquatable<Identity>
    {
        public int Id { get; }
        public Identity(int id)
        {
            this.Id = id;
        }

        #region Equality
        public bool Equals(Identity other)
            => other != null && Id.Equals(other.Id);

        #endregion

        #region Conversion
        public static implicit operator int(Identity value)
        {
            return value.Id;
        }

        public static implicit operator Identity(int value)
        {
            return new Identity(value);
        }
        #endregion
    }
}
