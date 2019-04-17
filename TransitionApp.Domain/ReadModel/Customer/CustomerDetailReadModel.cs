using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.ReadModel.Invoice;

namespace TransitionApp.Domain.ReadModel.Customer
{
    public class CustomerDetailReadModel
    {
        public CustomerReadModel Customer { get; set; }
        public AddressReadModel Address { get; set; }
    }
}
