using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.Vehicle
{
    public class ImportVehicleCommand : BaseCommand
    {

        public List<DataImportVehicle> Vehicles { get; set; }
        public override bool IsValid(object obj = null)
        {
            throw new NotImplementedException();
        }
    }
    public class DataImportVehicle
    {
        public string Code { get; set; }
        public int? DriverID { get; set; }
        public int ID { get; set; }
        public double MaxLoad { get; set; }
        public string Name { get; set; }
        public int? TypeVehicleID { get; set; }
        public string Note { get; set; }
        public string LicensePlate { get; set; }
        public string Volume { get; set; }
    }
}
