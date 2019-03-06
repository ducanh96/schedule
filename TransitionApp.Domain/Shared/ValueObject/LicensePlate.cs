using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Shared.ValueObject
{
    public class LicensePlate : IEquatable<LicensePlate>
    {
        public string Number { get; }
        public LicensePlate(string number)
        {
            this.Number = number;
        }

        #region Equality
        public bool Equals(LicensePlate other)
            => other != null && Number.Equals(other.Number);

        #endregion

        #region Conversion
        public static implicit operator string(LicensePlate value)
        {
            return value.Number;
        }

        public static implicit operator LicensePlate(string value)
        {
            return new LicensePlate(value);
        }
        #endregion
    }
}
