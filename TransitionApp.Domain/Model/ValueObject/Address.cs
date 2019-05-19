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
        public string Lat { get;  }
        public string Lng { get; }
        public string FullAddress { get; }
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

        public Address(string lat, string lng)
        {
            Lat = lat;
            Lng = lng;
        }

        public Address(string lat, string lng, string fullAddress) : this(lat, lng)
        {
            FullAddress = fullAddress;
        }

        public Address(string city, string country, string district, string street, string streetNumber, string lat, string lng) : this(city, country, district, street, streetNumber)
        {
            Lat = lat;
            Lng = lng;
        }
    }
}
