using System;

namespace OrderService.Domain.Commands.Customer
{
    public class AddNewCustomerCommand
    {
       
        public string Name { get;  set; }
        public string AddressId { get;  set; }
      
        public string PhoneNumber { get;  set; }
        public string Code { get;  set; }
        public int Status { get;  set; }
      
        public int CustomerGroupId { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime UpdatedAt { get;  set; }

        //Address
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
    }
}
