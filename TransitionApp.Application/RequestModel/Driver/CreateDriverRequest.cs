using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel.Driver
{
    public class CreateDriverRequest
    {
        public DriverRequest Driver { get; set; }

    }
    public class DriverRequest
    {
        public AddressRequest Address { get; set; }
        public string Code { get; set; }
        public string DoB { get; set; }
        public string IDCardNumber { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public string Sex { get; set; }
        public string StartDate { get; set; }
        public int Status { get; set; }
        public IEnumerable<int> VehicleTypeIDs { get; set; }
    

    }
    public class AddressRequest
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }

    }
}
