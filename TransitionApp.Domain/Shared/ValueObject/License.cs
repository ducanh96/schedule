using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Shared.ValueObject
{
    public class License : IEquatable<License>
    {
        public string Number { get; }
        public DateTime Expiration { get; }
        public License(string number, DateTime expiration)
        {
            this.Number = number;
            this.Expiration = expiration;
        }

        #region Equality
        public bool Equals(License other)
            => other != null && Number.Equals(other.Number) && Expiration == other.Expiration;

        #endregion

       
    }
}
