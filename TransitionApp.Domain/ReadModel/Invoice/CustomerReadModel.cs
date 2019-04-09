namespace TransitionApp.Domain.ReadModel.Invoice
{
    public class CustomerReadModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
    }
}
