using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Invoice
{
    public class AddressReadModel
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
    }
}
