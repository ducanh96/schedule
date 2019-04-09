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
        public Status Status { get; }
        public Address Address { get;}
        public Day DoB { get; }
        public CardNumber IDCardNumber { get; }
        public Note Note { get; }
        public Sex Sex { get; }
        public Day StartDate { get; }
        public Identity UserID { get; set; }
        public VehicleTypeID VehicleTypeIDs { get; set; }

        public Driver(Identity id
            , Name name
            , Code code
            , PhoneNumber phoneNumber
            , Status status
            , Address address
            , Day dateOfBirth
            , CardNumber cardNumber
            , Note note
            , Sex sex
            , Day startDate
            , Identity userID
            , VehicleTypeID vehicleTypeID            
            )
        {
            Id = id;
            Name = name;
            Code = code;
            PhoneNumber = phoneNumber;
            Status = status;
            Address = address;
            DoB = dateOfBirth;
            IDCardNumber = cardNumber;
            Note = note;
            Sex = sex;
            StartDate = startDate;
            UserID = userID;
            VehicleTypeIDs = vehicleTypeID;
        }
        public Driver(Identity id)
        {
            Id = id;
        }
    }
}
