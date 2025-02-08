using Core.Entities;

namespace Core.IRepository
{
    public interface IVirtualAppointmentRepository
    {
        Task<IReadOnlyList<VirtualAppointment>> GetVirtualAppointmentListAsync();

        Task<VirtualAppointment> GetVirtualAppointmentAsync(int id);

        Task<VirtualAppointment> SaveVirtualAppointment(VirtualAppointment virtualAppointment);

        Task<bool> DeleteVirtualAppointmentAsync(int id);
    }
}
