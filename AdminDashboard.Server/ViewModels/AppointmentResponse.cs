using AdminDashboard.Server.Entities;

namespace AdminDashboard.Server.ViewModels
{
    public class AppointmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public VirtualAppointment Data { get; set; }
    }
}
