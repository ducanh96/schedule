using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.Model.Entity
{
    public class Driver
    {
        public Identity Id { get; }
        public Name Name { get; }
        public Code Code { get; }
        public PhoneNumber PhoneNumber { get; }
        public Status Status { get; set; }

        public Driver(Identity id, Name name, Code code, PhoneNumber phoneNumber, Status status)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            Status = status;
            Code = code;
        }
        public Driver(Identity id)
        {
            Id = id;
        }
    }
}
