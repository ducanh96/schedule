using TransitionApp.Domain.Model.Entity;


namespace TransitionApp.Domain.Query.ViewModel
{
    public class VehicleViewModel
    {
        public int Id { get; }
        public string LicensePlate { get; }
        public double Capacity { get; }
        public TypeVehicle Type { get; }
        public VehicleViewModel(int id, string licensePlate, double capacity, TypeVehicle typeVehicle)
        {
            Id = id;
            LicensePlate = licensePlate;
            Capacity = capacity;
            Type = typeVehicle;
        }
    }
}
