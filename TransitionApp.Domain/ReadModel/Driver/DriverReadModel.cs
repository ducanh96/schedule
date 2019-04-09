using System;

namespace TransitionApp.Domain.ReadModel.Driver
{
    public class DriverReadModel
    {

        #region Address
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        #endregion

        #region DriverInfo

        public string Code { get; set; }
        public DateTime? DoB { get; set; }
        public string IDCardNumber { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public string Sex { get; set; }
        public DateTime? StartDate { get; set; }
        public int Status { get; set; }
        public string VehicleTypeIDs { get; set; }

        #endregion



        public int Id { get; set; }
        // thieu userID
        public int UserID { get; set; }


    }
}
