﻿namespace Core.Entities
{
    public class VirtualAppointment : BaseEntity
    {
        public string CompanyName { get; set; }

        // Jewelry like [Ring, Earrings]
        public int CategoryId { get; set; }

        public DateTime RegisterDate { get; set; }

        public string RegisterTime { get; set; } //  hh:mm

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailId { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }
    }
}
