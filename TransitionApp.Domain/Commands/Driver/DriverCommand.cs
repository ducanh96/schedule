using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.Driver
{
    public abstract class DriverCommand : BaseCommand
    {
        #region Address
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }

        #endregion

        public string Code { get; set; }
        public DateTime DoB { get; set; }
        public string IDCardNumber { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public string Sex { get; set; }
        public DateTime StartDate { get; set; }
        public int Status { get; set; }
        public int UserID { get; set; }
        public string VehicleTypeIDs { get; set; }

    }

}
