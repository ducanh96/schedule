using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Driver.DAO
{
    public class AddressModel
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
    }
}
