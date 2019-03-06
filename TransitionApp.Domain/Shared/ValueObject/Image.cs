using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Shared.ValueObject
{
    public class Image : IEquatable<Image>
    {
        public string Url { get; }
        public Image(string url)
        {
            this.Url = url;
        }

        #region Equality
        public bool Equals(Image other)
            => other != null && Url.Equals(other.Url);

        #endregion

        #region Conversion
        public static implicit operator string(Image value)
        {
            return value.Url;
        }

        public static implicit operator Image(string value)
        {
            return new Image(value);
        }
        #endregion



    }
}
