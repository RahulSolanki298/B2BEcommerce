using System;
using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Server.ViewModals
{
    public class VirtualAppointmentVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter company name.")]
        public string CompanyName { get; set; }

        // Jewelry like [Ring, Earrings]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please enter register date.")]
        public DateTime? RegisterDate { get; set; }

        [Required(ErrorMessage = "Please enter register time.")]
        public string RegisterTime { get; set; } //  hh:mm

        [Required(ErrorMessage = "Please enter first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter email.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please enter message.")]
        public string Message { get; set; }

        public string Status { get; set; }
    }
}
