namespace Core.Entities
{
    public class AddressMaster : BaseEntity
    {
        public string UserId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Contry { get; set; }

        public string Pincode { get; set; }

        public string AddressType { get; set; }

    }
}
