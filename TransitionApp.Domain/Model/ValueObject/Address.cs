using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Model.ValueObject
{
    public class Address
    {
        public string City { get; }
        public string Country { get; }
        public string District { get; }
        public string Street { get; }
        public string StreetNumber { get; }
        public Address(string city
            , string country
            , string district
            , string street
            , string streetNumber)
        {
            City = city;
            Country = country;
            District = district;
            Street = street;
            StreetNumber = streetNumber;
        }
    }
}
